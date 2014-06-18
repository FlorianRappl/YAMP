/*
    Copyright (c) 2012-2014, Florian Rappl.
    All rights reserved.

    Redistribution and use in source and binary forms, with or without
    modification, are permitted provided that the following conditions are met:
        * Redistributions of source code must retain the above copyright
          notice, this list of conditions and the following disclaimer.
        * Redistributions in binary form must reproduce the above copyright
          notice, this list of conditions and the following disclaimer in the
          documentation and/or other materials provided with the distribution.
        * Neither the name of the YAMP team nor the names of its contributors
          may be used to endorse or promote products derived from this
          software without specific prior written permission.

    THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
    ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
    WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
    DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
    DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
    (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
    LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
    ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
    (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
    SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System;
using System.Collections.Generic;
using System.Collections;

namespace YAMP
{
    /// <summary>
    /// Class that describes the current parse context (available functions, constants, variables, ...).
    /// </summary>
    public sealed partial class ParseContext : BaseParseContext
    {
        #region Members

        IDictionary<string, Value> variables;
        IDictionary<string, IFunction> functions;
        IDictionary<string, IConstants> constants;
        IDictionary<string, IDictionary<string, Value>> defaultProperties;
        ParseContext parent;
        int? precision = 5;
        bool isReadOnly;
        PlotValue lastPlot;
        DisplayStyle displayStyle;

        #endregion

        #region Events

        /// <summary>
        /// If an existing variable changed, this event is executed.
        /// </summary>
        public event EventHandler<VariableEventArgs> OnVariableChanged;

        /// <summary>
        /// If a new variable is added, this event is executed.
        /// </summary>
        public event EventHandler<VariableEventArgs> OnVariableCreated;

        /// <summary>
        /// If an existing variable is removed, this event is executed.
        /// </summary>
        public event EventHandler<VariableEventArgs> OnVariableRemoved;

        /// <summary>
        /// If the last plot variable is changed, this event is executed.
        /// </summary>
        public event EventHandler<PlotEventArgs> OnLastPlotChanged;

        #endregion

        #region ctors

        /// <summary>
        /// Creates a new (fresh) context with the default context as parent.
        /// </summary>
        public ParseContext() : this(_default)
        {
        }

        /// <summary>
        /// Creates a new context with a custom parent (nested, i.e. more local layer).
        /// </summary>
        /// <param name="parent">
        /// The parent context for the new context.
        /// </param>
        public ParseContext(ParseContext parent)
        {
            isReadOnly = false;
            variables = new Dictionary<string, Value>();
            functions = new Dictionary<string, IFunction>();
            constants = new Dictionary<string, IConstants>();
            defaultProperties = new Dictionary<string, IDictionary<string, Value>>();
            this.parent = parent;

            if (parent == null)
            {
                precision = 6;
                displayStyle = DisplayStyle.Default;
            }
            else
            {
                precision = parent.Precision;
                displayStyle = parent.displayStyle;
            }
        }

        #endregion

        #region Default

        static ParseContext _default;

        /// <summary>
        /// Gets the default (root) parse context.
        /// </summary>
        internal static ParseContext Default
        {
            get
            {
                if (_default == null)
                {
                    _default = new ParseContext();
                    _default.isReadOnly = true;
                    _default.variables = new ReadOnlyDictionary<string, Value>(_default.variables);
                }

                return _default;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the context's parent context (NULL for the top context).
        /// </summary>
        public ParseContext Parent
        {
            get { return parent; }
        }

        /// <summary>
        /// Gets or sets the default display style.
        /// </summary>
        public DisplayStyle DefaultDisplayStyle
        {
            get { return displayStyle; }
            set { displayStyle = value; }
        }

        /// <summary>
        /// Gets the constants that are present in the local context.
        /// </summary>
        public IDictionary<string, IConstants> Constants
        {
            get { return constants; }
        }

        /// <summary>
        /// Gets all constants that are currently available in the workspace.
        /// </summary>
		public IDictionary<string, IConstants> AllConstants
        {
            get
            {
				var consts = new Dictionary<string, IConstants>(constants);
                var top = parent;

                while (top != null)
                {
                    foreach (var cst in top.Constants.Keys)
                        consts.Add(cst, top.Constants[cst]);

                    top = top.parent;
                }

                return consts;
            }
        }

        /// <summary>
        /// Gets the functions that are currently available in the local workspace.
        /// </summary>
        public IDictionary<string, IFunction> Functions
        {
            get { return functions; }
        }

        /// <summary>
        /// Gets all functions that are currently available in the workspace.
        /// </summary>
        public IDictionary<string, IFunction> AllFunctions
        {
            get
            {
                var funcs = new Dictionary<string, IFunction>(functions);
                var top = parent;

                while (top != null)
                {
                    foreach (var function in top.Functions.Keys)
                        funcs.Add(function, top.Functions[function]);

                    top = top.parent;
                }

                return funcs;
            }
        }

        /// <summary>
        /// Gets the currently assigned (local) variables.
        /// </summary>
        public IDictionary<string, Value> Variables
        {
            get { return variables; }
        }

        /// <summary>
        /// Gets all currently assigned variables (local and global).
        /// </summary>
        public IDictionary<string, Value> AllVariables
        {
            get
            {
                var vars = new Dictionary<string, Value>(variables);
                var top = parent;

                while (top != null)
                {
                    foreach (var variable in top.Variables.Keys)
                        vars.Add(variable, top.Variables[variable]);

                    top = top.parent;
                }

                return vars;
            }
        }

        /// <summary>
        /// Gets or sets if values should use a custom string representation mode where
        /// exponents are shown in times ten to the power of some superscript instead of
        /// the scientific exponential notation. 
        /// </summary>
        public bool CustomExponent
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the value if the context is read only (the variables cannot be altered).
        /// </summary>
        public bool IsReadOnly
        {
            get { return isReadOnly; }
        }

        /// <summary>
        /// Gets or sets the current precision in decimal digits.
        /// </summary>
        public int Precision
        {
            get { return precision.HasValue ? precision.Value : parent.Precision; }
            set { precision = value; }
        }

        /// <summary>
        /// Gets the last plot added to the context or the parent's context.
        /// </summary>
        public PlotValue LastPlot
        {
            get 
            {
                if (lastPlot == null && parent != null)
                    return parent.LastPlot;

                return lastPlot; 
            }
            internal set
            {
                if (value == lastPlot)
                    return;

                lastPlot = value;
                ApplyPlotTemplate(value);
                RaiseLastPlotChanged(value);
            }
        }

        #endregion

        #region Add elements

        /// <summary>
        /// Adds a constant to the context.
        /// </summary>
        /// <param name="name">
        /// The name of the constant.
        /// </param>
        /// <param name="constant">
        /// The class instance of the constant.
        /// </param>
        /// <returns>The current context.</returns>
        public ParseContext AddConstant(string name, IConstants constant)
        {
            var lname = name.ToLower();

            if (constants.ContainsKey(lname))
                constants[lname] = constant;
            else
                constants.Add(lname, constant);

            return this;
        }

        /// <summary>
        /// Adds a function to the context.
        /// </summary>
        /// <param name="name">
        /// The name of the function.
        /// </param>
        /// <param name="func">
        /// The IFunction instance to add.
        /// </param>
        /// <returns>The current context.</returns>
        public ParseContext AddFunction(string name, IFunction func)
        {
            var lname = name.ToLower();

            if (functions.ContainsKey(lname))
                functions[lname] = func;
            else
                functions.Add(lname, func);

            return this;
        }

        #endregion

        #region Remove elements

        /// <summary>
        /// Removes a constant from the context.
        /// </summary>
        /// <param name="name">
        /// The name of the constant.
        /// </param>
        /// <returns>The current context.</returns>
        public ParseContext RemoveConstant(string name)
        {
            var lname = name.ToLower();

            if (constants.ContainsKey(lname))
                constants.Remove(lname);

            return this;
        }

        /// <summary>
        /// Removes a function from the context.
        /// </summary>
        /// <param name="name">
        /// The name of the function.
        /// </param>
        /// <returns>The current context.</returns>
        public ParseContext RemoveFunction(string name)
        {
            var lname = name.ToLower();
            
            if (functions.ContainsKey(lname))
                functions.Remove(lname);

            return this;
        }

        #endregion

        #region Rename elements

        /// <summary>
        /// Renames a constant from the context.
        /// </summary>
        /// <param name="oldName">
        /// The old name of the constant.
        /// </param>
        /// <param name="newName">
        /// The new name for the constant.
        /// </param>
        /// <returns>The current context.</returns>
        public ParseContext RenameConstant(string oldName, string newName)
        {
            var lname = oldName.ToLower();

            if (constants.ContainsKey(lname))
            {
                var buffer = constants[lname];
                constants.Remove(lname);
                constants.Add(newName, buffer);
            }

            return this;
        }

        /// <summary>
        /// Renames a function from the context.
        /// </summary>
        /// <param name="oldName">
        /// The old name of the function.
        /// </param>
        /// <param name="newName">
        /// The new name for the function.
        /// </param>
        /// <returns>The current context.</returns>
        public ParseContext RenameFunction(string oldName, string newName)
        {
            var lname = oldName.ToLower();

            if (functions.ContainsKey(lname))
            {
                var buffer = functions[lname];
                functions.Remove(lname);
                functions.Add(newName, buffer);
            }

            return this;
        }

        #endregion

        #region Find Methods

        /// <summary>
        /// Finds the constant with the specified name.
        /// </summary>
        /// <param name="name">
        /// The symbolic name to retrieve.
        /// </param>
        /// <returns>The value of the constant.</returns>
        public IConstants FindConstants(string name)
        {
            var lname = name.ToLower();

            if (constants.ContainsKey(lname))
                return constants[lname];

            if (parent != null)
                return parent.FindConstants(name);

            return null;
        }

        /// <summary>
        /// Finds the function instance with the specified name.
        /// </summary>
        /// <param name="name">
        /// The symbolic name to retrieve.
        /// </param>
        /// <returns>The instance of the function's class.</returns>
        public IFunction FindFunction(string name)
        {
            var lname = name.ToLower();

            if (functions.ContainsKey(lname))
                return functions[lname];

            if (parent != null)
                return parent.FindFunction(name);

            return null;
        }

        #endregion

        #region Variables Methods

        /// <summary>
        /// Clears the list of assigned variables.
        /// </summary>
        public ParseContext Clear()
        {
            foreach (var pair in variables)
                RaiseVariableRemoved(pair.Key, pair.Value);

            variables.Clear();
            return this;
        }

        /// <summary>
        /// Assigns a value to a symbolic name.
        /// </summary>
        /// <param name="name">
        /// The name to assign a value to.
        /// </param>
        /// <param name="value">
        /// The value of the symbol.
        /// </param>
        /// <returns>The current context.</returns>
        public ParseContext AssignVariable(string name, Value value)
        {
            var context = GetVariableContext(name);

            if (context == null)
                context = this;

            AssignVariable(context, name, value);
            return this;
        }

        /// <summary>
        /// Assigns a variable to the given context.
        /// </summary>
        /// <param name="context">The context, where to assign the variable to.</param>
        /// <param name="name">The name of the variable.</param>
        /// <param name="value">The value of the variable.</param>
        static void AssignVariable(ParseContext context, string name, Value value)
        {
            if (value != null)
            {
                if (context.variables.ContainsKey(name))
                {
                    context.variables[name] = value;
                    context.RaiseVariableChanged(name, value);
                }
                else
                {
                    context.variables.Add(name, value);
                    context.RaiseVariableCreated(name, value);
                }
            }
            else
            {
                if (context.variables.ContainsKey(name))
                {
                    context.variables.Remove(name);
                    context.RaiseVariableRemoved(name, value);
                }
            }
        }

        /// <summary>
        /// Gets the value with the specific symbolic name.
        /// </summary>
        /// <param name="name">
        /// The variable's name.
        /// </param>
        /// <returns>The value of the variable or null.</returns>
        public Value GetVariable(string name)
        {
            if (variables.ContainsKey(name))
                return variables[name] as Value;

            if (parent != null)
                return parent.GetVariable(name);

            return null;
        }

        /// <summary>
        /// Gets the exact context of the given variable.
        /// </summary>
        /// <param name="name">The name of the variable.</param>
        /// <returns>The context or NULL if nothing was found.</returns>
        public ParseContext GetVariableContext(string name)
        {
            if (variables.ContainsKey(name))
                return this;

            if (parent != null)
                return parent.GetVariableContext(name);

            return null;
        }

        #endregion

        #region Comfort Methods

        /// <summary>
        /// Sets the lastplot to be used to the given value.
        /// </summary>
        /// <param name="plot">The plot to change to.</param>
        /// <returns>The current context.</returns>
        public ParseContext ChangeLastPlotTo(PlotValue plot)
        {
            lastPlot = plot;
            RaiseLastPlotChanged(plot);
            return this;
        }

        /// <summary>
        /// Runs a query within the current context.
        /// </summary>
        /// <param name="query">
        /// The input to parse and execute.
        /// </param>
        /// <returns>The current context.</returns>
        public QueryContext Run(string query)
        {
            var parser = Parser.Parse(this, query);
            parser.Execute();
            return parser.Context;
        }

        /// <summary>
        /// Runs a query within the current context.
        /// </summary>
        /// <param name="query">
        /// The input to parse and execute.
        /// </param>
        /// <param name="variables">
        /// The volatile variables to consider.
        /// </param>
        /// <returns>The current context.</returns>
        public QueryContext Run(string query, Dictionary<string, object> variables)
        {
            var parser = Parser.Parse(this, query);
            parser.Execute(variables);
            return parser.Context;
        }
        
        #endregion

        #region Event Methods

        /// <summary>
        /// This is raised when a variable has changed.
        /// </summary>
        /// <param name="name">The name of the variable.</param>
        /// <param name="value">The value of the variable.</param>
        internal void RaiseVariableChanged(string name, Value value)
        {
            if (OnVariableChanged != null)
            {
                var args = new VariableEventArgs(name, value);
                OnVariableChanged(this, args);
            }
        }

        /// <summary>
        /// This is raised when a variable has been created.
        /// </summary>
        /// <param name="name">The name of the variable.</param>
        /// <param name="value">The value of the variable.</param>
        internal void RaiseVariableCreated(string name, Value value)
        {
            if (OnVariableCreated != null)
            {
                var args = new VariableEventArgs(name, value);
                OnVariableCreated(this, args);
            }
        }

        /// <summary>
        /// This is raised when a variable has been removed.
        /// </summary>
        /// <param name="name">The name of the variable.</param>
        /// <param name="value">The value of the variable.</param>
        internal void RaiseVariableRemoved(string name, Value value)
        {
            if (OnVariableRemoved != null)
            {
                var args = new VariableEventArgs(name, value);
                OnVariableRemoved(this, args);
            }
        }

        /// <summary>
        /// This is raised when the last plot has been changed.
        /// </summary>
        /// <param name="plot"></param>
        internal void RaiseLastPlotChanged(PlotValue plot)
        {
            if (OnLastPlotChanged != null)
            {
                var args = new PlotEventArgs(plot, string.Empty);
                OnLastPlotChanged(this, args);
            }
        }

        #endregion

        #region Template Properties

        /// <summary>
        /// Sets a template property in the dictionary.
        /// </summary>
        /// <param name="binName">The category of the default property.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="propertyValue">The default value of the property.</param>
        /// <returns>The current context.</returns>
        public ParseContext SetDefaultProperty(string binName, string propertyName, Value propertyValue)
        {
            if (!defaultProperties.ContainsKey(binName))
                defaultProperties.Add(binName, new Dictionary<string, Value>());

            var bin = defaultProperties[binName];

            if (propertyValue != null)
            {
                if (bin.ContainsKey(propertyName))
                    bin[propertyName] = propertyValue;
                else
                    bin.Add(propertyName, propertyValue);
            }
            else if (bin.ContainsKey(propertyName))
                bin.Remove(propertyName);

            return this;
        }

        /// <summary>
        /// Gets the key value pairs of the specified bin.
        /// </summary>
        /// <param name="binName">The name of the template bin.</param>
        /// <returns>The read only key value pairs.</returns>
        public ReadOnlyDictionary<string, Value> GetDefaultProperties(string binName)
        {
            if(defaultProperties.ContainsKey(binName))
                return new ReadOnlyDictionary<string, Value>(defaultProperties[binName]);

            return new ReadOnlyDictionary<string, Value>();
        }

        /// <summary>
        /// Applies the template set for plots.
        /// </summary>
        /// <param name="plot">The plot which will adjusted to the default values.</param>
        /// <returns>The current context.</returns>
        ParseContext ApplyPlotTemplate(PlotValue plot)
        {
            if (defaultProperties.ContainsKey("plot"))
            {
                var bin = defaultProperties["plot"];

                foreach (var pair in bin)
                {
                    try { SetFunction.AlterProperty(plot, pair.Key, pair.Value); }
                    catch { }
                }
            }

            if (defaultProperties.ContainsKey("series"))
            {
                var bin = defaultProperties["series"];

                foreach (var pair in bin)
                {
                    try 
                    {
                        for(var i = 0; i < plot.Count; i++)
                            SetFunction.AlterSeriesProperty(plot, i, pair.Key, pair.Value); 
                    }
                    catch { }
                }
            }

            return this;
        }

        #endregion
    }
}
