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
using System.Text.RegularExpressions;

namespace YAMP
{
    /// <summary>
    /// The absolute expression |...| which returns the absolute value
    /// of the evaluated value inside.
    /// </summary>
    class AbsExpression : TreeExpression
    {
        #region ctor

        public AbsExpression()
        {
        }

        public AbsExpression(int line, int column, int length, QueryContext query, ContainerExpression child)
            : base(child, query, line, column)
        {
            Length = length;
        }

        #endregion

        #region Methods

        public override Value Interpret(Dictionary<string, Value> symbols)
        {
            return AbsFunction.Abs(base.Interpret(symbols));
        }

        public override Expression Scan(ParseEngine engine)
        {
            var chars = engine.Characters;
            var start = engine.Pointer;

            if (chars[start] == '|')
            {
                var line = engine.CurrentLine;
                var col = engine.CurrentColumn;
                var container = engine.Advance().ParseStatement('|', e => new YAMPTerminatorMissingError(line, col, '|')).Container;
                return new AbsExpression(line, col, engine.Pointer - start, engine.Query, container);
            }

            return null;
        }

        #endregion

        #region String Representations

        public override string ToCode()
        {
            return "|" + base.ToCode() + "|";
        }

        #endregion
    }
}