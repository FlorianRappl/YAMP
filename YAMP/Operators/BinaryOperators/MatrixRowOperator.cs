using System;

namespace YAMP
{
	class MatrixRowOperator : BinaryOperator
	{
		public MatrixRowOperator() : base(";", 0)
		{
		}

		public override Value Perform(Value left, Value right)
		{
			if (!(left is NumericValue))
				throw new OperationNotSupportedException(";", left);

			if (!(right is NumericValue))
				throw new OperationNotSupportedException(";", right);

			return MatrixValue.Create(left).AddRow(right);
		}

		public override void RegisterElement()
		{
			MatrixParseTree.Register(this);
		}  
	}
}
