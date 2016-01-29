namespace YAMP
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents the context that is used for the current input query.
    /// </summary>
    public class QueryContext
	{
		#region Fields

        readonly ParseEngine parser;
        readonly Dictionary<String, IFunction> functionBuffer;

        String _input;
        Boolean _stop;
        Statement currentStatement;

		#endregion

		#region ctor

		/// <summary>
		/// Creates a new query context.
		/// </summary>
		/// <param name="input">The input to parse</param>
		public QueryContext(String input)
		{
			Input = input;
            parser = new ParseEngine(this);
            functionBuffer = new Dictionary<String, IFunction>();
		}

		/// <summary>
		/// Creates a new (underlying) QueryContext
		/// </summary>
		/// <param name="query">The query context to copy</param>
		internal QueryContext(QueryContext query)
            : this(query.Input)
		{
			Parent = query;
            parser.Parent = query.Parser;
			Context = query.Context;
		}

		/// <summary>
		/// Just a stupid dummy!
		/// </summary>
		private QueryContext()
		{
		}

		/// <summary>
		/// Creates a dummy context that just holds the given ParseContext.
		/// </summary>
		/// <param name="context">The ParseContext to contain</param>
		/// <returns>A new (dummy) QueryContext</returns>
		public static QueryContext Dummy(ParseContext context)
		{
			var query = new QueryContext();
			query.Context = context;
			return query;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the parent of this query context.
		/// </summary>
		public QueryContext Parent
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the result in a string representation.
		/// </summary>
		public String Result
		{
			get
			{
				if (Output == null)
					return String.Empty;

				return Output.ToString(Context);
			}
		}

		/// <summary>
		/// Gets or sets the input that is being used by the parser.
		/// </summary>
		public String Input
		{
			get { return _input; }
			set
			{
				_input = value ?? String.Empty;
			}
		}

		/// <summary>
		/// Gets a boolean indicating whether the result should be printed.
		/// </summary>
		public Boolean IsMuted
		{
			get { return Output == null; }
		}

		/// <summary>
		/// Gets the result of the query.
		/// </summary>
		public Value Output { get; internal set; }

		/// <summary>
		/// Gets the context used for this query.
		/// </summary>
		public ParseContext Context { get; internal set; }

		/// <summary>
		/// Gets the statements generated for this query.
		/// </summary>
		public IEnumerable<Statement> Statements
		{
			get { return parser.Statements; }
		}

        /// <summary>
        /// Gets the currently executed statement.
        /// </summary>
        public Statement CurrentStatement
        {
            get { return currentStatement; }
        }

        /// <summary>
        /// Gets the parser for this query.
        /// </summary>
        public ParseEngine Parser
        {
            get { return parser; }
        }

		#endregion

        #region Methods

        internal IFunction GetFromBuffer(String functionName)
        {
            if (functionBuffer.ContainsKey(functionName))
                return functionBuffer[functionName];

            return null;
        }

        internal void SetToBuffer(String functionName, IFunction function)
        {
            if (functionBuffer.ContainsKey(functionName))
                functionBuffer[functionName] = function;
            else
                functionBuffer.Add(functionName, function);
        }

        /// <summary>
        /// Begins the interpretation of the current parse tree.
        /// </summary>
        /// <param name="values">A dictionary with additional symbols to consider.</param>
		internal void Interpret(Dictionary<String, Value> values)
		{
            if (!parser.CanRun)
            {
                if (!parser.IsParsed)
                    parser.Parse();

                if (parser.HasErrors)
                    throw new YAMPParseException(parser);
            }

            currentStatement = null;
            _stop = false;

            foreach (var statement in parser.Statements)
            {
                if (_stop)
                    break;

                currentStatement = statement;
                Output = statement.Interpret(values);
            }

            if (currentStatement != null && Output != null)
            {
                if (!currentStatement.IsAssignment)
                {
                    if (Output is ArgumentsValue)
                        Output = ((ArgumentsValue)Output).First();

                    Context.AssignVariable(YAMP.Parser.answer, Output);
                }
            }
		}

        /// <summary>
        /// Stops the current interpretation.
        /// </summary>
        internal void Stop()
        {
            _stop = true;
        }

        #endregion

        #region String Representation

        /// <summary>
        /// Outputs the string representation of the query context.
        /// </summary>
        /// <returns>A string variable.</returns>
		public override String ToString()
		{
			return String.Format("{0} = {1}{2}", Input, Environment.NewLine, Output);
		}

		#endregion
    }
}