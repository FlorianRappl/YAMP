using System;

namespace YAMP
{
    /// <summary>
    /// Just returns the given value.
    /// </summary>
    class PosateOperator : LeftUnaryOperator
    {
        public static readonly String Symbol = OpDefinitions.PosateOperator;
        public static readonly int OpLevel = OpDefinitions.PosateOperatorLevel;

        public PosateOperator()
            : base(Symbol, OpLevel)
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
