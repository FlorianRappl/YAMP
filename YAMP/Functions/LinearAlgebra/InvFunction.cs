using System;

namespace YAMP
{
	class InvFunction : StandardFunction
	{
		public override Value Perform (Value argument)
		{
			if(argument is ScalarValue)
				return 1.0 / (argument as ScalarValue);
			else if (argument is MatrixValue)
			{
				var m = argument as MatrixValue;
				return m.Inverse();
			}

			throw new OperationNotSupportedException("inv", argument);
		}
	}
}

