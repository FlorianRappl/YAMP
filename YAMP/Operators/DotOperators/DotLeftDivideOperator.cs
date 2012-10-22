using System;

namespace YAMP
{
    class DotLeftDivideOperator : DotOperator
    {
        public DotLeftDivideOperator() : base(new LeftDivideOperator())
        {
        }

        public override ScalarValue Operation(ScalarValue left, ScalarValue right)
        {
            return right.Divide(left) as ScalarValue;
        }
    }
}

