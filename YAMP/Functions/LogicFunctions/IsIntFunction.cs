using System;

namespace YAMP
{
    class IsIntFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return value.IsInt();
        }
    }
}
