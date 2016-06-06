namespace YAMP
{
    using System;

    /// <summary>
    /// This is the factorial operator !.
    /// </summary>
	class FactorialOperator : RightUnaryOperator
	{
        #region Mapping

        public static readonly String Symbol = OpDefinitions.FactorialOperator;
        public static readonly int OpLevel = OpDefinitions.FactorialOperatorLevel;

        #endregion

        static readonly FactorialFunction fac = new FactorialFunction();
		
		public FactorialOperator() : 
            base(Symbol, OpLevel)
		{
		}
		
		public override Value Perform (Value left)
		{
			return fac.Perform(left);
		}

        public override Operator Create()
        {
            return new FactorialOperator();
        }
	}
}

