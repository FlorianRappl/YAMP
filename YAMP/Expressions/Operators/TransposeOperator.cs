using System;

namespace YAMP
{
	class TransposeOperator : UnaryOperator
	{
		public TransposeOperator () : base("'")
		{
		}
		
		public override Value Perform (Value left)
		{
			if(left is ScalarValue)
				return left;
			else if(left is MatrixValue)
				return (left as MatrixValue).Transpose();
			
			throw new OperationNotSupportedException("'", left);
		}
	}
}

