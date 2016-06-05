namespace YAMP
{
    using System;

    /// <summary>
    /// The standard right divide / operator.
    /// </summary>
	class RightDivideOperator : BinaryOperator
    {
        #region Mapping

        public static readonly String Symbol = OpDefinitions.RightDivideOperator;
        public static readonly int OpLevel = OpDefinitions.RightDivideOperatorLevel;
        public static readonly BinaryOperatorMappingList Mapping = new BinaryOperatorMappingList(Symbol);

        #endregion

        #region ctor

        public RightDivideOperator () : 
            base(Symbol, OpLevel)
		{
		}


        #endregion

        #region Methods

        public override Value Perform (Value left, Value right)
        {
            return PerformOverFind(left, right, Mapping);
		}

        public override Operator Create()
        {
            return new RightDivideOperator();
        }

        #endregion
    }
}

