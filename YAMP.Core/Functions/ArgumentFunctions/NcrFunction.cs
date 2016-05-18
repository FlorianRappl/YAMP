namespace YAMP
{
    [Description("NcrFunctionDescription")]
    [Kind(PopularKinds.Function)]
    [Link("NcrFunctionLink")]
    sealed class NcrFunction : ArgumentFunction
    {
        [Description("NcrFunctionDescriptionForScalarScalar")]
        [Example("ncr(8, 2)", "NcrFunctionExampleForScalarScalar1")]
        public ScalarValue Function(ScalarValue n, ScalarValue r)
        {
            return n.Factorial() / (r.Factorial() * (n - r).Factorial());
        }
    }
}
