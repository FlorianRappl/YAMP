using System;

namespace YAMP
{
	class DivideOperator : Operator
	{
		public DivideOperator () : base("/", 20)
		{
		}
		
		public override Value Perform (Value left, Value right)
		{
			return left.Divide(right);
		}
	}
}

