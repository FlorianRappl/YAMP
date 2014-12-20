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
    /// The class representing the do keyword for do {...} while(...); loops. Basic syntax:
    /// do STATEMENT while ( CONDITION) ;
    /// </summary>
    class DoKeyword : BreakableKeyword
    {
        #region Members

        bool __break;

        #endregion

        #region ctor

        public DoKeyword()
            : base("do")
        {
        }

        public DoKeyword(int line, int column, QueryContext query)
            : this()
        {
            StartLine = line;
            StartColumn = column;
            Query = query;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the associated condition in form of the while keyword.
        /// </summary>
        public WhileKeyword While { get; internal set; }

        #endregion

        #region Methods

        public override Value Interpret(Dictionary<string, Value> symbols)
        {
            __break = false;

            do
            {
                Body.Interpret(symbols);

                if (__break)
                    break;
            }
            while (While.InterpretCondition(symbols));

            return null;
        }

        public override void Break()
        {
            __break = true;
        }

        public override Expression Scan(ParseEngine engine)
        {
            var start = engine.Pointer;
            var kw = new DoKeyword(engine.CurrentLine, engine.CurrentColumn, engine.Query);
            var index = engine.Advance(Token.Length).Pointer;
            kw.Body = engine.ParseStatement();
            kw.Length = engine.Pointer - start;
            return kw;
        }

        #endregion

        #region String Representations

        public override string ToCode()
        {
            var sb = new StringBuilder();
            sb.AppendLine(Token);
            sb.AppendLine(Body.ToCode());
            return sb.ToString();
        }

        #endregion
    }
}
