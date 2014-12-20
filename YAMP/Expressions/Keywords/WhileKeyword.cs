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
    /// The while keyword has its logic defined here. The basic syntax:
    /// while ( CONDITION ) STATEMENT
    /// </summary>
    class WhileKeyword : BreakableKeyword
    {
        #region Members

        bool __break;

        #endregion

        #region ctor

        public WhileKeyword() : base("while")
        {
        }

        public WhileKeyword(int line, int column, QueryContext query)
            : this()
		{
			Query = query;
            StartLine = line;
            StartColumn = column;
		}

        #endregion

        #region Properties

        /// <summary>
        /// Gets the condition statement for each iteration.
        /// </summary>
        public Statement Condition { get; private set; }

        /// <summary>
        /// Gets the status of the while-loop. Is it a do-while loop?
        /// </summary>
        public bool IsDoWhile { get; private set; }

        #endregion

        #region Methods

        public override Expression Scan(ParseEngine engine)
        {
            var start = engine.Pointer;
            var kw = new WhileKeyword(engine.CurrentLine, engine.CurrentColumn, engine.Query);
            var index = engine.Advance(Token.Length).Skip().Pointer;
            var chars = engine.Characters;

            if (index == chars.Length)
            {
                kw.Length = engine.Pointer - start;
                engine.AddError(new YAMPWhileArgumentsMissing(engine), kw);
                return kw;
            }

            if (chars[index] == '(')
            {
                var ln = engine.CurrentLine;
                var col = engine.CurrentColumn;
                kw.Condition = engine.Advance().ParseStatement(')', e => new YAMPBracketNotClosedError(ln, col));

                if (kw.Condition.Container == null || !kw.Condition.Container.HasContent)
                    engine.AddError(new YAMPWhileArgumentsMissing(engine), kw);

                SetMarker(engine);
                kw.Body = engine.ParseStatement();
                UnsetMarker(engine);

                if (engine.LastStatement != null && engine.LastStatement.IsKeyword<DoKeyword>())
                {
                    if (kw.Body.IsEmpty)
                    {
                        IsDoWhile = true;
                        engine.LastStatement.GetKeyword<DoKeyword>().While = kw;
                    }
                    else
                        engine.AddError(new YAMPDoWhileNotEmptyError(engine), kw);
                }
            }
            else
                engine.AddError(new YAMPWhileArgumentsMissing(engine), kw);

            kw.Length = engine.Pointer - start;
            return kw;
        }

        public override void Break()
        {
            __break = true;
        }

        public override Value Interpret(Dictionary<string, Value> symbols)
        {
            if (IsDoWhile)
                return null;

            __break = false;

            while (InterpretCondition(symbols))
            {
                Body.Interpret(symbols);

                if (__break)
                    break;
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
            sb.Append(Condition.Container.ToCode());
            sb.AppendLine(")").AppendLine(Body.ToCode());
            return sb.ToString();
        }

        #endregion
    }
}