namespace YAMP
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// Class that describes the current parse context (available functions, constants, variables, ...).
    /// </summary>
    public sealed class ParseContext
    {
        #region Fields

        internal static readonly ParseContext Root = new ParseContext(new Dictionary<String, Value>());

        readonly IDictionary<String, Value> _variables;
        readonly IDictionary<String, IFunction> _functions;
        readonly IDictionary<String, IConstants> _constants;
        readonly IDictionary<String, IDictionary<String, Value>> _defaultProperties;
        readonly ParseContext _parent;
        readonly Elements _elements;
        readonly Boolean _isReadOnly;

        Boolean _scripting;
        Boolean _interactive;
        Int32? _precision = 5;
        PlotValue _lastPlot;
        DisplayStyle _displayStyle;
        String _answer;

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

        /// <summary>
        /// If a new notification has been sent, this event is fired (only in interactive mode).
        /// </summary>
        public event EventHandler<NotificationEventArgs> OnNotificationReceived;

        /// <summary>
        /// If the user is required to enter something this event is fired.
        /// </summary>
        public event EventHandler<UserInputEventArgs> OnUserInputRequired;

        /// <summary>
        /// If the user is required to press a key in order to continue this event is fired.
        /// </summary>
        public event EventHandler<PauseEventArgs> OnPauseDemanded;

        #endregion

        #region ctors

        ParseContext(IDictionary<String, Value> shadowVariables)
        {
            _answer = "$";
            _isReadOnly = true;
            _variables = new ReadOnlyDictionary<String, Value>(shadowVariables);
            _functions = new Dictionary<String, IFunction>();
            _constants = new Dictionary<String, IConstants>();
            _defaultProperties = new Dictionary<String, IDictionary<String, Value>>();
            _parent = null;
            _precision = 6;
            _displayStyle = DisplayStyle.Default;
            _elements = new Elements(this);
            _elements.RegisterAssembly(this, Assembly.GetExecutingAssembly());
        }

        /// <summary>
        /// Creates a new top context.
        /// </summary>
        public ParseContext()
            : this(Root)
        {
        }

        /// <summary>
        /// Creates a new context with a custom parent (nested, i.e. more local layer).
        /// </summary>
        /// <param name="parentContext">
        /// The parent context for the new context.
        /// </param>
        public ParseContext(ParseContext parentContext)
        {
            _isReadOnly = false;
            _variables = new Dictionary<String, Value>();
            _functions = new Dictionary<String, IFunction>();
            _constants = new Dictionary<String, IConstants>();
            _defaultProperties = new Dictionary<String, IDictionary<String, Value>>();
            _parent = parentContext;
            _precision = parentContext.Precision;
            _displayStyle = parentContext._displayStyle;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the name of the last answer.
        /// </summary>
        public String Answer
        {
            get { return String.IsNullOrEmpty(_answer) ? Parent.Answer : _answer; }
            set 
            {
                if (!IsReadOnly)
                {
                    _answer = value;
                }
            }
        }

        /// <summary>
        /// Gets the elements.
        /// </summary>
        public Elements Elements
        {
            get { return _elements ?? _parent.Elements; }
        }

        /// <summary>
        /// Gets or sets of scripting should be enabled (allowed / activated).
        /// Scripting will activate loop constructs and conditionals. This will
        /// also activate file system access in the non-portable version.
        /// </summary>
        public Boolean UseScripting
        {
            get { return _scripting; }
            set { _scripting = value; }
        }

        /// <summary>
        /// Gets or sets if YAMP should run in interactive mode.
        /// The interactive mode will fire events.
        /// </summary>
        public Boolean InteractiveMode
        {
            get { return _interactive; }
            set { _interactive = value; }
        }

        /// <summary>
        /// Gets the context's parent context (NULL for the top context).
        /// </summary>
        public ParseContext Parent
        {
            get { return _parent; }
        }

        /// <summary>
        /// Gets or sets the default display style.
        /// </summary>
        public DisplayStyle DefaultDisplayStyle
        {
            get { return _displayStyle; }
            set { _displayStyle = value; }
        }

        /// <summary>
        /// Gets the constants that are present in the local context.
        /// </summary>
        public IDictionary<String, IConstants> Constants
        {
            get { return _constants; }
        }

        /// <summary>
        /// Gets all constants that are currently available in the workspace.
        /// </summary>
		public IDictionary<String, IConstants> AllConstants
        {
            get
            {
				var consts = new Dictionary<String, IConstants>(_constants);
                var top = _parent;

                while (top != null)
                {
                    foreach (var cst in top.Constants.Keys)
                    {
                        consts.Add(cst, top.Constants[cst]);
                    }

                    top = top._parent;
                }

                return consts;
            }
        }

        /// <summary>
        /// Gets the functions that are currently available in the local workspace.
        /// </summary>
        public IDictionary<String, IFunction> Functions
        {
            get { return _functions; }
        }

        /// <summary>
        /// Gets all functions that are currently available in the workspace.
        /// </summary>
        public IDictionary<String, IFunction> AllFunctions
        {
            get
            {
                var funcs = new Dictionary<String, IFunction>(_functions);
                var top = _parent;

                while (top != null)
                {
                    foreach (var function in top.Functions.Keys)
                    {
                        funcs.Add(function, top.Functions[function]);
                    }

                    top = top._parent;
                }

                return funcs;
            }
        }

        /// <summary>
        /// Gets the currently assigned (local) variables.
        /// </summary>
        public IDictionary<String, Value> Variables
        {
            get { return _variables; }
        }

        /// <summary>
        /// Gets all currently assigned variables (local and global).
        /// </summary>
        public IDictionary<String, Value> AllVariables
        {
            get
            {
                var vars = new Dictionary<String, Value>(_variables);
                var top = _parent;

                while (top != null)
                {
                    foreach (var variable in top.Variables.Keys)
                    {
                        vars.Add(variable, top.Variables[variable]);
                    }

                    top = top._parent;
                }

                return vars;
            }
        }

        /// <summary>
        /// Gets or sets if values should use a custom string representation mode where
        /// exponents are shown in times ten to the power of some superscript instead of
        /// the scientific exponential notation. 
        /// </summary>
        public Boolean CustomExponent
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the value if the context is read only (the variables cannot be altered).
        /// </summary>
        public Boolean IsReadOnly
        {
            get { return _isReadOnly; }
        }

        /// <summary>
        /// Gets or sets the current precision in decimal digits.
        /// </summary>
        public Int32 Precision
        {
            get { return _precision.HasValue ? _precision.Value : _parent.Precision; }
            set { _precision = value; }
        }

        /// <summary>
        /// Gets the last plot added to the context or the parent's context.
        /// </summary>
        public PlotValue LastPlot
        {
            get 
            {
                if (_lastPlot == null && _parent != null)
                {
                    return _parent.LastPlot;
                }

                return _lastPlot; 
            }
            internal set
            {
                if (value != _lastPlot)
                {
                    _lastPlot = value;
                    ApplyPlotTemplate(value);
                    RaiseLastPlotChanged(new PlotEventArgs(value));
                }
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
        public ParseContext AddConstant(String name, IConstants constant)
        {
            var lname = name.ToLower();
            _constants[lname] = constant;
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
        public ParseContext AddFunction(String name, IFunction func)
        {
            var lname = name.ToLower();
            _functions[lname] = func;
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
        public ParseContext RemoveConstant(String name)
        {
            var lname = name.ToLower();

            if (_constants.ContainsKey(lname))
            {
                _constants.Remove(lname);
            }

            return this;
        }

        /// <summary>
        /// Removes a function from the context.
        /// </summary>
        /// <param name="name">
        /// The name of the function.
        /// </param>
        /// <returns>The current context.</returns>
        public ParseContext RemoveFunction(String name)
        {
            var lname = name.ToLower();

            if (_functions.ContainsKey(lname))
            {
                _functions.Remove(lname);
            }

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
        public ParseContext RenameConstant(String oldName, String newName)
        {
            var lname = oldName.ToLower();

            if (_constants.ContainsKey(lname))
            {
                var buffer = _constants[lname];
                _constants.Remove(lname);
                _constants.Add(newName, buffer);
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
        public ParseContext RenameFunction(String oldName, String newName)
        {
            var lname = oldName.ToLower();

            if (_functions.ContainsKey(lname))
            {
                var buffer = _functions[lname];
                _functions.Remove(lname);
                _functions.Add(newName, buffer);
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
        public IConstants FindConstants(String name)
        {
            var lname = name.ToLower();

            if (_constants.ContainsKey(lname))
            {
                return _constants[lname];
            }

            if (_parent != null)
            {
                return _parent.FindConstants(name);
            }

            return null;
        }

        /// <summary>
        /// Finds the function instance with the specified name.
        /// </summary>
        /// <param name="name">
        /// The symbolic name to retrieve.
        /// </param>
        /// <returns>The instance of the function's class.</returns>
        public IFunction FindFunction(String name)
        {
            var lname = name.ToLower();

            if (_functions.ContainsKey(lname))
            {
                return _functions[lname];
            }

            if (_parent != null)
            {
                return _parent.FindFunction(name);
            }

            return null;
        }

        /// <summary>
        /// Tries to load a function from a given file.
        /// </summary>
        /// <param name="symbolName">
        /// The name of the function (equals the name of the file).
        /// </param>
        /// <returns>The function (if found) or NULL.</returns>
        public IFunction LoadFunction(String symbolName)
        {
            foreach (var loader in Elements.Loaders)
            {
                var function = loader.Load(symbolName);

                if (function != null)
                {
                    return function;
                }
            }

            return null;
        }

        #endregion

        #region Variables Methods

        /// <summary>
        /// Clears the list of assigned variables.
        /// </summary>
        public ParseContext Clear()
        {
            foreach (var pair in _variables)
            {
                var e = new VariableEventArgs(pair.Key, pair.Value);
                RaiseVariableRemoved(e);
            }

            _variables.Clear();
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
        public ParseContext AssignVariable(String name, Value value)
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
        static void AssignVariable(ParseContext context, String name, Value value)
        {
            var e = new VariableEventArgs(name, value);

            if (value != null)
            {
                if (context._variables.ContainsKey(name))
                {
                    context._variables[name] = value;
                    context.RaiseVariableChanged(e);
                }
                else
                {
                    context._variables.Add(name, value);
                    context.RaiseVariableCreated(e);
                }
            }
            else
            {
                if (context._variables.ContainsKey(name))
                {
                    context._variables.Remove(name);
                    context.RaiseVariableRemoved(e);
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
        public Value GetVariable(String name)
        {
            if (_variables.ContainsKey(name))
            {
                return _variables[name] as Value;
            }

            if (_parent != null)
            {
                return _parent.GetVariable(name);
            }

            return null;
        }

        /// <summary>
        /// Gets the exact context of the given variable.
        /// </summary>
        /// <param name="name">The name of the variable.</param>
        /// <returns>The context or NULL if nothing was found.</returns>
        public ParseContext GetVariableContext(String name)
        {
            if (_variables.ContainsKey(name))
            {
                return this;
            }

            if (_parent != null)
            {
                return _parent.GetVariableContext(name);
            }

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
            _lastPlot = plot;
            RaiseLastPlotChanged(new PlotEventArgs(plot));
            return this;
        }
        
        #endregion

        #region Event Methods

        /// <summary>
        /// This is raised when a variable has changed.
        /// </summary>
        /// <param name="e">The variable arguments.</param>
        internal void RaiseVariableChanged(VariableEventArgs e)
        {
            if (OnVariableChanged != null)
            {
                OnVariableChanged(this, e);
            }
        }

        /// <summary>
        /// This is raised when a variable has been created.
        /// </summary>
        /// <param name="e">The variable arguments.</param>
        internal void RaiseVariableCreated(VariableEventArgs e)
        {
            if (OnVariableCreated != null)
            {
                OnVariableCreated(this, e);
            }
        }

        /// <summary>
        /// This is raised when a variable has been removed.
        /// </summary>
        /// <param name="e">The variable arguments.</param>
        internal void RaiseVariableRemoved(VariableEventArgs e)
        {
            if (OnVariableRemoved != null)
            {
                OnVariableRemoved(this, e);
            }
        }

        /// <summary>
        /// This is raised when the last plot has been changed.
        /// </summary>
        /// <param name="e">The plot arguments.</param>
        internal void RaiseLastPlotChanged(PlotEventArgs e)
        {
            if (OnLastPlotChanged != null)
            {
                OnLastPlotChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the notification if in interactive mode.
        /// </summary>
        /// <param name="e">The notification arguments.</param>
        internal void RaiseNotification(NotificationEventArgs e)
        {
            if (InteractiveMode && OnNotificationReceived != null)
            {
                OnNotificationReceived(this, e);
            }
        }

        /// <summary>
        /// Raises the input prompt if in interactive mode.
        /// </summary>
        /// <param name="e">The input arguments.</param>
        internal void RaiseInputPrompt(UserInputEventArgs e)
        {
            if (InteractiveMode && OnUserInputRequired != null)
            {
                OnUserInputRequired(this, e);
            }
            else
            {
                e.Continue(String.Empty);
            }
        }

        /// <summary>
        /// Raises the input prompt if in interactive mode.
        /// </summary>
        /// <param name="e">The input arguments.</param>
        internal void RaisePause(PauseEventArgs e)
        {
            if (InteractiveMode && OnPauseDemanded != null)
            {
                OnPauseDemanded(this, e);
            }
            else
            {
                e.Continue();
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
        public ParseContext SetDefaultProperty(String binName, String propertyName, Value propertyValue)
        {
            if (!_defaultProperties.ContainsKey(binName))
                _defaultProperties.Add(binName, new Dictionary<String, Value>());

            var bin = _defaultProperties[binName];

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
        public ReadOnlyDictionary<String, Value> GetDefaultProperties(String binName)
        {
            if(_defaultProperties.ContainsKey(binName))
                return new ReadOnlyDictionary<String, Value>(_defaultProperties[binName]);

            return new ReadOnlyDictionary<String, Value>();
        }

        /// <summary>
        /// Applies the template set for plots.
        /// </summary>
        /// <param name="plot">The plot which will adjusted to the default values.</param>
        /// <returns>The current context.</returns>
        ParseContext ApplyPlotTemplate(PlotValue plot)
        {
            if (_defaultProperties.ContainsKey("plot"))
            {
                var bin = _defaultProperties["plot"];

                foreach (var pair in bin)
                {
                    try { SetFunction.AlterProperty(plot, pair.Key, pair.Value); }
                    catch { }
                }
            }

            if (_defaultProperties.ContainsKey("series"))
            {
                var bin = _defaultProperties["series"];

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
