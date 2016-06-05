namespace YAMP
{
    using System;

    /// <summary>
    /// This is the left divide operator \
    /// </summary>
	class LeftDivideOperator : BinaryOperator
    {
        #region Mapping

        public static readonly String Symbol = OpDefinitions.LeftDivideOperator;
        public static readonly int OpLevel = OpDefinitions.LeftDivideOperatorLevel;
        public static readonly BinaryOperatorMappingList Mapping = new BinaryOperatorMappingList(Symbol);

        #endregion

        #region ctor

        public LeftDivideOperator () : 
            base(Symbol, OpLevel)
		{
		}

        #endregion

        #region Methods

        public override Value Perform (Value left, Value right)
        {
            //NH:Check with Florian: Why was it using RightDivideOperator.Mapping ?!
            //Was it being used as an "alias" to it?
            //return PerformOverFind(left, right, RightDivideOperator.Mapping);

            return PerformOverFind(left, right, Mapping);
        }

        public override Operator Create()
        {
            return new LeftDivideOperator();
        }

        #endregion
    }
}

