namespace YAMP
{
    using System;

    /// <summary>
    /// The class for the standard modulo operator.
    /// </summary>
	class ModuloOperator : BinaryOperator
    {
        #region Mapping

        public static readonly String Symbol = OpDefinitions.ModuloOperator;
        public static readonly int OpLevel = OpDefinitions.ModuloOperatorLevel;
        public static readonly BinaryOperatorMappingList Mapping = new BinaryOperatorMappingList(Symbol);

        #endregion

        #region ctor

        public ModuloOperator() : 
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
            return new ModuloOperator();
        }

        #endregion
    }
}
