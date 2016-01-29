using System;

namespace YAMP
{
    /// <summary>
    /// Just returns the given value.
    /// </summary>
    class PosateOperator : LeftUnaryOperator
    {
        public PosateOperator()
            : base("+", 7)
        {
        }

        public override Value Perform(Value value)
        {
            return value;
        }

        public override Operator Create()
        {
            return new PosateOperator();
        }
    }
}
