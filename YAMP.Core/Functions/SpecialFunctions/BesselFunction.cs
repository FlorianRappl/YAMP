using System;
using YAMP.Numerics;

namespace YAMP
{
	[Description("In mathematics, Bessel functions, first defined by the mathematician Daniel Bernoulli and generalized by Friedrich Bessel, are canonical solutions y(x) of Bessel's differential equation. This function represents Bessel functions of the first kind with the given order.")]
	[Kind(PopularKinds.Function)]
    sealed class BesselFunction : ArgumentFunction
	{
		[Description("Computes the Bessel function of an arbitrary order for the given argument and returns the function's value.")]
		[Example("bessel(3, 0.4)", "Computes the Bessel function of the first kind with order 3 for the argument 0.4 and returns a scalar with the computed value.")]
		public ScalarValue Function(ScalarValue order, ScalarValue argument)
        {
            var n = order.GetIntegerOrThrowException("order", Name);
			return GetValue(n, argument.Re);
		}

		[Description("Computes the Bessel function of an arbitrary order for the given arguments. The return matrix has the same dimensions as the input matrix.")]
		[Example("bessel(3, [0, 1, 2, 3, 4])", "Computes the Bessel function of the first kind with order 3 for the arguments 0, 1, 2, 3 and 4. Returns a matrix (vector) with the computed values.")]
		public MatrixValue Function(ScalarValue order, MatrixValue argument)
        {
            var n = order.GetIntegerOrThrowException("order", Name);
			var M = new MatrixValue(argument.DimensionY, argument.DimensionX);

			for (var j = 1; j <= argument.DimensionY; j++)
				for (var i = 1; i <= argument.DimensionX; i++)
					M[j, i] = GetValue(n, argument[j, i].Re);

			return M;
		}

		ScalarValue GetValue(int order, double value)
		{
            return new ScalarValue(Bessel.jn(order, value));
		}
	}
}
