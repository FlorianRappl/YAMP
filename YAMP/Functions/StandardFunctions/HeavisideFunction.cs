using System;

namespace YAMP
{
	[Description("Represents the heaviside step function.")]
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

        [Description("Returns 0 for values smaller or equal to 0, else 1.")]
        [Example("heaviside(3-4)", "Results in 0.")]
        [Example("heaviside(3-2)", "Results in 1.")]
        public override ScalarValue Function(ScalarValue x)
        {
            return base.Function(x);
        }
    }
}
