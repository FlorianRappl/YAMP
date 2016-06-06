namespace YAMP
{
    using System;

    /// <summary>
    /// The construction scheme for a greater than operator.
    /// </summary>
	class GtOperator : LogicOperator
	{
        #region Mapping

        public static readonly String Symbol = OpDefinitions.GtOperator;
        public static readonly int OpLevel = OpDefinitions.GtOperatorLevel;
        public static readonly BinaryOperatorMappingList Mapping = new BinaryOperatorMappingList(Symbol);

        #endregion

        public GtOperator ()
            : base(Symbol, OpLevel)
		{
		}

		public override ScalarValue Compare (ScalarValue left, ScalarValue right)
		{
            return new ScalarValue(left > right);
		}

        public override Operator Create()
        {
            return new GtOperator();
        }
	}
}

