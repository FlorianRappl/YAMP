namespace YAMP
{
    using YAMP.Numerics;

    [Description("RandiFunctionDescription")]
    [Kind(PopularKinds.Random)]
    [Link("RandiFunctionLink")]
    sealed class RandiFunction : ArgumentFunction
	{
		readonly DiscreteUniformDistribution Distribution = new DiscreteUniformDistribution(Rng);

		[Description("RandiFunctionDescriptionForVoid")]
		[Example("randi(0, 10)", "RandiFunctionExampleForVoid1")]
		public ScalarValue Function (ScalarValue min, ScalarValue max)
        {
            Distribution.Alpha = min.GetIntegerOrThrowException("min", Name);
			Distribution.Beta = max.GetIntegerOrThrowException("max", Name);
			return new ScalarValue(Distribution.Next());
		}

		[Description("RandiFunctionDescriptionForScalarScalarScalar")]
		[Example("randi(5, 0, 10)", "RandiFunctionExampleForScalarScalarScalar1")]
		public MatrixValue Function(ScalarValue dim, ScalarValue min, ScalarValue max)
        {
            Distribution.Alpha = min.GetIntegerOrThrowException("min", Name);
            Distribution.Beta = max.GetIntegerOrThrowException("max", Name);
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
                    m[j, i] = new ScalarValue(Distribution.Next());
                }
            }
			
			return m;
		}

		[Description("RandiFunctionDescriptionForScalarScalarScalarScalar")]
		[Example("randi(5, 2, 0, 10)", "RandiFunctionExampleForScalarScalarScalarScalar1")]
		public MatrixValue Function(ScalarValue rows, ScalarValue cols, ScalarValue min, ScalarValue max)
        {
            Distribution.Alpha = min.GetIntegerOrThrowException("min", Name);
            Distribution.Beta = max.GetIntegerOrThrowException("max", Name);
            var k = rows.GetIntegerOrThrowException("rows", Name);
            var l = cols.GetIntegerOrThrowException("cols", Name);
			var m = new MatrixValue(k, l);

            for (var i = 1; i <= l; i++)
            {
                for (var j = 1; j <= k; j++)
                {
                    m[j, i] = new ScalarValue(Distribution.Next());
                }
            }
			
			return m;
		}
	}
}

