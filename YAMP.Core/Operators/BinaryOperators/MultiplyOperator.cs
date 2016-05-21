namespace YAMP
{
    using System;

    /// <summary>
    /// The class for the standard multiply operator.
    /// </summary>
	class MultiplyOperator : BinaryOperator
    {
        #region Mapping

        public static readonly BinaryOperatorMappingList Mapping = new BinaryOperatorMappingList();
        public static readonly String Symbol = "*";

        #endregion

        #region ctor

        public MultiplyOperator () :
            base(Symbol, 10)
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

