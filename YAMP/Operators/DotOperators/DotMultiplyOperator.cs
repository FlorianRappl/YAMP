using System;

namespace YAMP
{
    class DotMultiplyOperator : DotOperator
    {
        public DotMultiplyOperator() : base(new MultiplyOperator())
        {
        }

        public override Operator Create()
        {
            return new DotMultiplyOperator();
        }

        public override ScalarValue Operation(ScalarValue left, ScalarValue right)
        {
            return left.Multiply(right) as ScalarValue;
        }
    }
}

