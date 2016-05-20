namespace YAMP
{
    using System;

    [Description("FplotFunctionDescription")]
	[Kind(PopularKinds.Plot)]
	sealed class FplotFunction : VisualizationFunction
	{
        public FplotFunction(ParseContext context)
            : base(context)
        {
        }

        [Description("FplotFunctionDescriptionForFunction")]
        [Example("fplot(sin)", "FplotFunctionExampleForFunction1")]
        [Example("fplot(x => sin(x) * cos(x))", "FplotFunctionExampleForFunction2")]
		public Plot2DValue Function(FunctionValue f)
		{
			return Plot(f, -1.0, 1.0, 0.05);
		}

        [Description("FplotFunctionDescriptionForFunctionScalarScalar")]
        [Example("fplot(sin, 0, 2 * pi)", "FplotFunctionExampleForFunctionScalarScalar1")]
		public Plot2DValue Function(FunctionValue f, ScalarValue min, ScalarValue max)
		{
			return Plot(f, min.Re, max.Re, 0.05);
		}

        [Description("FplotFunctionDescriptionForFunctionScalarScalarScalar")]
        [Example("fplot(sin, 0, 2 * pi, 10^-3)", "FplotFunctionExampleForFunctionScalarScalarScalar1")]
		public Plot2DValue Function(FunctionValue f, ScalarValue min, ScalarValue max, ScalarValue precision)
		{
			return Plot(f, min.Re, max.Re, precision.Re);
		}

		Plot2DValue Plot(IFunction f, Double minx, Double maxx, Double precision)
		{
			var cp = new Plot2DValue();
			var N = (Int32)((maxx - minx) / precision) + 1;
			var M = new MatrixValue(N, 2);
			var x = new ScalarValue(minx);

			for (var i = 0; i < N; i++)
			{
				var row = i + 1;
				var y = f.Perform(Context, x);
				M[row, 1] = x.Clone();

                if (y is ScalarValue)
                {
                    M[row, 2] = (ScalarValue)y;
                }
                else if (y is MatrixValue)
                {
                    var Y = (MatrixValue)y;

                    for (var j = 1; j <= Y.Length; j++)
                    {
                        M[row, j + 1] = Y[j];
                    }
                }

				x.Re += precision;
			}

			cp.AddPoints(M);
			return cp;
		}
	}
}
