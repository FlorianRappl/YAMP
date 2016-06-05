namespace YAMP
{
    using System;

    /// <summary>
    /// This is the representation of a lighter or equal to operator.
    /// </summary>
    class LtEqOperator : LogicOperator
    {
        #region Mapping

        public static readonly String Symbol = OpDefinitions.LtEqOperator;
        public static readonly int OpLevel = OpDefinitions.LtEqOperatorLevel;
        public static readonly BinaryOperatorMappingList Mapping = new BinaryOperatorMappingList(Symbol);

        #endregion

        public LtEqOperator ()
            : base(Symbol, OpLevel)
		{
		}

		public override ScalarValue Compare (ScalarValue left, ScalarValue right)
		{
			return new ScalarValue(left <= right);
		}

        public override Operator Create()
        {
            return new LtEqOperator();
        }
    }
}
