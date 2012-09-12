using System;

namespace YAMP
{
	class DotDivideOperator : DotOperator
	{
		public DotDivideOperator () : base(new RightDivideOperator())
		{
		}

        public override Operator Create()
        {
            return new DotDivideOperator();
        }

		public override ScalarValue Operation (ScalarValue left, ScalarValue right)
		{
			return left.Divide(right) as ScalarValue;
		}
	}
}

