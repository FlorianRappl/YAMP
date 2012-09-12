using System;

namespace YAMP
{
	class LengthFunction : StandardFunction
	{
		public override Value Perform (Value argument)
		{
			if(argument is ScalarValue)
				return new ScalarValue(1.0);
			else if(argument is MatrixValue)
				return new ScalarValue((argument as MatrixValue).Length);
			else if(argument is StringValue)
				return new ScalarValue((argument as StringValue).Value.Length);

			throw new OperationNotSupportedException("length", argument);
		}
	}
}

