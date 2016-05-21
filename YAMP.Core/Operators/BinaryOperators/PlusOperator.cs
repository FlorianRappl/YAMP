namespace YAMP
{
    using System;

    /// <summary>
    /// The class for the standard + operator.
    /// </summary>
	class PlusOperator : BinaryOperator
    {
        #region Mapping

        public static readonly BinaryOperatorMappingList Mapping = new BinaryOperatorMappingList();
        public static readonly String Symbol = "+";

        #endregion

        #region ctor

		public PlusOperator () : 
            base(Symbol, 5)
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

