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
using System.Collections.Generic;
using System.Text;

namespace YAMP
{
    /// <summary>
    /// The else keyword - can (and should) only be instantiated by the if keyword.
    /// Basic syntax: else STATEMENT
    /// Can only be used after an IF or ELSE IF
    /// </summary>
    class ElseKeyword : BodyKeyword
    {
        #region ctor

        public ElseKeyword()
            : base("else")
        {

        }

        public ElseKeyword(int line, int column, QueryContext query)
            : this()
		{
			Query = query;
            StartLine = line;
            StartColumn = column;
		}

        #endregion

        #region Properties

        /// <summary>
        /// Gets a boolean indicating if the body is another if-statement.
        /// </summary>
        public bool IsElseIf
        {
            get { return Body.IsKeyword<IfKeyword>(); }
        }

        /// <summary>
        /// Gets the if-keyword contained in the body.
        /// </summary>
        public IfKeyword ElseIf
        {
            get { return Body.GetKeyword<IfKeyword>(); }
        }

        #endregion

        #region Methods

        public override Expression Scan(ParseEngine engine)
        {
            var start = engine.Pointer;
            var kw = new ElseKeyword(engine.CurrentLine, engine.CurrentColumn, engine.Query);
            kw.Body = engine.Advance(Token.Length).ParseStatement();

            if (engine.LastStatement == null)
                engine.AddError(new YAMPIfRequiredError(engine), kw);
            else if (engine.LastStatement.IsKeyword<IfKeyword>())
                engine.LastStatement.GetKeyword<IfKeyword>().Else = kw;
            else if(engine.LastStatement.IsKeyword<ElseKeyword>())
            {
                var otherwise = engine.LastStatement.GetKeyword<ElseKeyword>();

                if (otherwise.IsElseIf)
                    otherwise.ElseIf.Else = kw;
                else
                    engine.AddError(new YAMPSingleElseError(engine), kw);
            }
            else
                engine.AddError(new YAMPIfRequiredError(engine), kw);

            kw.Length = engine.Pointer - start;
            return kw;
        }

        public override Value Interpret(Dictionary<string, Value> symbols)
        {
            return null;
        }

        #endregion

        #region String Representations

        public override string ToCode()
        {
            var sb = new StringBuilder();
            sb.AppendLine(Token).Append(Body.ToCode());
            return sb.ToString();
        }

        #endregion
    }
}
