namespace YAMP
{
    using System;
    using YAMP.Exceptions;

    /// <summary>
    /// Represents a transpose operator (in analogy to the adjungate operator).
    /// </summary>
	class TransposeOperator : RightUnaryOperator
	{
        public static readonly String Symbol = OpDefinitions.TransposeOperator;
        public static readonly int OpLevel = OpDefinitions.TransposeOperatorLevel;

        public TransposeOperator () :
            base(Symbol, OpLevel)
		{
		}
		
		public override Value Perform (Value left)
		{
            if (left is ScalarValue)
            {
                return (left as ScalarValue).Clone();
            }
            else if (left is MatrixValue)
            {
                return (left as MatrixValue).Transpose();
            }
			
			throw new YAMPOperationInvalidException(".'", left);
		}

        public override Operator Create()
        {
            return new TransposeOperator();
        }
	}
}

