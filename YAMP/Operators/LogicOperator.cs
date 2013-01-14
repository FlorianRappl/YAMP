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

namespace YAMP
{
    /// <summary>
    /// The abstract base class for any logic operator (==, ~=, >, >=, ...),
    /// which is essentially a binary operator.
    /// </summary>
    public abstract class LogicOperator : BinaryOperator
	{
		public LogicOperator (string op) : base(op, 4)
		{
		}

		public abstract ScalarValue Compare(ScalarValue left, ScalarValue right);
		
		public override Value Perform (Value left, Value right)
		{
			if(!(left is ScalarValue || left is MatrixValue))
				throw new YAMPOperationInvalidException(Op, left);

			if(!(right is ScalarValue || right is MatrixValue))
				throw new YAMPOperationInvalidException(Op, right);
			
			if(left is ScalarValue && right is ScalarValue)
			{
				return Compare (left as ScalarValue, right as ScalarValue);
			}
			else if(left is MatrixValue && right is ScalarValue)
			{
				var l = left as MatrixValue;
				var r = right as ScalarValue;
				var m = new MatrixValue(l.DimensionY, l.DimensionX);
				
				for(var i = 1; i <= m.DimensionX; i++)
					for(var j = 1; j <= m.DimensionY; j++)
						m[j, i] = Compare(l[j, i], r);
				
				return m;
			}
			else if(left is ScalarValue && right is MatrixValue)
			{
				var l = left as ScalarValue;
				var r = right as MatrixValue;
				var m = new MatrixValue(r.DimensionY, r.DimensionX);
				
				for(var i = 1; i <= m.DimensionX; i++)
					for(var j = 1; j <= m.DimensionY; j++)
						m[j, i] = Compare(l, r[j, i]);
				
				return m;
			}
			else
			{
				var l = left as MatrixValue;
				var r = right as MatrixValue;

				if(l.DimensionX != r.DimensionX || l.DimensionY != r.DimensionY)
					throw new YAMPDifferentDimensionsException(l, r);

				var m = new MatrixValue(l.DimensionY, l.DimensionX);
				
				for(var i = 1; i <= m.DimensionX; i++)
					for(var j = 1; j <= m.DimensionY; j++)
						m[j, i] = Compare(l[j, i], r[j, i]);
				
				return m;
			}
		}
	}
}

