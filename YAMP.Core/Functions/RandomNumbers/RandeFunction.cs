namespace YAMP
{
    using System;
    using YAMP.Numerics;

    [Description("RandeFunctionDescription")]
    [Kind(PopularKinds.Random)]
    [Link("RandeFunctionLink")]
    sealed class RandeFunction : ArgumentFunction
	{
		readonly ExponentialDistribution Distribution = new ExponentialDistribution();

		[Description("RandeFunctionDescriptionForVoid")]
		[Example("rande()", "RandeFunctionExampleForVoid1")]
		public ScalarValue Function()
		{
			return new ScalarValue(Exponential());
		}

		[Description("RandeFunctionDescriptionForScalar")]
		[Example("rande(3)", "RandeFunctionExampleForScalar1")]
		public MatrixValue Function(ScalarValue dim)
        {
			return Function(dim, dim);
		}

		[Description("RandeFunctionDescriptionForScalarScalar")]
		[Example("rande(3, 1)", "RandeFunctionExampleForScalarScalar1")]
		public MatrixValue Function(ScalarValue rows, ScalarValue cols)
		{
            return Function(rows, cols, new ScalarValue(1.0));
		}

		[Description("RandeFunctionDescriptionForScalarScalarScalar")]
		[Example("rande(3, 1, 2.5)", "RandeFunctionExampleForScalarScalarScalar1")]
		public MatrixValue Function(ScalarValue rows, ScalarValue cols, ScalarValue lambda)
        {
            var k = rows.GetIntegerOrThrowException("rows", Name);
            var l = cols.GetIntegerOrThrowException("cols", Name);
			var m = new MatrixValue(k, l);

            for (var i = 1; i <= l; i++)
            {
                for (var j = 1; j <= k; j++)
                {
                    m[j, i] = new ScalarValue(Exponential(lambda.Re));
                }
            }

			return m;
		}

		Double Exponential()
		{
			return Exponential(1.0);
		}

		Double Exponential(Double lambda)
		{
			Distribution.Lambda = lambda;
			return Distribution.NextDouble();
		}
	}
}

