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
    /// This is the class that represents some special expressions (like :).
    /// </summary>
	class SpecialExpression : Expression
    {
        #region Members

        string specialName;
        Func<Dictionary<string, Value>, Value> specialValue;
        static readonly Dictionary<string, Func<Dictionary<string, Value>, Value>> specialExpressions;

        #endregion

        #region ctor

        public SpecialExpression ()
		{
		}

		public SpecialExpression(ParseEngine engine, string name) : base(engine)
		{
            specialName = name;
            Length = name.Length;
		}

        static SpecialExpression()
        {
            specialExpressions = new Dictionary<string, Func<Dictionary<string, Value>, Value>>();
            specialExpressions.Add(":", symbols => new RangeValue());
        }

        #endregion

        #region Methods

        public override Value Interpret(Dictionary<string, Value> symbols)
		{
            return specialValue(symbols);
		}

        public override Expression Scan(ParseEngine engine)
        {
            foreach(var specialExpression in specialExpressions)
            {
                if (Compare(engine.Characters, engine.Pointer, specialExpression.Key))
                {
                    var exp = new SpecialExpression(engine, specialExpression.Key);
                    engine.Advance();
                    exp.specialValue = specialExpression.Value;
                    return exp;
                }
            }

            return null;
        }

        #endregion

        #region String Representations

        public override string ToCode()
        {
            return specialName;
        }

        #endregion

        #region Helpers

        static bool Compare(char[] characters, int index, string value)
        {
            if (characters.Length - index < value.Length)
                return false;

            for (var i = 0; i < value.Length; i++)
            {
                if (value[i] != characters[i + index])
                    return false;
            }

            return true;
        }

        #endregion
    }
}

