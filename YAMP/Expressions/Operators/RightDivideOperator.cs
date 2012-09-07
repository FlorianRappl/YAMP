using System;

namespace YAMP
{
	class RightDivideOperator : BinaryOperator
	{
		public RightDivideOperator () : base("/", 20)
		{
		}
		
		public override Value Perform (Value left, Value right)
		{
			return left.Divide(right);
		}
	}
}

