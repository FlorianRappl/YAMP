namespace YAMP
{
    using System;
    using YAMP.Exceptions;

    /// <summary>
    /// The matrix column operator , - used to seperate various columns in the
    /// entered matrix.
    /// </summary>
	class ColumnOperator : BinaryOperator
    {
        #region Mapping

        public static readonly String Symbol = OpDefinitions.ColumnOperator;
        public static readonly int OpLevel = OpDefinitions.ColumnOperatorLevel;

        public static readonly BinaryOperatorMappingList Mapping = new BinaryOperatorMappingList(Symbol);

        #endregion

        #region ctor

        public ColumnOperator() : 
            base(Symbol, OpLevel)
		{
		}

        public ColumnOperator(ParseEngine engine) :
            this()
        {
            StartColumn = engine.CurrentColumn;
            StartLine = engine.CurrentLine;
        }

        #endregion

        #region Methods

        public override Value Perform(Value left, Value right)
		{
            if (left is NumericValue == false)
            {
                throw new YAMPOperationInvalidException(",", left);
            }

            if (right is NumericValue == false)
            {
                throw new YAMPOperationInvalidException(",", right);
            }

			return MatrixValue.Create(left).AddColumn(right);
		}

        public override void RegisterElement(IElementMapping elementMapping)
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
