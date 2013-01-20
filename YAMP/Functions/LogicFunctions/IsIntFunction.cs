using System;

namespace YAMP
{
	[Description("Returns a boolean matrix to state if the given values are integers.")]
	[Kind(PopularKinds.Logic)]
    class IsIntFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return new ScalarValue(value.IsInt);
        }
    }
}
