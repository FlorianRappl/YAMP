using System;

namespace YAMP
{
	class ExpFunction : IFunction
	{
		#region IFunction implementation
		
		public Value Perform (Value argument)
		{
			if(argument is ScalarValue)
			{
				return (argument as ScalarValue).Exp();
			}
			
			throw new OperationNotSupportedException("exp", argument);
		}
		
		#endregion		
	}
}

