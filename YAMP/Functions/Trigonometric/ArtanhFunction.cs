using System;

namespace YAMP
{
    class ArtanhFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return 0.5 * ((1.0 + value) / (1.0 - value)).Ln();
        }
    }
}
