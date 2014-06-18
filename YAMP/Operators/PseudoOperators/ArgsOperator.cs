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
using System.Collections;
using System.Collections.Generic;

namespace YAMP
{
    /// <summary>
    /// Operator for arguments () for symbols (usually functions!).
    /// </summary>
    class ArgsOperator : RightUnaryOperator
    {
        #region Members

        Expression _content;

		#endregion

		#region ctor

		public ArgsOperator() : base("(", 1000)
        {
        }

		#endregion

		#region Creation

		public override Operator Create()
        {
            return new ArgsOperator();
        }

        public override Operator Create(ParseEngine engine)
        {
            var start = engine.Pointer;

            //Arguments need to be attached directly.
            if (start == 0 || ParseEngine.IsWhiteSpace(engine.Characters[start - 1]) || ParseEngine.IsNewLine(engine.Characters[start - 1]))
                return null;

            var ao = new ArgsOperator();
            ao.Query = engine.Query;
            ao.StartLine = engine.CurrentLine;
            ao.StartColumn = engine.CurrentColumn;
            ao._content = Elements.Instance.FindExpression<BracketExpression>().Scan(engine);
            ao.Length = engine.Pointer - start;
            return ao;
        }

        #endregion

        #region Methods

        public override Value Perform(Value left)
        {
            return left;
        }

        public override Value Handle(Expression expression, Dictionary<string, Value> symbols)
        {
            var args = _content.Interpret(symbols);
            var left = expression.Interpret(symbols);

            if(left is IFunction)
                return ((IFunction)left).Perform(Context, args);

            if(expression is SymbolExpression)
                throw new YAMPFunctionMissingException(((SymbolExpression)expression).SymbolName);

            throw new YAMPExpressionNoFunctionException();
        }

        public Value Handle(Expression expression, Value value, Dictionary<string, Value> symbols)
        {
            var symbolName = string.Empty;
            var isSymbol = expression is SymbolExpression;
            var args = _content.Interpret(symbols);

            if (isSymbol)
            {
                var sym = (SymbolExpression)expression;
                symbolName = sym.SymbolName;

                if (Context.GetVariableContext(sym.SymbolName) == null)
                    Context.AssignVariable(sym.SymbolName, new MatrixValue());
            }

            var left = expression.Interpret(symbols);

            if (left is ISetFunction)
            {
                var sf = (ISetFunction)left;
                sf.Perform(Context, args, value);

                if (isSymbol)
                    Context.AssignVariable(symbolName, left);

                return left;
            }

            if (expression is SymbolExpression)
                throw new YAMPFunctionMissingException(((SymbolExpression)expression).SymbolName);

            throw new YAMPExpressionNoFunctionException();
        }

        #endregion

        #region String Representations

        public override string ToString()
        {
            return _content.ToString();
        }

        public override string ToCode()
        {
            return "(" + _content.ToCode() + ")";
        }

		#endregion
    }
}