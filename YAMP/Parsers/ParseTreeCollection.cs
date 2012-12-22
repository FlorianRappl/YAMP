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
using System.Text;

namespace YAMP
{
    /// <summary>
    /// The collection of statement parse trees i.e. all statements of a query context
    /// </summary>
    public sealed class ParseTreeCollection
    {
        #region Members

        List<ParseTree> statements;
        QueryContext query;
        int count;
		int startLine;

        #endregion

        #region ctor

        public ParseTreeCollection(QueryContext _query)
        {
			if(Parser.EnableScripting)
				CreateParser = CreateScriptingParser;
			else
				CreateParser = CreateStandardParser;

			startLine = 1;
            count = 0;
            statements = new List<ParseTree>();
            query = _query;
        }

        #endregion

        #region Properties

		/// <summary>
		/// Gets the used query context.
		/// </summary>
        public QueryContext Query
        {
            get 
            {
                return query; 
            }
        }

        /// <summary>
        /// Gets if the current statement collection should be cancelled.
        /// </summary>
        public bool Broken
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the currently executed statement line.
        /// </summary>
        public ParseTree Current
        {
            get;
            private set;
        }

		/// <summary>
		/// Gets the start line number of the parse tree collection.
		/// </summary>
		public int StartLine
		{
			get
			{
				return startLine;
			}
		}

		/// <summary>
		/// Gets the currently parsed number of lines.
		/// </summary>
        public int Count
        {
            get
            {
                return count;
            }
        }

		/// <summary>
		/// Gets the currently parsed line.
		/// </summary>
		public int CurrentLine
		{
			get { return startLine + count; }
		}

		/// <summary>
		/// Gets the currently available statements.
		/// </summary>
		public IEnumerable<ParseTree> Statements
		{
			get
			{
				foreach (var statement in statements)
					yield return statement;
			}
		}

		Func<QueryContext, string, int, ParseTree> CreateParser
		{
			get;
			set;
		}

        #endregion

        #region Methods

		ParseTree CreateStandardParser(QueryContext query, string input, int line)
		{
			return new StatementParseTree(query, input, line);
		}

		ParseTree CreateScriptingParser(QueryContext query, string input, int line)
		{
			return new KeywordParseTree(query, input, line);
		}

        public string ReplaceComments(string input)
        {
            var chars = new char[input.Length];

            for (var i = 0; i < input.Length; i++)
            {
                var spaces = ParseTree.ReplaceComment(input.Substring(i));

                if (spaces == 0)
                    chars[i] = input[i];
                else
                {
                    for (var j = 1; j < spaces; j++)
                        chars[i++] = ' ';

                    chars[i] = ' ';
                }
            }

            return new String(chars);
        }

		/// <summary>
		/// Initializes a parse tree collection, i.e. starts parsing.
		/// </summary>
		/// <param name="input">The input to parse.</param>
		/// <param name="lineShift">The line shift to the standard line #1.</param>
		internal void Init(string input, int lineShift = 0)
		{
			var current = input;
			var line = lineShift + 1;
			startLine = line;

			do
			{
				var statement = CreateParser(query, current, line++);
				statements.Add(statement);
				count++;
				current = statement.Rest;
			}
			while (!string.IsNullOrEmpty(current));
		}

        internal void Break()
        {
            if (Current.IsBreakable)
                Broken = true;
            else if (Query.Parent != null)
                Query.Parent.Statements.Break();
        }

		/// <summary>
		/// Evaluates a parsed tree collection, i.e. starts evaluating.
		/// </summary>
		/// <param name="values">The values to consider additionally to those available in the context.</param>
		/// <returns>A value or null if nothing should be displayed.</returns>
        internal Value Run(Dictionary<string, Value> values)
        {
            Broken = false;
            Value value = null;

            foreach (var statement in statements)
            {
                if (statement.HasContent)
                {
                    Current = statement;
                    value = statement.Interpret(values);

                    if (!statement.IsAssignment)
                    {
                        if (value is ArgumentsValue)
                            value = ((ArgumentsValue)value).First();

                        Query.Context.AssignVariable("$", value);
                    }
                }
                else
                    value = null;

                if (Broken)
                    break;
            }

            return value;
        }

        public override string ToString()
        {
            var str = new string[statements.Count];

            for (var i = 0; i != statements.Count; i++)
                str[i] = statements[i].ToString();

            return string.Join(Environment.NewLine, str);
        }

        #endregion
    }
}
