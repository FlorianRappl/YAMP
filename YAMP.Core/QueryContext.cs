namespace YAMP
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using YAMP.Exceptions;

    /// <summary>
    /// Represents the context that is used for the current input query.
    /// </summary>
    public class QueryContext
	{
		#region Fields

        readonly ParseEngine _parser;
        readonly Dictionary<String, IFunction> _functionBuffer;
        readonly ParseContext _context;
        readonly String _input;

        Boolean _stop;
        Statement _current;

		#endregion

		#region ctor

		/// <summary>
		/// Creates a new query context.
		/// </summary>
        /// <param name="context">The context to reference.</param>
		/// <param name="input">The input to parse</param>
		public QueryContext(ParseContext context, String input)
            : this(context)
		{
			_input = input;
            _parser = new ParseEngine(this, context);
            _functionBuffer = new Dictionary<String, IFunction>();
		}

		/// <summary>
		/// Creates a new (underlying) QueryContext
		/// </summary>
		/// <param name="query">The query context to copy</param>
        /// <param name="input">The new input to use.</param>
		internal QueryContext(QueryContext query, String input)
            : this(query._context, input)
		{
			Parent = query;
            _parser.Parent = query.Parser;
		}

		/// <summary>
		/// Just a stupid dummy!
		/// </summary>
        private QueryContext(ParseContext context)
        {
            _context = context;
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
			get { return Output != null ? Output.ToString(Context) : String.Empty; }
		}

		/// <summary>
		/// Gets or sets the input that is being used by the parser.
		/// </summary>
		public String Input
		{
			get { return _input; }
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
		public Value Output 
        { 
            get; 
            internal set; 
        }

		/// <summary>
		/// Gets the context used for this query.
		/// </summary>
		public ParseContext Context
        {
            get { return _context; }
        }

		/// <summary>
		/// Gets the statements generated for this query.
		/// </summary>
		public IEnumerable<Statement> Statements
		{
			get { return _parser.Statements; }
		}

        /// <summary>
        /// Gets the currently executed statement.
        /// </summary>
        public Statement CurrentStatement
        {
            get { return _current; }
        }

        /// <summary>
        /// Gets the parser for this query.
        /// </summary>
        public ParseEngine Parser
        {
            get { return _parser; }
        }

        /// <summary>
        /// Gets the used (global) variables.
        /// </summary>
        public IEnumerable<VariableInfo> Variables
        {
            get 
            {
                var variables = new List<VariableInfo>();

                foreach (var statement in _parser.Statements)
                {
                    var container = statement.Container;
                    var symbols = container.GetGlobalSymbols();

                    foreach (var symbol in symbols)
                    {
                        var name = symbol.SymbolName;
                        var context = symbol.Context.GetSymbolContext(name);

                        if (!variables.Any(m => m.Context == context && m.Name.Equals(name)))
                        {
                            var assigned = statement.IsAssignment && container.IsAssigned(symbol);
                            var variable = new VariableInfo(name, assigned, context);
                            variables.Add(variable);
                        }
                    }
                }

                return variables;
            }
        }

		#endregion

        #region Methods

        internal IFunction GetFromBuffer(String functionName)
        {
            if (_functionBuffer.ContainsKey(functionName))
            {
                return _functionBuffer[functionName];
            }

            return null;
        }

        internal void SetToBuffer(String functionName, IFunction function)
        {
            _functionBuffer[functionName] = function;
        }

        /// <summary>
        /// Begins the interpretation of the current parse tree.
        /// </summary>
        /// <param name="values">A dictionary with additional symbols to consider.</param>
		public void Run(Dictionary<String, Value> values)
		{
            if (!_parser.CanRun)
            {
                if (!_parser.IsParsed)
                {
                    _parser.Parse();
                }

                if (_parser.HasErrors)
                {
                    throw new YAMPParseException(_parser);
                }
            }

            _current = null;
            _stop = false;

            foreach (var statement in _parser.Statements)
            {
                if (_stop)
                {
                    break;
                }

                _current = statement;
                Output = statement.Interpret(values);
            }

            if (_current != null && Output != null)
            {
                if (!_current.IsAssignment)
                {
                    if (Output is ArgumentsValue)
                    {
                        Output = ((ArgumentsValue)Output).First();
                    }

                    Context.AssignVariable(_context.Answer, Output);
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