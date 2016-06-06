namespace YAMP
{
    using System;

    /// <summary>
    /// The basic and &amp;&amp; operator.
    /// </summary>
    class AndOperator : LogicOperator
    {
        #region Mapping

        public static readonly String Symbol = OpDefinitions.AndOperator;
        public static readonly int OpLevel = OpDefinitions.AndOperatorLevel;
        public static readonly BinaryOperatorMappingList Mapping = new BinaryOperatorMappingList(Symbol);

        #endregion

        public AndOperator()
            : base(Symbol, OpLevel)
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
