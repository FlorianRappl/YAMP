using System;

namespace YAMP
{
	class LogFunction : StandardFunction
	{
		protected override ScalarValue GetValue(ScalarValue value)
		{
			return value.Log();
		}
	}
}

