namespace YAMP
{
    using YAMP.Exceptions;

    /// <summary>
    /// This is the operator for adjungating a matrix.
    /// </summary>
	class AdjungateOperator : RightUnaryOperator
	{
		public AdjungateOperator () : base("'", 100)
		{
		}
		
		public override Value Perform (Value left)
		{
            if (left is ScalarValue)
            {
                return (left as ScalarValue).Conjugate();
            }
            else if (left is MatrixValue)
            {
                return (left as MatrixValue).Adjungate();
            }
			
			throw new YAMPOperationInvalidException("'", left);
		}

        public override Operator Create()
        {
            return new AdjungateOperator();
        }
	}
}

