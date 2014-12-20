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
    /// This is the class for the range operator : - this one is also
    /// available as a stand-alone expression.
    /// </summary>
	class RangeOperator : BinaryOperator
	{
		const string END = "end";

		public RangeOperator () : base(":", 3)
		{
			IsRightToLeft = true;
		}
		
		public override Value Perform (Value left, Value right)
		{
			var step = 1.0;
			var explicitStep = false;
			var start = 0.0;
			var end = 0.0;
			var all = false;
			
			if(left is ScalarValue)
				start = (left as ScalarValue).Re;
			else if (left is RangeValue)
			{
				var m = left as RangeValue;
				start = m.Start;
				step = m.End;
				explicitStep = true;
			}
			else
				throw new YAMPOperationInvalidException(":", left);

			if(right is ScalarValue)
				end = (right as ScalarValue).Re;
			else if (right is RangeValue)
			{
				var m = right as RangeValue;
				step = m.Start;
				end = m.End;
				all = m.All;
				explicitStep = true;
			}
			else if(right is StringValue)
				all = (right as StringValue).Value.Equals(END);
			else
				throw new YAMPOperationInvalidException(":", left);

			if(!all && !explicitStep && end < start)
				step = -1.0;

			if(all)
				return new RangeValue(start, step);

			return new RangeValue(start, end, step);
		}

		public override Value Handle(Expression left, Expression right, Dictionary<string, Value> symbols)
		{
			var l = left.Interpret(symbols);
			var r = new StringValue(END) as Value;

			if(!(right is SymbolExpression) || !((right as SymbolExpression).SymbolName.Equals(END)))
				r = right.Interpret(symbols);

			return Perform(l, r);
		}

        public override Operator Create()
        {
            return new RangeOperator();
        }
	}
}

