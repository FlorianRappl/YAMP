using System;

namespace YAMP
{
	[Description("Represents the sign function.")]
	[Kind(PopularKinds.Function)]
    sealed class SignFunction : StandardFunction
    {
        public override Value Perform(Value argument)
        {
            return base.Perform(argument);
        }

        [Description("Returns -1 for values smaller than 0, +1 for values greater than zero, else 0.")]
        [Example("sign(3-4)", "Results in -1.")]
        [Example("sign(3-2)", "Results in +1.")]
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
