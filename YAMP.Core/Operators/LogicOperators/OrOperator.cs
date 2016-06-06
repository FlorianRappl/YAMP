namespace YAMP
{
    using System;

    /// <summary>
    /// The basic or || operator.
    /// </summary>
    class OrOperator : LogicOperator
    {
        #region Mapping

        public static readonly String Symbol = OpDefinitions.OrOperator;
        public static readonly int OpLevel = OpDefinitions.OrOperatorLevel;
        public static readonly BinaryOperatorMappingList Mapping = new BinaryOperatorMappingList(Symbol);

        #endregion

        public OrOperator()
            : base(Symbol, OpLevel)
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
