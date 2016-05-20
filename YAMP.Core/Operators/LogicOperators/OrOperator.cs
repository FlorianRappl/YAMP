namespace YAMP
{
    using System;

    /// <summary>
    /// The basic or || operator.
    /// </summary>
    class OrOperator : LogicOperator
    {
        public OrOperator()
            : base("||", 3)
		{
		}

		public override ScalarValue Compare (ScalarValue left, ScalarValue right)
		{
            return new ScalarValue(left.IsTrue || right.IsTrue);
		}

        public override Operator Create()
        {
            return new OrOperator();
        }
    }
}
