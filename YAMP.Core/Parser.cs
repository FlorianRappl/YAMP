namespace YAMP
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text.RegularExpressions;

    /// <summary>
    /// The YAMP interaction class.
    /// </summary>
    public sealed class Parser
	{
		#region Fields

        readonly ParseContext _primary;

        #endregion

        #region Events

        /// <summary>
        /// Events fired once notifications are received.
        /// </summary>
        public event EventHandler<NotificationEventArgs> NotificationReceived
        {
            add { _primary.OnNotificationReceived += value; }
            remove { _primary.OnNotificationReceived -= value; }
        }

        /// <summary>
        /// Events fired once user input is required.
        /// </summary>
        public event EventHandler<UserInputEventArgs> UserInputRequired
        {
            add { _primary.OnUserInputRequired += value; }
            remove { _primary.OnUserInputRequired -= value; }
        }

        /// <summary>
        /// Events fired once an external pause is demanded.
        /// </summary>
        public event EventHandler<PauseEventArgs> PauseDemanded
        {
            add { _primary.OnPauseDemanded += value; }
            remove { _primary.OnPauseDemanded -= value; }
        } 

        #endregion

        #region ctor

        public Parser()
            : this(new ParseContext())
        {
        }

        public Parser(ParseContext context)
        {
            _primary = context;
        }

		#endregion

        #region Properties

        /// <summary>
        /// Gets or sets if YAMP should run in interactive mode.
        /// The interactive mode will fire events.
        /// </summary>
        public Boolean InteractiveMode
        {
            get { return _primary.InteractiveMode; }
            set { _primary.InteractiveMode = value; }
        }

        /// <summary>
        /// Gets or sets of scripting should be enabled (allowed / activated).
        /// Scripting will activate loop constructs and conditionals. This will
        /// also activate file system access in the non-portable version.
        /// </summary>
        public Boolean UseScripting
        {
            get { return _primary.UseScripting; }
            set { _primary.UseScripting = value; }
        }

        /// <summary>
        /// Gets a list of the available keywords.
        /// </summary>
        public String[] Keywords
        {
            get { return _primary.Elements.Keywords; }
        }

        /// <summary>
        /// Gets the version of the YAMP parser.
        /// </summary>
        public static String Version
        {
            get
            {
                var fullAssemblyName = Assembly.GetExecutingAssembly().FullName;
                var match = Regex.Match(fullAssemblyName, @"\d{1,}.\d{1,}.\d{1,}.\d{1,}");
                return match.Success ? match.Value : String.Empty;
            }
        }

		/// <summary>
		/// Gets the primary context of the parser (not the root context).
		/// </summary>
		public ParseContext Context
		{
			get { return _primary; }
		}

		#endregion

        #region Evaluation

        /// <summary>
        /// Execute the parsing of the given input.
        /// </summary>
        /// <param name="input">The input to parse.</param>
        /// <returns>The query context for further evaluation.</returns>
        public QueryContext Parse(String input)
        {
            var query = new QueryContext(_primary, input);
            query.Parser.Parse();
            return query;
        }

        /// <summary>
        /// Execute the evaluation of this parser instance without any external symbols.
        /// </summary>
        /// <param name="input">The input to evaluate.</param>
        /// <returns>The value from the evaluation.</returns>
        public Value Evaluate(String input)
        {
            return Evaluate(input, new Dictionary<String, Value>());
        }

		/// <summary>
		/// Execute the evaluation of this parser instance with external symbols.
        /// </summary>
        /// <param name="input">The input to evaluate.</param>
		/// <param name="values">
		/// The values in an Hashtable containing string (name), Value (value) pairs.
		/// </param>
		/// <returns>The value from the evaluation.</returns>
        public Value Evaluate(String input, Dictionary<String, Value> values)
        {
            var query = new QueryContext(_primary, input);
            query.Interpret(values);
            return query.Output;
		}

		/// <summary>
		/// Execute the evaluation of this parser instance with external symbols.
        /// </summary>
        /// <param name="input">The input to evaluate.</param>
		/// <param name="values">
		/// The values in an anonymous object - containing name - value pairs.
		/// </param>
		/// <returns>The value from the evaluation.</returns>
        public Value Evaluate(String input, Object values)
		{
			var symbols = new Dictionary<String, Value>();

			if (values != null)
			{
				var props = values.GetType().GetProperties();

				foreach (var prop in props)
				{
					var s = prop.GetValue(values, null);
                    var v = Convert(s);

                    if (v == null)
						throw new ArgumentException("Cannot execute YAMP queries with a list of values that contains types, which are not of a Value, numeric (int, double, float, long) or string (char, string) type.", "values");

					symbols.Add(prop.Name, v);

				}
			}

            return Evaluate(input, symbols);
		}

		#endregion

		#region Customization

		/// <summary>
		/// Adds a custom constant to the parser (to the primary context).
		/// </summary>
		/// <param name="name">
		/// The name of the symbol corresponding to the constant.
		/// </param>
		/// <param name="constant">
		/// The value of the constant.
		/// </param>
		public void AddCustomConstant(String name, Double constant)
        {
            Context.AddConstant(name, new ContainerConstant(name, new ScalarValue(constant)));
		}

		/// <summary>
		/// Adds a custom constant to the parser (to the primary context).
		/// </summary>
		/// <param name="name">
		/// The name of the symbol corresponding to the constant.
		/// </param>
		/// <param name="constant">
		/// The value of the constant.
		/// </param>
		public void AddCustomConstant(String name, Value constant)
        {
            Context.AddConstant(name, new ContainerConstant(name, constant));
		}

		/// <summary>
		/// Removes a custom constant (to the primary context).
		/// </summary>
		/// <param name="name">
		/// The name of the symbol corresponding to the constant that should be removed.
		/// </param>
		public void RemoveCustomConstant(String name)
		{
            Context.RemoveConstant(name);
		}

        /// <summary>
        /// Renames an existing constant (custom or defined).
        /// </summary>
        /// <param name="oldName">The old name of the constant.</param>
        /// <param name="newName">The new name for the constant.</param>
        public void RenameConstant(String oldName, String newName)
        {
            Context.RenameConstant(oldName, newName);
        }

        /// <summary>
        /// Renames an existing function (custom or defined).
        /// </summary>
        /// <param name="oldName">The old name of the function.</param>
        /// <param name="newName">The new name for the function.</param>
        public void RenameFunction(String oldName, String newName)
        {
            Context.RenameFunction(oldName, newName);
        }

		/// <summary>
		/// Adds a custom function to be used by the parser (to the primary context).
		/// </summary>
		/// <param name="name">
		/// The name of the symbol corresponding to the function that should be added.
		/// </param>
		/// <param name="f">
		/// The function that fulfills the signature Value f(Value v).
		/// </param>
		public void AddCustomFunction(String name, FunctionDelegate f)
        {
            Context.AddFunction(name, new ContainerFunction(name, f));
		}

		/// <summary>
		/// Removes a custom function (to the primary context).
		/// </summary>
		/// <param name="name">
		/// The name of the symbol corresponding to the function that should be removed.
		/// </param>
		public void RemoveCustomFunction(String name)
        {
            Context.RemoveFunction(name);
		}

		/// <summary>
		/// Adds a variable to be used by the parser (to the primary context).
		/// </summary>
		/// <param name="name">
		/// The name of the symbol corresponding to the variable that should be added.
		/// </param>
		/// <param name="value">
		/// The value of the variable.
		/// </param>
		public void AddVariable(String name, Value value)
        {
            Context.Variables.Add(name, value);
		}

		/// <summary>
		/// Removes a variable from the workspace (to the primary context).
		/// </summary>
		/// <param name="name">
		/// The name of the symbol corresponding to the variable that should be removed.
		/// </param>
		public void RemoveVariable(String name)
        {
            Context.Variables.Remove(name);
		}

		/// <summary>
		/// Loads an external library (assembly) that uses IFunction, Operator, ..., into the primary context.
		/// </summary>
		/// <param name="assembly">
		/// The assembly to load as a plugin.
        /// </param>
        /// <returns>The ID for the plugin.</returns>
		public Int32 LoadPlugin(Assembly assembly)
        {
            return _primary.Elements.RegisterAssembly(Context, assembly);
		}

        /// <summary>
        /// Unloads a previously loaded plugin.
        /// </summary>
        /// <param name="pluginId">The ID for the assembly to unload.</param>
        /// <returns>The primary parse context.</returns>
        public void UnloadPlugin(Int32 pluginId)
        {
            _primary.Elements.RemoveAssembly(pluginId);
        }

		#endregion

        #region Helper

        static Value Convert(Object s)
        {
            if (s is Value)
            {
                return (Value)s;
            }
            else if (s is Double || s is Int32 || s is Single || s is Int64)
            {
                return new ScalarValue((Double)s);
            }
            else if (s is String || s is Char)
            {
                return new StringValue(s.ToString());
            }

            return null;
        }

        #endregion
    }
}