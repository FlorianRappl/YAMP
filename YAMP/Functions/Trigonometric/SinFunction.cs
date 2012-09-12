using System;

namespace YAMP
{
	class SinFunction : StandardFunction
	{
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return value.Sin();
        }
	}
}

