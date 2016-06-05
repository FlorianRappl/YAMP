namespace YAMP
{
    using System;

    /// <summary>
    /// The class for the standard + operator.
    /// </summary>
	class PlusOperator : BinaryOperator
    {
        #region Mapping

        public static readonly String Symbol = OpDefinitions.PlusOperator;
        public static readonly int OpLevel = OpDefinitions.PlusOperatorLevel;
        public static readonly BinaryOperatorMappingList Mapping = new BinaryOperatorMappingList(Symbol);

        #endregion

        #region ctor

        public PlusOperator () : 
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
            return new PlusOperator();
        }

        #endregion
    }
}

