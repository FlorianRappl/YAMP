using System;

namespace YAMP
{
    /// <summary>
    /// The .^ operator.
    /// </summary>
    class DotPowerOperator : DotOperator
    {
        public DotPowerOperator() : base(new PowerOperator())
        {
        }

        public override ScalarValue Operation(ScalarValue left, ScalarValue right)
        {
            return left.Pow(right);
        }

        public override Operator Create()
        {
            return new DotPowerOperator();
        }
    }
}

