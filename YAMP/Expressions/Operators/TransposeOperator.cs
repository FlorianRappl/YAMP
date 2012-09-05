using System;

namespace YAMP
{
	class TransposeOperator : Operator
	{
		public TransposeOperator () : base("'", 200)
		{
		}
		
		public override Value Perform (Value left, Value right)
		{
			if(left is ScalarValue)
				return left;
			else if(left is MatrixValue)
				return (left as MatrixValue).Transpose();
			
			throw new OperationNotSupportedException("'", left);
		}
		
		public override string Set (string input)
		{
			return "0" + base.Set (input);
		}
	}
}

