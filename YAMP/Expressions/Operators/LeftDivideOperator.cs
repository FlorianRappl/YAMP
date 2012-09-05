using System;

namespace YAMP
{
	class LeftDivideOperator : Operator
	{
		public LeftDivideOperator () : base(@"\", 20)
		{
		}
		
		public override Value Perform (Value left, Value right)
		{
			return right.Divide(left);
		}
	}
}

