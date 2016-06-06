namespace YAMP
{
    using System;

    /// <summary>
    /// The basic greater equals operator.
    /// </summary>
    class GtEqOperator : LogicOperator
    {
        #region Mapping

        public static readonly String Symbol = OpDefinitions.GtEqOperator;
        public static readonly int OpLevel = OpDefinitions.GtEqOperatorLevel;
        public static readonly BinaryOperatorMappingList Mapping = new BinaryOperatorMappingList(Symbol);

        #endregion

        public GtEqOperator ()
            : base(Symbol, OpLevel)
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
