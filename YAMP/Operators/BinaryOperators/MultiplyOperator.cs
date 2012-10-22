using System;

namespace YAMP
{
	class MultiplyOperator : BinaryOperator
	{
		public MultiplyOperator () : base("*", 10)
		{
		}
		
		public override Value Perform (Value left, Value right)
		{
			return left.Multiply(right);
		}
	}
}

