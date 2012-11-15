using System;

namespace YAMP
{
	class ModuloOperator : BinaryOperator
	{
		static readonly ModFunction mod = new ModFunction();

		public ModuloOperator() : base("%", 30)
		{
		}
		
		public override Value Perform (Value left, Value right)
		{
			if(!(right is ScalarValue))
				throw new OperationNotSupportedException("%", right);

			if(left is ScalarValue)
				return mod.Function(left as ScalarValue, right as ScalarValue);
			else if(left is MatrixValue)
				return mod.Function(left as MatrixValue, right as ScalarValue);

			throw new OperationNotSupportedException("%", left);
		}
	}
}
