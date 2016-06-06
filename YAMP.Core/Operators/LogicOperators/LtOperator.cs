namespace YAMP
{
    using System;

    /// <summary>
    /// This is the lighter than operator.
    /// </summary>
	class LtOperator : LogicOperator
	{
        #region Mapping

        public static readonly String Symbol = OpDefinitions.LtOperator;
        public static readonly int OpLevel = OpDefinitions.LtOperatorLevel;
        public static readonly BinaryOperatorMappingList Mapping = new BinaryOperatorMappingList(Symbol);

        #endregion

        public LtOperator ()
            : base(Symbol, OpLevel)
		{
		}

		public override ScalarValue Compare(ScalarValue left, ScalarValue right)
		{
            return new ScalarValue(left < right);
		}

        public override Operator Create()
        {
            return new LtOperator();
        }
	}
}

