namespace YAMP
{
    using System;
    using YAMP.Exceptions;
    using YAMP.Numerics;

    [Description("OdeFunctionDescription")]
	[Kind(PopularKinds.Function)]
    [Link("OdeFunctionLink")]
    sealed class OdeFunction : SystemFunction
	{
        public OdeFunction(ParseContext context)
            : base(context)
        {
        }

		[Description("OdeFunctionDescriptionForFunctionMatrixScalar")]
		[Example("ode((t, x) => -x, 0:0.01:2, 1)", "OdeFunctionExampleForFunctionMatrixScalar1")]
		[Example("ode((t, x) => x - t, 0:0.01:5, 1.5)", "OdeFunctionExampleForFunctionMatrixScalar2")]
		public MatrixValue Function(FunctionValue deq, MatrixValue points, ScalarValue x0)
		{
			var lambda = new Func<Double, Double, Double>((t, x) => 
			{
                var av = new ArgumentsValue(new ScalarValue(t), new ScalarValue(x));
				var sv = deq.Perform(Context, av);

                if (!(sv is ScalarValue))
                {
                    throw new YAMPArgumentInvalidException(Name, sv.Header, 1);
                }

				return ((ScalarValue)sv).Re;
			});

			var euler = new RungeKutta(lambda, points[1].Re, points[points.Length].Re, x0.Re, points.Length - 1);
			return new MatrixValue(euler.Result);
		}
	}
}
