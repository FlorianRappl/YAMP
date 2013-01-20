/*
	Copyright (c) 2012-2013, Florian Rappl.
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
    /// Gets the class for the unary (- or + or negation ~ in front) expression.
    /// </summary>
    class UnaryExpression : Expression
    {
        #region Members

        static IDictionary<string, LeftUnaryOperator> operators = new Dictionary<string, LeftUnaryOperator>();

        #endregion

        #region ctors

        public UnaryExpression()
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds an operator to the dictionary.
        /// </summary>
        /// <param name="pattern">The operator pattern, i.e. += for add and assign.</param>
        /// <param name="op">The instance of the operator.</param>
        public static void AddOperator(string pattern, LeftUnaryOperator op)
        {
            operators.Add(pattern, op);
        }

        public override Value Interpret(Dictionary<string, Value> symbols)
        {
            return null;
        }

        public override Expression Scan(ParseEngine engine)
        {
            var op = FindUnaryOperator(engine);

            if (op != null)
            {
                engine.CurrentStatement.Push(new EmptyExpression());
                engine.CurrentStatement.Push(op);

                if(engine.Pointer < engine.Characters.Length)
                    return Elements.Instance.FindExpression(engine);

                engine.AddError(new YAMPExpressionExpectedError(engine));
                return new EmptyExpression();
            }

            return null;
        }

        /// <summary>
        /// Finds the closest matching (left) unary operator.
        /// </summary>
        /// <param name="engine">The engine to parse the query.</param>
        /// <returns>Unary operator that matches the current characters.</returns>
        public Operator FindUnaryOperator(ParseEngine engine)
        {
            var maxop = string.Empty;
            var notfound = true;
            var chars = engine.Characters;
            var ptr = engine.Pointer;
            var rest = chars.Length - ptr;

            foreach (var op in operators.Keys)
            {
                if (op.Length > rest)
                    continue;

                if (op.Length <= maxop.Length)
                    continue;

                notfound = false;

                for (var i = 0; i < op.Length; i++)
                    if (notfound = (chars[ptr + i] != op[i]))
                        break;

                if (notfound == false)
                    maxop = op;
            }

            if (maxop.Length == 0)
                return null;

            return operators[maxop].Create(engine);
        }

        #endregion

        #region String Representations

        public override string ToCode()
        {
            return string.Empty;
        }

        #endregion
    }
}
