using System;

namespace YAMP
{
	class MultiplyOperator : BinaryOperator
	{
		public MultiplyOperator () : base("*", 10)
		{
		}

        public override Operator Create()
        {
            return new MultiplyOperator();
        }
		
		public override Value Perform (Value left, Value right)
		{
			return left.Multiply(right);
		}
	}
}

