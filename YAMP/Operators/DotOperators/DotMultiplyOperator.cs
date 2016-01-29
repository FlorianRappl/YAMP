using System;

namespace YAMP
{
    /// <summary>
    /// The .* operator.
    /// </summary>
    class DotMultiplyOperator : DotOperator
    {
        public DotMultiplyOperator() : base(new MultiplyOperator())
        {
        }

        public override ScalarValue Operation(ScalarValue left, ScalarValue right)
        {
            return left * right;
        }

        public override Operator Create()
        {
            return new DotMultiplyOperator();
        }
    }
}

