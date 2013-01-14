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

        ScalarValue factor;

        #endregion

        #region ctors

        public UnaryExpression()
        {
        }

        public UnaryExpression(ParseEngine engine) : base(engine)
        {
            Length = 1;
        }

        #endregion

        #region Methods

        public override Value Interpret(Dictionary<string, Value> symbols)
        {
            return factor;
        }

        public override Expression Scan(ParseEngine engine)
        {
            var index = engine.Pointer;
            var chars = engine.Characters;

            if (chars[index] == '-' || chars[index] == '+')
            {
                var exp = new UnaryExpression(engine);
                exp.factor = new ScalarValue(chars[index] == '+' ? 1.0 : -1.0);
                engine.Replace(index, '*');
                return exp;
            }
            else if (chars[index] == '~')
            {
                var exp = new UnaryExpression(engine);
                exp.factor = new ScalarValue();
                engine.Replace(index, '~');
                return exp;
            }

            return null;
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
