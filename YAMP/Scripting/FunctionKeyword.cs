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
using System.Text.RegularExpressions;

namespace YAMP
{
    class FunctionKeyword : Keyword
    {
        /*
         * Syntax:
         * function name(arg1, arg2, ...) {
         *  // BODY
         * }
         */

        #region Members

        string[] arguments;
        string name;

        #endregion

        #region ctor

        public FunctionKeyword() : base("function", 0)
        {
        }

        public FunctionKeyword(QueryContext query) : this()
        {
            Query = query;
        }

        #endregion

        #region Methods

        public override Keyword Create(QueryContext query)
        {
            return new FunctionKeyword(query);
        }

        public override Value Run(Dictionary<string, Value> symbols)
        {
            var f = new FunctionValue(arguments, Body);
            Query.Context.AddFunction(name, f);
            return f;
        }

        public override string ParseArguments(string input)
        {
            var start = 0;
            var end = 0;

            for (; start < input.Length; start++)
            {
                if (ParseTree.IsWhiteSpace(input[start]))
                    continue;
                else if (ParseTree.IsNewLine(input[start]))
                    continue;
                
                break;
            }

            var rx = new Regex(SymbolExpression.SymbolRegularExpression);
            var match = rx.Match(input.Substring(start));

            if (!match.Success)
                throw new ParseException(Offset + start, input.Substring(start));

            name = match.Value;
            input = input.Substring(start + name.Length);

            for (; end < input.Length; end++)
            {
                if (ParseTree.IsWhiteSpace(input[end]))
                    continue;
                else if (ParseTree.IsNewLine(input[end]))
                    continue;

                break;
            }

            var bracketExpression = new ArgumentsBracketExpression(Query);
            var rest = bracketExpression.Set(input.Substring(end));

            if (string.IsNullOrEmpty(rest))
                throw new ParseException(Offset + end - rest.Length, rest);

            var args = FatArrowOperator.GetAllExpressions(bracketExpression);
            arguments = new string[args.Length];

            for (var i = 0; i < args.Length; i++)
                arguments[i] = args[i].SymbolName;
            
            return rest;
        }

        #endregion
    }
}
