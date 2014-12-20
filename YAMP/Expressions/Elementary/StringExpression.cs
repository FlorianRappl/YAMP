/*
	Copyright (c) 2012-2014, Florian Rappl.
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

namespace YAMP
{
    /// <summary>
    /// Presents the class for string expressions "...".
    /// </summary>
	class StringExpression : Expression
    {
        #region Members

        bool literal;
        string value;

        #endregion

        #region ctor

        public StringExpression()
		{
		}

        public StringExpression(string content)
        {
            value = content;
        }

        public StringExpression(ParseEngine engine) : base(engine)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value if the string literal (@) was used.
        /// </summary>
        public bool IsLiteral
        {
            get { return literal; }
        }

        #endregion

        #region Methods

		public override Value Interpret(Dictionary<string, Value> symbols)
		{
            return new StringValue(value);
		}

        public override Expression Scan(ParseEngine engine)
        {
            var chars = engine.Characters;
            var start = engine.Pointer;

            if (chars[start] == '"' || (chars[start] == '@' && start + 1 < chars.Length && chars[start + 1] == '"'))
            {
                var index = start;
                var exp = new StringExpression(engine);
                var escape = false;
                var terminated = false;
                var sb = new StringBuilder();

                if (chars[index] == '@')
                {
                    index += 2;
                    exp.literal = true;
                }
                else
                    index++;

                while (index < chars.Length)
                {
                    if (!literal && !escape && chars[index] == '\\')
                        escape = true;
                    else if (!escape && chars[index] == '"')
                    {
                        terminated = true;
                        index++;
                        break;
                    }
                    else if (escape)
                    {
                        switch (chars[index])
                        {
                            case 't':
                                sb.Append("\t");
                                break;
                            case 'n':
                                sb.AppendLine();
                                break;
                            case '\\':
                            case '"':
                                sb.Append(chars[index]);
                                break;
                            default:
                                engine.SetPointer(index);
                                engine.AddError(new YAMPEscapeSequenceNotFoundError(engine, chars[index]), exp);
                                break;
                        }

                        escape = false;
                    }
                    else
                        sb.Append(chars[index]);

                    index++;
                }

                if (!terminated)
                    engine.AddError(new YAMPStringNotTerminatedError(engine), exp);

                exp.value = sb.ToString();
                exp.Length = index - start;
                engine.SetPointer(index);
                return exp;
            }

            return null;
        }

        #endregion

        #region String Representations

        public override string ToCode()
        {
            if(IsLiteral)
                return "@\"" + value + '"';

            return '"' + 
                     value.Replace("\t", "\\t")
                          .Replace("\n", "\\n")
                          .Replace("\\", "\\\\")
                          .Replace("\"", "\\\"")
                    + '"';
        }

        #endregion
    }
}

