namespace YAMP
{
    using System;

    /// <summary>
    /// The basic greater equals operator.
    /// </summary>
    class GtEqOperator : LogicOperator
    {
		public GtEqOperator ()
            : base(">=")
		{
		}

		public override ScalarValue Compare (ScalarValue left, ScalarValue right)
		{
            return new ScalarValue(left >= right);
		}

        public override Operator Create()
        {
            return new GtEqOperator();
        }
    }
}
