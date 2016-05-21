namespace YAMP
{
    using System;

    /// <summary>
    /// This class contains the construction plan for a power operator.
    /// </summary>
	class PowerOperator : BinaryOperator
    {
        #region Mapping

        public static readonly BinaryOperatorMappingList Mapping = new BinaryOperatorMappingList();
        public static readonly String Symbol = "^";

        #endregion

        #region ctor

		public PowerOperator () : 
            base(Symbol, 100)
		{
			IsRightToLeft = true;
        }

        #endregion

        #region Methods
		
		public override Value Perform (Value left, Value right)
        {
            return PerformOverFind(left, right, Mapping);
		}

        public override Operator Create()
        {
            return new PowerOperator();
        }

        #endregion
	}
}

