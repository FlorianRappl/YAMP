using System;

namespace YAMP
{
	class LnFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return value.Ln();
        }
    }
}

