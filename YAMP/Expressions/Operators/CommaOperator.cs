using System;

namespace YAMP
{
	class CommaOperator : Operator
	{
		public CommaOperator () : base(",", 2)
		{
		}
		
		public override Value Perform (Value left, Value right)
		{
			if(left is MatrixValue || left is ScalarValue)
				return MatrixValue.Create(left).AddColumn(right);
			
			throw new OperationNotSupportedException(",", left);
		}
	}
}

