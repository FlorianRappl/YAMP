﻿using System;
using YAMP.Numerics;

namespace YAMP
{
    [Description("Represents the gamma function, which is the analytic continuation of the faculty for non-integers. The gamma function is defined for all complex numbers except the non-positive integers. For complex numbers with a positive real part, it is defined via an improper integral that converges.")]
	[Kind(PopularKinds.Function)]
    class GammaFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            if (value.IntValue == value.Value && value.IntValue < 0)
                throw new ArgumentException("everything except non-positive integers", value.Value.ToString());

			return Gamma.LinearGamma(value);
        }
    }
}
