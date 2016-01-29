using System;

namespace YAMP
{
    [Description("This is a function operating on complex numbers (visualised as a flat plane). It gives the angle between the line joining the point to the origin and the positive real axis known as an argument of the point (that is, the angle between the half-lines of the position vector representing the number and the positive real axis).")]
    [Kind(PopularKinds.Function)]
    sealed class ArgFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return new ScalarValue(value.Arg());
        }
    }
}
