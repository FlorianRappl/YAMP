using System;

namespace YAMP
{
	class SinFunction : IFunction
	{
		#region IFunction implementation
		
		public Value Perform (Value argument)
		{
			if(argument is ScalarValue)
			{
				return (argument as ScalarValue).Sin();
			}
			
			throw new OperationNotSupportedException("sin", argument);
		}
		
		#endregion	
	}
}

