namespace YAMP
{
    /// <summary>
    /// This is the left divide operator \
    /// </summary>
	class LeftDivideOperator : BinaryOperator
    {
        #region ctor

        public LeftDivideOperator () : 
            base(@"\", 20)
		{
		}

        #endregion

        #region Methods

        public override Value Perform (Value left, Value right)
        {
            return PerformOverFind(left, right, RightDivideOperator.Mapping);
		}

        public override Operator Create()
        {
            return new LeftDivideOperator();
        }

        #endregion
    }
}

