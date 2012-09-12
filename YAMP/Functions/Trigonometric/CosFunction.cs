using System;

namespace YAMP
{
	class CosFunction : StandardFunction
	{
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return value.Cos();
        }
	}
}

