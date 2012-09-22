using System;

namespace YAMP
{
    class SignFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return new ScalarValue(Math.Sign(value.Value));
        }
    }
}
