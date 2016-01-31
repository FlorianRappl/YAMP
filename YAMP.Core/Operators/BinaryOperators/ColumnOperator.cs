namespace YAMP
{
    using YAMP.Exceptions;

    /// <summary>
    /// The matrix column operator , - used to seperate various columns in the
    /// entered matrix.
    /// </summary>
	class ColumnOperator : BinaryOperator
    {
        #region ctor

        public ColumnOperator() : base(",", 1)
		{
		}

        public ColumnOperator(ParseEngine engine)
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
				throw new YAMPOperationInvalidException(",", left);

			if (!(right is NumericValue))
				throw new YAMPOperationInvalidException(",", right);

			return MatrixValue.Create(left).AddColumn(right);
		}

		public override void RegisterElement(Elements elements)
		{
            //Nothing to do here.
		}

        public override Operator Create()
        {
            return new ColumnOperator();
        }

        #endregion
    }
}
