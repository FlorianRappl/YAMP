namespace YAMP
{
    using System;

    [Description("LinspaceFunctionDescription")]
	[Kind(PopularKinds.Function)]
    sealed class LinspaceFunction : ArgumentFunction
	{
		[Description("LinspaceFunctionDescriptionForScalarScalarScalar")]
		[Example("linspace(0, 10, 5)", "LinspaceFunctionExampleForScalarScalarScalar1")]
		public MatrixValue Function(ScalarValue from, ScalarValue to, ScalarValue count)
        {
            var c = count.GetIntegerOrThrowException("count", Name);

            if (c < 2)
            {
                throw new ArgumentException("linspace");
            }

			var step = (to.Re - from.Re) / (c - 1);
			return new RangeValue(from.Re, to.Re, step);
		}
	}
}

