using System;

namespace YAMP
{
	class ColonOperator : Operator
	{
		public ColonOperator () : base(";", 1)
		{
		}
		
		public override Value Perform (Value left, Value right)
		{
			if(left is MatrixValue || left is ScalarValue)
				return MatrixValue.Create(left).AddRow(right);
			
			throw new OperationNotSupportedException(";", left);
		}
	}
}

