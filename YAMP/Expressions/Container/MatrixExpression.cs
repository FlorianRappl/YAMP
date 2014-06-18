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

namespace YAMP
{
    /// <summary>
    /// The matrix [ ... ] expression.
    /// </summary>
    class MatrixExpression : TreeExpression
    {
        #region ctor

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public MatrixExpression()
		{
		}

        /// <summary>
        /// Creates a new instance with some parameters.
        /// </summary>
        /// <param name="line">The line where the matrix expression starts.</param>
        /// <param name="column">The column in the line where the matrix exp. starts.</param>
        /// <param name="length">The length of the matrix expression.</param>
        /// <param name="query">The associated query context.</param>
        /// <param name="child">The child containing the column and rows.</param>
        public MatrixExpression(int line, int column, int length, QueryContext query, ContainerExpression child)
            : base(child, query, line, column)
		{
            Length = length;
		}

        #endregion

        #region Methods

        /// <summary>
        /// Begins interpreting the matrix expression.
        /// </summary>
        /// <param name="symbols">External symbols to load.</param>
        /// <returns>The evaluated matrix value.</returns>
        public override Value Interpret(Dictionary<string, Value> symbols)
        {
            return base.Interpret(symbols) ?? new MatrixValue();
        }

        /// <summary>
        /// Scans the current parse engine for a matrix expression.
        /// </summary>
        /// <param name="engine">The parse engine to use.</param>
        /// <returns>The found expression or NULL.</returns>
        public override Expression Scan(ParseEngine engine)
        {
            var column = engine.CurrentColumn;
            var line = engine.CurrentLine;
            var chars = engine.Characters;
            var start = engine.Pointer;

            if (chars[start] == '[')
            {
                engine.Advance();
                var terminated = false;
                var statement = new Statement();
                bool ws = false, nl = false;

                while (engine.Pointer < chars.Length && engine.IsParsing)
                {
                    if (ParseEngine.IsWhiteSpace(chars[engine.Pointer]))
                    {
                        ws = true;
                        engine.Advance();
                    }
                    else if (ParseEngine.IsNewLine(chars[engine.Pointer]))
                    {
                        nl = true;
                        engine.Advance();
                    }
                    else if (chars[engine.Pointer] == ']')
                    {
                        terminated = true;
                        engine.Advance();
                        break;
                    }
                    else if (chars[engine.Pointer] == ',')
                    {
                        ws = false;
                        nl = false;
                        statement.Push(engine, new ColumnOperator(engine));
                        engine.Advance();
                    }
                    else if (chars[engine.Pointer] == ';')
                    {
                        ws = false;
                        nl = false;
                        statement.Push(engine, new RowOperator(engine));
                        engine.Advance();
                    }
                    else if (engine.Pointer < chars.Length - 1 && ParseEngine.IsComment(chars[engine.Pointer], chars[engine.Pointer + 1]))
                    {
                        if (ParseEngine.IsLineComment(chars[engine.Pointer], chars[engine.Pointer + 1]))
                            engine.AdvanceToNextLine();
                        else
                            engine.AdvanceTo("*/");
                    }
                    else
                    {
                        engine.ParseBlock(statement, nl ? (Operator)new RowOperator(engine) : (ws ? new ColumnOperator(engine) : null));
                        ws = false;
                        nl = false;
                    }
                }

                if (!terminated)
                {
                    var err = new YAMPMatrixNotClosedError(line, column);
                    engine.AddError(err);
                }

                var container = statement.Finalize(engine).Container;
                return new MatrixExpression(line, column, engine.Pointer - start, engine.Query, container ?? new ContainerExpression());
            }

            return null;
        }

        #endregion

        #region String Representations

        /// <summary>
        /// Transforms the expression into YAMP query code.
        /// </summary>
        /// <returns>The string representation of the part of the query.</returns>
        public override string ToCode()
        {
            return "[" + base.ToCode() + "]";
        }

        #endregion
    }
}
