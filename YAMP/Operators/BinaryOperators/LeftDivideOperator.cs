using System;

namespace YAMP
{
	class LeftDivideOperator : BinaryOperator
	{
		public LeftDivideOperator () : base(@"\", 20)
		{
		}

        public override Operator Create()
        {
            return new LeftDivideOperator();
        }
		
		public override Value Perform (Value left, Value right)
		{
			return right.Divide(left);
		}
	}
}

