using System;

namespace YAMP
{
    class DotLeftDivideOperator : DotOperator
    {
        public DotLeftDivideOperator() : base(new LeftDivideOperator())
        {
        }

        public override Operator Create()
        {
            return new DotLeftDivideOperator();
        }

        public override ScalarValue Operation(ScalarValue left, ScalarValue right)
        {
            return right.Divide(left) as ScalarValue;
        }
    }
}

