using System;

namespace YAMP
{
    [Description("The standard sin(x) function.")]
	class SinFunction : StandardFunction
	{
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return value.Sin();
        }
	}
}

