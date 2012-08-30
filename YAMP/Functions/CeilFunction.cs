using System;

namespace YAMP
{
	class CeilFunction : IFunction
	{
		#region IFunction implementation
		
		public Value Perform (Value argument)
		{
			if(argument is ScalarValue)
			{
				var v = Math.Ceiling((argument as ScalarValue).Value);
				return new ScalarValue(v);
			}
			
			throw new OperationNotSupportedException("ceil", argument);
		}
		
		#endregion		
	}
}

