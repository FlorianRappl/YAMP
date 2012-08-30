using System;

namespace YAMP
{
	class CommaOperator : Operator
	{
		public CommaOperator () : base(",", 2)
		{
		}
		
		public override Value Perform (Value left, Value right)
		{
			return new VectorValue(left, right);
		}
	}
}

