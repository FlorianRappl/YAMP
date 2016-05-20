namespace YAMP
{
    using System;

    [Description("LogFunctionDescription")]
	[Kind(PopularKinds.Function)]
    sealed class LogFunction : ArgumentFunction
	{
        [Description("LogFunctionDescriptionForScalar")]
        [Example("log(e)", "LogFunctionExampleForScalar1")]
        public ScalarValue Function(ScalarValue z)
        {
            return GetValue(z, Math.E);
        }

        [Description("LogFunctionDescriptionForScalarScalar")]
        [Example("log(16, 2)", "LogFunctionExampleForScalarScalar1")]
        public ScalarValue Function(ScalarValue z, ScalarValue b)
        {
            return GetValue(z, b.Re);
        }

        [Description("LogFunctionDescriptionForMatrix")]
        [Example("log(randi(10, 0, 1000))", "LogFunctionExampleForMatrix1")]
        public MatrixValue Function(MatrixValue Z)
        {
            return Function(Z, new ScalarValue(Math.E));
        }

        [Description("LogFunctionDescriptionForMatrixScalar")]
        [Example("log(randi(10, 0, 1000), 10)", "LogFunctionExampleForMatrixScalar1")]
        public MatrixValue Function(MatrixValue Z, ScalarValue b)
        {
            var bse = b.Re;
            var M = new MatrixValue(Z.DimensionY, Z.DimensionX);

            for (var j = 1; j <= Z.DimensionY; j++)
            {
                for (var i = 1; i <= Z.DimensionX; i++)
                {
                    M[j, i] = GetValue(Z[j, i], bse);
                }
            }

            return M;
        }

		static ScalarValue GetValue(ScalarValue value, Double newBase)
		{
			return value.Log(newBase);
		}
	}
}

