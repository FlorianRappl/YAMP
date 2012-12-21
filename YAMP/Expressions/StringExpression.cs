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
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace YAMP
{
	class StringExpression : Expression
    {
        #region Members

        bool literal;

        #endregion

        #region ctor

        public StringExpression() : base(@"@?\"".*""")
		{
		}

        #endregion

        #region Properties

        public bool Literal
        {
            get { return literal; }
        }

        #endregion

        #region Methods

        public override Expression Create(QueryContext query, Match match)
		{
			return new StringExpression();
		}

		public override Value Interpret(Dictionary<string, Value> symbols)
		{
			return new StringValue(_input);
		}
		
		public override string Set(string input)
		{
			var escape = false;
			var sb = new StringBuilder();
            var offset = 1;

            if (input[0] == '@')
            {
                offset++;
                literal = true;
            }

			for(var i = offset; i < input.Length; i++)
			{
				if(!literal && !escape && input[i] == '\\')
					escape = true;
				else if(!escape && input[i] == '"')
				{
					_input = sb.ToString();
					return input.Length > i + 1 ? input.Substring(i + 1) : string.Empty;
				}
				else if (escape)
				{
					switch (input[i])
					{
						case 't':
							sb.Append("\t");
							break;
						case 'n':
							sb.AppendLine();
							break;
						case '\\':
						case '"':
							sb.Append(input[i]);
							break;
						default:
							throw new EscapeSequenceNotDetectedException(input[i]);
					}

					escape = false;
				}
				else
				{
					sb.Append(input[i]);
				}
			}
			
			throw new BracketException(Offset, "\"", input);
        }

        #endregion
    }
}

