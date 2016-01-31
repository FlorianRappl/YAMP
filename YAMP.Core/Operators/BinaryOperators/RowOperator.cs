namespace YAMP
{
    using YAMP.Exceptions;

    /// <summary>
    /// This class represents the matrix row seperator operator ;.
    /// </summary>
	class RowOperator : BinaryOperator
    {
        #region ctor

        public RowOperator() : base(";", 0)
		{
		}

        public RowOperator(ParseEngine engine)
            : this()
        {
            StartColumn = engine.CurrentColumn;
            StartLine = engine.CurrentLine;
        }

        #endregion

        #region Methods

        public override Value Perform(Value left, Value right)
		{
			if (!(left is NumericValue))
				throw new YAMPOperationInvalidException(";", left);

			if (!(right is NumericValue))
				throw new YAMPOperationInvalidException(";", right);

			return MatrixValue.Create(left).AddRow(right);
		}

        public override void RegisterElement(Elements elements)
		{
            //Nothing to do here.
		}

        public override Operator Create()
        {
            return new RowOperator();
        }

        #endregion
    }
}
