using System;

namespace YAMP
{
	class FloorFunction : IFunction
	{
		#region IFunction implementation
		
		public Value Perform (Value argument)
		{
			if(argument is ScalarValue)
			{
				var v = Math.Floor((argument as ScalarValue).Value);
				return new ScalarValue(v);
			}
			
			throw new OperationNotSupportedException("floor", argument);
		}
		
		#endregion		
	}
}

