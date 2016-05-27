using System;
using YAMP;

namespace YAMP.Sets
{
    [Description("The NewRandomSet (new Numeric set) function.")]
    [Kind(PopularKinds.Function)]
    sealed class NewRandomSetFunction : ArgumentFunction
	{
        [Description("Creates a new Unordered Numeric Random Set")]
        public SetValue Function(StringValue expression, ScalarValue size, ScalarValue maxVal)
        {
            return Function(expression, size, maxVal, new ScalarValue(0));
        }

        [Description("Creates a new Numeric Random Set")]
        public SetValue Function(StringValue expression, ScalarValue size, ScalarValue maxVal, ScalarValue ordered)
        {
            SetValue set = new SetValue(expression.Value, null, ordered.Value != 0);

            Random rnd = new Random();

            int tot = (int)size.Value;
            for (int i = 0; i < tot; i++)
            {
                set.Set.Add(new ScalarValue(rnd.Next((int)maxVal.Value)));
            }

            return set;
        }
	}
}

