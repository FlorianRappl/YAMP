namespace YAMP
{
    /// <summary>
    /// This is the class used for the operator that seperated various arguments
    /// in round brackets.
    /// </summary>
	class CommaOperator : BinaryOperator
    {
        #region ctor

        public CommaOperator() : base(",", 1)
		{
		}

        public CommaOperator(ParseEngine engine)
            : this()
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
