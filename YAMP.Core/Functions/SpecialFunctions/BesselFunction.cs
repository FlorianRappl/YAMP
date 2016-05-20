namespace YAMP
{
    using System;
    using YAMP.Numerics;

    [Description("BesselFunctionDescription")]
	[Kind(PopularKinds.Function)]
    sealed class BesselFunction : ArgumentFunction
	{
		[Description("BesselFunctionDescriptionForScalarScalar")]
		[Example("bessel(3, 0.4)", "BesselFunctionExampleForScalarScalar1")]
		public ScalarValue Function(ScalarValue order, ScalarValue argument)
        {
            var n = order.GetIntegerOrThrowException("order", Name);
			return GetValue(n, argument.Re);
		}

		[Description("BesselFunctionDescriptionForScalarMatrix")]
		[Example("bessel(3, [0, 1, 2, 3, 4])", "BesselFunctionExampleForScalarMatrix1")]
		public MatrixValue Function(ScalarValue order, MatrixValue argument)
        {
            var n = order.GetIntegerOrThrowException("order", Name);
			var M = new MatrixValue(argument.DimensionY, argument.DimensionX);

            for (var j = 1; j <= argument.DimensionY; j++)
            {
                for (var i = 1; i <= argument.DimensionX; i++)
                {
                    M[j, i] = GetValue(n, argument[j, i].Re);
                }
            }

			return M;
		}

		static ScalarValue GetValue(Int32 order, Double value)
		{
            return new ScalarValue(Bessel.jn(order, value));
		}
	}
}
