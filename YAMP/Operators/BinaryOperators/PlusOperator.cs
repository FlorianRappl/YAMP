using System;

namespace YAMP
{
	class PlusOperator : BinaryOperator
	{
		public PlusOperator () : base("+", 5)
		{
		}
		
		public override Value Perform (Value left, Value right)
		{
			return left.Add(right);
		}
	}
}

