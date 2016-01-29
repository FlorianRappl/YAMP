using System;
using YAMP.Numerics;

namespace YAMP
{
    [Description("The Faddeeva function or Kramp function is a scaled complex complementary error function. It is related to the Fresnel integral, to Dawson's integral, and to the Voigt function. The function also arises frequently in problems involving small-amplitude waves propagating through Maxwellian plasmas, and in particular appears in the plasma's permittivity from which dispersion relations are derived, hence it is sometimes referred to as the plasma dispersion function.")]
    [Kind(PopularKinds.Function)]
    sealed class Faddeeva : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return ErrorFunction.Faddeeva(value);
        }
    }
}
