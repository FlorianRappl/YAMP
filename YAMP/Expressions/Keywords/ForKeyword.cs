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
    /// The for keyword with the corresponding loop construct. Basic syntax:
    /// for ( INIT ; CONDITION ; END ) STATEMENT
    /// </summary>
    class ForKeyword : BreakableKeyword
    {
        #region Members

        bool __break;

        #endregion

        #region ctor

        public ForKeyword()
            : base("for")
        {
        }

		public ForKeyword(int line, int column, QueryContext query)
            : this()
		{
			Query = query;
            StartLine = line;
            StartColumn = column;
		}

        #endregion

        #region Properties

        /// <summary>
        /// Gets the initialization statement before the iterations.
        /// </summary>
        public Statement Initialization { get; private set; }

        /// <summary>
        /// Gets the condition statement for each iteration.
        /// </summary>
        public Statement Condition { get; private set; }

        /// <summary>
        /// Gets the ending statement of each iteration.
        /// </summary>
        public Statement End { get; private set; }

        #endregion

        #region Methods

        public override Expression Scan(ParseEngine engine)
        {
            var kw = new ForKeyword(engine.CurrentLine, engine.CurrentColumn, engine.Query);
            var start = engine.Pointer;
            var index = engine.Advance(Token.Length).Skip().Pointer;
            var chars = engine.Characters;

            if (index == chars.Length)
            {
                kw.Length = engine.Pointer - start;
                engine.AddError(new YAMPForArgumentsMissing(engine));
                return kw;
            }
            
            if (chars[index] == '(')
            {
                var ln = engine.CurrentLine;
                var col = engine.CurrentColumn;
                kw.Initialization = engine.Advance().ParseStatement();
                kw.Condition = engine.ParseStatement();
                kw.Condition.IsMuted = false;
                kw.End = engine.ParseStatement(')', e => new YAMPBracketNotClosedError(ln, col));
                SetMarker(engine);
                kw.Body = engine.ParseStatement();
                UnsetMarker(engine);
            }
            else
                engine.AddError(new YAMPForArgumentsMissing(engine));

            kw.Length = engine.Pointer - start;
            return kw;
        }

        public override void Break()
        {
            __break = true;
        }

        public override Value Interpret(Dictionary<string, Value> symbols)
        {
            Initialization.Interpret(symbols);
            __break = false;

            while (InterpretCondition(symbols))
            {
                Body.Interpret(symbols);

                if (__break)
                    break;

                End.Interpret(symbols);
            }

            return null;
        }

        public bool InterpretCondition(Dictionary<string, Value> symbols)
        {
            var condition = Condition.Interpret(symbols);

            if (condition != null && condition is ScalarValue)
                return ((ScalarValue)condition).IsTrue;

            return false;
        }

        #endregion

        #region String Representations

        public override string ToCode()
        {
            var sb = new StringBuilder();
            sb.Append(Token).Append("(");
            sb.Append(Initialization.ToCode()).Append(Condition.ToCode()).Append(End.Container.ToCode());
            sb.AppendLine(")");
            sb.AppendLine(Body.ToCode());
            return sb.ToString();
        }

        #endregion
    }
}