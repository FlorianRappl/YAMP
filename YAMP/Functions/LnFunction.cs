using System;

namespace YAMP
{
	class LnFunction : IFunction
	{
		#region IFunction implementation
		
		public Value Perform (Value argument)
		{
			if(argument is ScalarValue)
				return (argument as ScalarValue).Ln();
			
			throw new OperationNotSupportedException("ln", argument);
		}
		
		#endregion
}
}

