using System;

namespace YAMP
{
	class SqrtFunction : StandardFunction
	{
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return value.Sqrt();
        }
	}
}

