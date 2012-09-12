using System;

namespace YAMP
{
	class PlusOperator : BinaryOperator
	{
		public PlusOperator () : base("+", 4)
		{
		}

        public override Operator Create()
        {
            return new PlusOperator();
        }
		
		public override Value Perform (Value left, Value right)
		{
			return left.Add(right);
		}
	}
}

