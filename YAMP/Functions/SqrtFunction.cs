using System;

namespace YAMP
{
	class SqrtFunction : IFunction
	{
		#region IFunction implementation
		
		public Value Perform (Value argument)
		{
			if(argument is ScalarValue)
			{
				return (argument as ScalarValue).Power(new ScalarValue(0.5));
			}
			
			throw new OperationNotSupportedException("sqrt", argument);
		}
		
		#endregion	
	}
}

