namespace YAMP
{
	[Description("HeavisideFunctionDescription")]
	[Kind(PopularKinds.Function)]
    sealed class HeavisideFunction : StandardFunction
    {
        public override Value Perform(Value argument)
        {
            return base.Perform(argument);
        }

        protected override ScalarValue GetValue(ScalarValue value)
        {
            return new ScalarValue(value.Re > 0 ? 1.0 : 0.0);
        }

        [Description("HeavisideFunctionDescriptionForScalar")]
        [Example("heaviside(3-4)", "HeavisideFunctionExampleForScalar1")]
        [Example("heaviside(3-2)", "HeavisideFunctionExampleForScalar2")]
        public override ScalarValue Function(ScalarValue x)
        {
            return base.Function(x);
        }
    }
}
