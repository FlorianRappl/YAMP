namespace YAMP
{
    using System;

    /// <summary>
    /// This is the class used for the operator that seperated various arguments
    /// in round brackets.
    /// </summary>
	class CommaOperator : BinaryOperator
    {
        #region Mapping

        public static readonly String Symbol = OpDefinitions.CommaOperator;
        public static readonly int OpLevel = OpDefinitions.CommaOperatorLevel;
        public static readonly BinaryOperatorMappingList Mapping = new BinaryOperatorMappingList(Symbol);

        #endregion

        #region ctor

        public CommaOperator() : 
            base(Symbol, OpLevel)
		{
		}

        public CommaOperator(ParseEngine engine) : 
            this()
        {
            StartLine = engine.CurrentLine;
            StartColumn = engine.CurrentColumn;
        }

        #endregion

        #region Methods

        public override Value Perform(Value left, Value right)
		{
			return ArgumentsValue.Create(left, right);
		}

        public override void RegisterElement(IElementMapping elementMapping)
		{
            //Nothing to do here.
		}

        public override Operator Create()
        {
            return new CommaOperator();
        }

        #endregion
    }
}
