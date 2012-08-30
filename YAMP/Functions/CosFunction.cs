using System;

namespace YAMP
{
	class CosFunction : IFunction
	{
		#region IFunction implementation
		
		public Value Perform (Value argument)
		{
			if(argument is ScalarValue)
			{
				return (argument as ScalarValue).Cos();
			}
			
			throw new OperationNotSupportedException("cos", argument);
		}
		
		#endregion	
	}
}

