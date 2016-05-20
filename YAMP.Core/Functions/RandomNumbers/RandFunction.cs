namespace YAMP
{
    using YAMP.Numerics;

    [Description("RandFunctionDescription")]
    [Kind(PopularKinds.Random)]
    [Link("RandFunctionLink")]
    sealed class RandFunction : ArgumentFunction
	{
		readonly ContinuousUniformDistribution Distribution = new ContinuousUniformDistribution();

		[Description("RandFunctionDescriptionForVoid")]
		public ScalarValue Function()
		{
            return new ScalarValue(Distribution.NextDouble());
		}

		[Description("RandFunctionDescriptionForScalar")]
		[Example("rand(5)", "RandFunctionExampleForScalar1")]
		public MatrixValue Function(ScalarValue dim)
        {
            var k = dim.GetIntegerOrThrowException("dim", Name);

            if (k < 1)
            {
                k = 1;
            }
			
			var m = new MatrixValue(k, k);

            for (var i = 1; i <= k; i++)
            {
                for (var j = 1; j <= k; j++)
                {
                    m[j, i] = new ScalarValue(Distribution.NextDouble());
                }
            }
			
			return m;
		}

		[Description("RandFunctionDescriptionForScalarScalar")]
		[Example("rand(5, 2)", "RandFunctionExampleForScalarScalar1")]
		public MatrixValue Function(ScalarValue rows, ScalarValue cols)
        {
            var k = rows.GetIntegerOrThrowException("rows", Name);
            var l = cols.GetIntegerOrThrowException("cols", Name);
			var m = new MatrixValue(k, l);

            for (var i = 1; i <= l; i++)
            {
                for (var j = 1; j <= k; j++)
                {
                    m[j, i] = new ScalarValue(Distribution.NextDouble());
                }
            }
			
			return m;
		}
	}
}