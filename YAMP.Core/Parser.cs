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
            add { _primary.NotificationReceived += value; }
            remove { _primary.NotificationReceived -= value; }
        }

        /// <summary>
        /// Events fired once user input is required.
        /// </summary>
        public event EventHandler<UserInputEventArgs> UserInputRequired
        {
            add { _primary.UserInputRequired += value; }
            remove { _primary.UserInputRequired -= value; }
        }

        /// <summary>
        /// Events fired once an external pause is demanded.
        /// </summary>
        public event EventHandler<PauseEventArgs> PauseDemanded
        {
            add { _primary.PauseDemanded += value; }
            remove { _primary.PauseDemanded -= value; }
        } 

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new parser.
        /// </summary>
        public Parser()
            : this(new ParseContext())
        {
        }

        /// <summary>
        /// Creates a new parser from the given context.
        /// </summary>
        /// <param name="context">The context to use.</param>
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
            query.Run(values);
            return query.Output;
		}

		#endregion
    }
}