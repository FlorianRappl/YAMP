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

        List<StatementParseTree> statements;
        QueryContext query;
        int count;

        #endregion

        #region ctor

        public ParseTreeCollection(QueryContext _query)
        {
            count = 1;
            statements = new List<StatementParseTree>();
            query = _query;
        }

        #endregion

        #region Properties

        public QueryContext Query
        {
            get 
            {
                return query; 
            }
        }

        public int Count
        {
            get
            {
                return count;
            }
        }

        #endregion

        #region Methods

        internal void AddStatement(string input)
        {
            var line = count++;
            var statement = new StatementParseTree(query, input, line);
            statements.Insert(0, statement);
        }

        internal Value Run(Dictionary<string, Value> values)
        {
            Value value = null;

            foreach (var statement in statements)
            {
                if (statement.HasContent)
                {
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
            }

            return value;
        }

        public override string ToString()
        {
            var str = new string[statements.Count];

            for (var i = 0; i != statements.Count; i++)
            {
                str[i] = statements[i].ToString();
            }

            return string.Join(Environment.NewLine, str);
        }

        #endregion
    }
}
