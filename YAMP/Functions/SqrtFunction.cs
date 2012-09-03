using System;

namespace YAMP
{
	class SqrtFunction : StandardFunction
	{
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return value.Power(new ScalarValue(0.5)) as ScalarValue;
        }
	}
}

