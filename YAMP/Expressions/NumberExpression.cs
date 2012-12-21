/*
	Copyright (c) 2012, Florian Rappl.
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
using System.Globalization;
using System.Text.RegularExpressions;

namespace YAMP
{
	class NumberExpression : Expression
	{		
		const NumberStyles style = NumberStyles.Float;
		
		public NumberExpression () : base(@"[-+]?[0-9]*\.?[0-9]+([eE][-+]?[0-9]+)?i?")
		{
		}

		public NumberExpression (QueryContext query, Match match) : this()
		{
			Query = query;
			mx = match;
		}

		public override Expression Create(QueryContext query, Match match)
		{
			return new NumberExpression(query, match);
		}

		public override Value Interpret(Dictionary<string, Value> symbols)
		{
			var real = 0.0;
			var imag = 0.0;
			
			if(_input[_input.Length - 1] == 'i')
				imag = double.Parse(_input.Remove(_input.Length - 1), style, Context.NumberFormat);
			else
				real = double.Parse(_input, style, Context.NumberFormat);
			
			return new ScalarValue(real, imag);
		}
	}
}

