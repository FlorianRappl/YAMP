using System;

namespace YAMP
{
    class SinhFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return (value.Exp() - (-value).Exp()) / 2.0;
        }
    }
}
