using System;

namespace YAMP
{
	class PowerOperator : Operator
	{
		public PowerOperator () : base("^", 100)
		{
		}
		
		public override Value Perform (Value left, Value right)
		{
			return left.Power(right);
		}
	}
}

