namespace YAMP
{
    using System;
    using YAMP.Exceptions;
    using YAMP.Numerics.Optimization;

	[Description("RootFunctionDescription")]
	[Kind(PopularKinds.Function)]
    [Link("RootFunctionLink")]
    sealed class RootFunction : SystemFunction
	{
        public RootFunction(ParseContext context)
            : base(context)
        {
        }

		[Description("RootFunctionDescriptionForFunctionScalar")]
		[Example("root(x => x^2+x, -2)", "RootFunctionExampleForFunctionScalar1")]
		[Example("root(x => x^2+x, 1)", "RootFunctionExampleForFunctionScalar2")]
		public ScalarValue Function(FunctionValue f, ScalarValue x)
		{
			var lambda = new Func<Double, Double>(t =>
			{
                var sv = f.Perform(Context, new ScalarValue(t));

                if (sv is ScalarValue == false)
                {
                    throw new YAMPArgumentInvalidException(Name, sv.Header, 1);
                }

				return ((ScalarValue)sv).Re;
			});

			var newton = new NewtonMethod(lambda, x.Re, 0.00001);
            return new ScalarValue(newton.Result[0, 0]);
		}
	}
}
