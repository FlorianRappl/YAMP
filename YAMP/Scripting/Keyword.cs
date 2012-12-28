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

namespace YAMP
{
	/// <summary>
	/// Abstract base class of scripting keywords.
	/// </summary>
	abstract class Keyword : IRegisterElement
    {
        #region ctor

        public Keyword(string token, int arguments)
		{
            HasBody = true;
			Count = Math.Max(arguments, 0);
            Token = token;
		}

		#endregion

        #region Properties

        public KeywordParseTree Body { get; set; }

        public ParseTree[] Arguments { get; set; }

		public string Token { get; private set; }

		public int Count { get; private set; }

		public QueryContext Query { get; protected set; }

        public int Offset { get; set; }

        public string Input { get; protected set; }

        public bool HasBody { get; protected set; }

		#endregion

        #region Methods

		public void RegisterElement()
		{
			Elements.Instance.AddKeyword(Token, this);
		}

        public abstract Keyword Create(QueryContext query);

        public abstract Value Run(Dictionary<string, Value> symbols);

        public virtual string Parse(string input)
        {
            Input = input;
            input = ParseArguments(input);
            var rest = input;

            if (HasBody)
            {
                Body = new KeywordParseTree(Query, input, Query.Statements.CurrentLine);
                rest = ParseBody();
            }

            if (rest.Length > 0)
                Input = Input.Remove(Input.Length - rest.Length);

            if (ParseTree.GetFirstSignificantToken(rest) == Tokens.Semicolon)
                return rest;

            return ";" + rest;
        }

        public virtual string ParseArguments(string input)
        {
            if (Count != 0)
            {
                var end = 0;
                var bracketContent = GetBracketContent(input, out end);
                var pts = new List<ParseTree>();

                do
                {
                    var statement = new KeywordParseTree(Query, bracketContent, Query.Statements.CurrentLine);
                    pts.Add(statement);
                    bracketContent = statement.Rest;
                }
                while (!string.IsNullOrEmpty(bracketContent));

                if (pts.Count != Count)
                    new ParseException(Offset, Token + ". You provided the wrong number of arguments");

                Arguments = pts.ToArray();
                input = input.Substring(end + 1);
            }

            return input;
        }

        public virtual string ParseBody()
        {
            return Body.Rest ?? string.Empty;
        }

        protected string GetBracketContent(string input, out int end)
        {
            var start = input.IndexOf('(');
            var brackets = 1;

            if (start == -1)
                throw new ParseException(Token + ". You did not provide any arguments");

            if (ParseTree.Scan(input.Substring(0, start), Tokens.Whitespace, Tokens.Newline))
                throw new ParseException(Token + ". You did not provide the arguments after the keyword");

            for (var i = 1 + start; i < input.Length; i++)
            {
                if (input[i] == ')')
                    brackets--;
                else if (input[i] == '(')
                    brackets++;

                if (brackets == 0)
                {
                    end = i;
                    return input.Substring(start + 1, i - start - 1);
                }
            }

            throw new BracketException(Offset + start + Token.Length, "(", input);
        }

		#endregion
    }
}
