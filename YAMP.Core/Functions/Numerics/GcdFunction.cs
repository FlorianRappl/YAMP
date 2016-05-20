namespace YAMP
{
    using YAMP.Numerics;

    [Description("GcdFunctionDescription")]
	[Kind(PopularKinds.Function)]
    [Link("GcdFunctionLink")]
    sealed class GcdFunction : ArgumentFunction
	{
		[Description("GcdFunctionDescriptionForMatrix")]
		[Example("gcd([54, 24])", "GcdFunctionExampleForMatrix1")]
		[Example("gcd([54, 24, 42])", "GcdFunctionExampleForMatrix2")]
        public ScalarValue Function(MatrixValue values)
		{
			if (values.Length != 0)
            {
                var gcd = values[1].GetIntegerOrThrowException("values", Name);

                for (var i = 2; i <= values.Length; i++)
                {
                    gcd = Helpers.GCD(gcd, values[i].GetIntegerOrThrowException("values", Name));
                }

                return new ScalarValue(gcd);
            }

            return new ScalarValue();
		}
	}
}
