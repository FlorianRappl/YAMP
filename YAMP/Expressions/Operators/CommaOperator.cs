using System;

namespace YAMP
{
	class CommaOperator : BinaryOperator
	{
		public CommaOperator () : base(",", 2)
		{
		}
		
		public override Value Perform (Value left, Value right)
		{
			if(IsList)
				return ArgumentsValue.Create(left, right);
			else if(IsNumeric(left) && IsNumeric(right))
				return MatrixValue.Create(left).AddColumn(right);
			
			throw new OperationNotSupportedException(",", left);
		}

		bool IsNumeric (Value value)
		{
			return value is MatrixValue || value is ScalarValue;
		}
	}
}

