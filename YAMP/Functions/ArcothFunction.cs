using System;

namespace YAMP
{
    class ArcothFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return 0.5 * ((1.0 + value) / (value - 1.0)).Ln();
        }
    }
}
