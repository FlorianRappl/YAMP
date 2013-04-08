using System;

namespace YAMP
{
    [Description("Conjugates the given complex number to transform it from x + iy to x - iy.")]
    [Kind(PopularKinds.Function)]
    sealed class ConjFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return value.Conjugate();
        }
    }
}
