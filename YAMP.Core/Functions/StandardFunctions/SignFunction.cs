namespace YAMP
{
	[Description("SignFunctionDescription")]
	[Kind(PopularKinds.Function)]
    sealed class SignFunction : StandardFunction
    {
        public override Value Perform(Value argument)
        {
            return base.Perform(argument);
        }

        [Description("SignFunctionDescriptionForScalar")]
        [Example("sign(3-4)", "SignFunctionExampleForScalar1")]
        [Example("sign(3-2)", "SignFunctionExampleForScalar2")]
        public override ScalarValue Function(ScalarValue x)
        {
            return base.Function(x);
        }

        protected override ScalarValue GetValue(ScalarValue value)
        {
            return value.Sign();
        }
    }
}
