using System;

namespace YAMP
{
	class PowerOperator : BinaryOperator
	{
		public PowerOperator () : base("^", 100)
		{
			IsRightToLeft = true;
		}
		
		public override Value Perform (Value left, Value right)
		{
			return left.Power(right);
		}
	}
}

