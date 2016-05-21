namespace YAMP
{
    using System;

    /// <summary>
    /// The standard right divide / operator.
    /// </summary>
	class RightDivideOperator : BinaryOperator
    {
        #region Mapping

        public static readonly BinaryOperatorMappingList Mapping = new BinaryOperatorMappingList();
        public static readonly String Symbol = "/";

        #endregion

        #region ctor

        public RightDivideOperator () : 
            base(Symbol, 20)
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

