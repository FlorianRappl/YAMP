namespace YAMP
{
    using System;
    using YAMP.Numerics;

    [Description("RandnFunctionDescription")]
    [Kind(PopularKinds.Random)]
    [Link("RandnFunctionLink")]
    sealed class RandnFunction : ArgumentFunction
	{	
		readonly NormalDistribution Distribution = new NormalDistribution(Rng);

		[Description("RandnFunctionDescriptionForVoid")]
		public ScalarValue Function()
		{
            return new ScalarValue(Gaussian());
		}

		[Description("RandnFunctionDescriptionForScalar")]
		[Example("randn(3)", "RandnFunctionExampleForScalar1")]
		public MatrixValue Function(ScalarValue dim)
		{
			return Function(dim, dim);
		}

		[Description("RandnFunctionDescriptionForScalarScalar")]
		[Example("randn(3, 1)", "RandnFunctionExampleForScalarScalar1")]
		public MatrixValue Function(ScalarValue rows, ScalarValue cols)
		{
            return Function(rows, cols, new ScalarValue(), new ScalarValue(1.0));
		}

		[Description("RandnFunctionDescriptionForScalarScalarScalarScalar")]
		[Example("randn(3, 1, 10, 2.5)", "RandnFunctionExampleForScalarScalarScalarScalar1")]
		public MatrixValue Function(ScalarValue rows, ScalarValue cols, ScalarValue mu, ScalarValue sigma)
        {
            var k = rows.GetIntegerOrThrowException("rows", Name);
            var l = cols.GetIntegerOrThrowException("cols", Name);
			var m = new MatrixValue(k, l);

            for (var i = 1; i <= l; i++)
            {
                for (var j = 1; j <= k; j++)
                {
                    m[j, i] = new ScalarValue(Gaussian(sigma.Re, mu.Re));
                }
            }

			return m;
		}

		Double Gaussian()
		{
			return Gaussian(1.0, 0.0);
		}

		Double Gaussian(Double sigma, Double mu)
		{
			Distribution.Sigma = sigma;
			Distribution.Mu = mu;
			return Distribution.NextDouble();
		}
	}
}

