using System;

namespace YAMP
{
	class DetFunction : StandardFunction
	{
		public override Value Perform (Value argument)
		{
			if(argument is MatrixValue)
				return (argument as MatrixValue).Det();
			else if(argument is ScalarValue)
				return argument;
			
			
			throw new OperationNotSupportedException("det", argument); 
		}
	}
}

