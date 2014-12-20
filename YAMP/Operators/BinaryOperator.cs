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
using System.Collections;
using System.Collections.Generic;

namespace YAMP
{
    /// <summary>
    /// The abstract base class for any binary operator (+, -, *, ...).
    /// </summary>
	public abstract class BinaryOperator : Operator
    {
        #region ctor

        /// <summary>
        /// Creates a new binary operator.
        /// </summary>
        /// <param name="op">The operator string.</param>
        /// <param name="level">The operator level.</param>
        public BinaryOperator(string op, int level) : base(op, level)
		{
            Expressions = 2;
		}

        #endregion

        #region Methods

        /// <summary>
        /// Performs the operation with the 2 evaluated values.
        /// </summary>
        /// <param name="left">The left value.</param>
        /// <param name="right">The right value.</param>
        /// <returns>The result of the operation.</returns>
        public abstract Value Perform(Value left, Value right);

        /// <summary>
        /// Handles the evaluation of two expressions.
        /// </summary>
        /// <param name="left">The expression on the left.</param>
        /// <param name="right">The expression on the right.</param>
        /// <param name="symbols">The external symbols to consider.</param>
        /// <returns>The result of the operation.</returns>
		public virtual Value Handle(Expression left, Expression right, Dictionary<string, Value> symbols)
		{
			var l = left.Interpret(symbols);
			var r = right.Interpret(symbols);
			return Perform(l, r);
		}

        internal Value PerformOverFind(Value left, Value right, List<BinaryOperatorMapping> mapping)
        {
            Func<Value, Value, Value> least = null;

            for (var i = 0; i != mapping.Count; i++)
            {
                var hit = mapping[i].IsMapping(left, right);

                if (hit == MapHit.Direct)
                    return mapping[i].Map(left, right);
                else if (hit == MapHit.Indirect)
                    least = mapping[i].Map;
            }

            if(least != null)
                return least(left, right);

            throw new YAMPOperationInvalidException(Op, left, right);
        }

        /// <summary>
        /// The implementation of the more general evaluate method.
        /// </summary>
        /// <param name="expressions">The array of expressions, binary operators require Length == 2.</param>
        /// <param name="symbols">The external symbols to consider.</param>
        /// <returns>The result of the operation.</returns>
		public override Value Evaluate(Expression[] expressions, Dictionary<string, Value> symbols)
		{
            if (expressions.Length != 2)
                throw new YAMPArgumentNumberException(Op, expressions.Length, 2);

			return Handle(expressions[0], expressions[1], symbols);
        }

        #endregion
    }
}