namespace YAMP
{
    using System;

    /// <summary>
    /// The class for the standard multiply operator.
    /// </summary>
	class MultiplyOperator : BinaryOperator
    {
        #region Mapping

        public static readonly String Symbol = OpDefinitions.MultiplyOperator;
        public static readonly int OpLevel = OpDefinitions.MultiplyOperatorLevel;
        public static readonly BinaryOperatorMappingList Mapping = new BinaryOperatorMappingList(Symbol);

        #endregion

        #region ctor

        public MultiplyOperator () :
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
            return new MultiplyOperator();
        }

        #endregion
    }
}

