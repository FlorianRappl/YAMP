using System;

namespace YAMP
{
    class CotFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return value.Cos() / value.Sin();
        }
    }
}
