using System;

namespace YAMP
{
    class DotMultiplyOperator : DotOperator
    {
        public DotMultiplyOperator() : base(new MultiplyOperator())
        {
        }

        public override ScalarValue Operation(ScalarValue left, ScalarValue right)
        {
            return left.Multiply(right) as ScalarValue;
        }
    }
}

