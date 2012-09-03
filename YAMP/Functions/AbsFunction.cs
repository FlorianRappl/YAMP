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
			else if(argument is VectorValue)
			{
				return (argument as VectorValue).Abs();
			}
			else if(argument is MatrixValue)
			{
				var m = argument as MatrixValue;
				
				if(m.DimensionX == 1)
				{
					return m[1].Abs();
				}
				else if(m.DimensionY == 1)
				{
					return m.GetRowVector(1).Abs();
				}
				
				return m.Det();
			}
			
			throw new OperationNotSupportedException("abs", argument);
		}
	}
}

