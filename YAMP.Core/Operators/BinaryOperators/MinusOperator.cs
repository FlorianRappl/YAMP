namespace YAMP
{
    using System;

    /// <summary>
    /// The usual - operator.
    /// </summary>
	class MinusOperator : BinaryOperator
    {
        #region Mapping

        public static readonly BinaryOperatorMappingList Mapping = new BinaryOperatorMappingList();
        public static readonly String Symbol = "-";

        #endregion

        #region ctor

        public MinusOperator () : 
            base(Symbol, 6)
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
            return new MinusOperator();
        }

        #endregion
    }
}

