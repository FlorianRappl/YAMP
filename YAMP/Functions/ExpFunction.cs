using System;

namespace YAMP
{
	class ExpFunction : StandardFunction
	{
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return value.Exp();
        }
	}
}

