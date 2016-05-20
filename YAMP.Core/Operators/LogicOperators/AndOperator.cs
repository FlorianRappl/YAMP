namespace YAMP
{
    using System;

    /// <summary>
    /// The basic and &amp;&amp; operator.
    /// </summary>
    class AndOperator : LogicOperator
    {
        public AndOperator()
            : base("&&", 3)
		{
		}

		public override ScalarValue Compare (ScalarValue left, ScalarValue right)
		{
            return new ScalarValue(left.IsTrue && right.IsTrue);
		}

        public override Operator Create()
        {
            return new AndOperator();
        }
    }
}
