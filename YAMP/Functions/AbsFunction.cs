using System;

namespace YAMP
{
	class AbsFunction : StandardFunction
	{		
		public override Value Perform (Value argument)
		{
			if(argument is ScalarValue)
			{
				return (argument as ScalarValue).Abs();
			}
			else if(argument is MatrixValue)
			{
				var m = argument as MatrixValue;
				
				if(m.DimensionX == 1)
					return m.Abs();
				else if(m.DimensionY == 1)
					return m.Abs();
				
				return m.Det();
			}
			
			throw new OperationNotSupportedException("abs", argument);
		}
	}
}

