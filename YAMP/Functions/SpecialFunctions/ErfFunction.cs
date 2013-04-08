using System;
using YAMP.Numerics;

namespace YAMP
{
	[Description("In mathematics, the error function (also called the Gauss error function) is a special function (non-elementary) of sigmoid shape which occurs in probability, statistics and partial differential equations. This function returns the error function of the specified number.")]
	[Kind(PopularKinds.Function)]
    sealed class ErfFunction : StandardFunction
	{
		protected override ScalarValue GetValue(ScalarValue value)
		{
			return ErrorFunction.Erf(value);
		}
	}
}
