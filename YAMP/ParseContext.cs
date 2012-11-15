/*
    Copyright (c) 2012, Florian Rappl.
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
using System.Globalization;
using System.Collections;

namespace YAMP
{
    /// <summary>
    /// Class that describes the current parse context (available functions, constants, variables, ...).
    /// </summary>
    public class ParseContext
    {
        #region Members

        IDictionary<string, Value> variables;
        IDictionary<string, IFunction> functions;
        IDictionary<string, IConstants> constants;
        ParseContext parent;
        IFormatProvider numFormat;
        int? precision = 5;
        bool isReadOnly;
        PlotValue lastPlot;

        #endregion

        #region Events

        public event EventHandler<VariableEventArgs> OnVariableChanged;
        public event EventHandler<PlotEventArgs> OnPlotChanged;
        public event EventHandler<PlotEventArgs> OnPlotCreated;

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
            this.parent = parent;

            if (parent == null)
            {
                numFormat = new CultureInfo("en-us").NumberFormat;
                precision = 5;
            }
            else
            {
                numFormat = parent.NumberFormat;
            }
        }

        #endregion

        #region Default

        static ParseContext _default;

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
        /// Gets the standard number format (en-US).
        /// </summary>
        public IFormatProvider NumberFormat
        {
            get { return numFormat; }
        }

        /// <summary>
        /// Gets the value if the context is read only (the variables cannot be altered).
        /// </summary>
        public bool IsReadOnly
        {
            get { return isReadOnly; }
        }

        /// <summary>
        /// Gets the current precision in decimal digits.
        /// </summary>
        public int Precision
        {
            get { return precision.HasValue ? precision.Value : parent.Precision; }
            internal set { precision = value; }
        }

        /// <summary>
        /// Gets the last plot added to the context.
        /// </summary>
        public PlotValue LastPlot
        {
            get { return lastPlot; }
            internal set
            {
                if (value == lastPlot)
                    return;

                lastPlot = value;
                RaisePlotCreated(value);
            }
        }

        #endregion

        #region Add Methods

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

            throw new SymbolException(name);
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

            throw new FunctionNotFoundException(name);
        }

        #endregion

        #region Variables Methods

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
            if (value != null)
            {
                if (variables.ContainsKey(name))
                    variables[name] = value;
                else
                    variables.Add(name, value);
            }
            else
            {
                if (variables.ContainsKey(name))
                    variables.Remove(name);
                else if (parent != null)
                    parent.AssignVariable(name, value);
            }

            RaiseVariableChanged(name, value);
            return this;
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

        #endregion

        #region Comfort Methods

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

        /// <summary>
        /// Runs a query asynchronously within the current context.
        /// </summary>
        /// <param name="query">
        /// The input to parse and execute.
        /// </param>
        /// <returns>The current context.</returns>
        public ParseContext RunAsync(string query)
        {
            Parser.ExecuteAsync(this, query);
            return this;
        }

        /// <summary>
        /// Runs a query asynchronously within the current context.
        /// </summary>
        /// <param name="query">
        /// The input to parse and execute.
        /// </param>
        /// <param name="continuation">
        /// The function that should be invoked after the Value has been computed.
        /// </param>
        /// <returns>The current context.</returns>
        public ParseContext RunAsync(string query, Action<QueryContext, Exception> continuation)
        {
            Parser.ExecuteAsync(this, query, continuation);
            return this;
        }

        /// <summary>
        /// Runs a query asynchronously within the current context.
        /// </summary>
        /// <param name="query">
        /// The input to parse and execute.
        /// </param>
        /// <param name="variables">
        /// The volatile variables to consider.
        /// </param>
        /// <param name="continuation">
        /// The function that should be invoked after the Value has been computed.
        /// </param>
        /// <returns>The current context.</returns>
        public ParseContext RunAsync(string query, Dictionary<string, object> variables, Action<QueryContext, Exception> continuation)
        {
            Parser.ExecuteAsync(this, query, variables, continuation);
            return this;
        }

        #endregion

        #region Event Methods

        internal void RaiseVariableChanged(string name, Value value)
        {
            if (OnVariableChanged != null)
            {
                var args = new VariableEventArgs(name, value);
                OnVariableChanged(this, args);
            }
        }

        internal void RaisePlotChanged(PlotValue plot, string property)
        {
            if (OnPlotChanged != null)
            {
                var args = new PlotEventArgs(plot, property);
                OnPlotChanged(this, args);
            }
        }

        internal void RaisePlotCreated(PlotValue plot)
        {
            if (OnPlotCreated != null)
            {
                var args = new PlotEventArgs(plot, string.Empty);
                OnPlotCreated(this, args);
            }
        }

        #endregion
    }
}
