namespace YAMP
{
    using System;

    [Description("IsNaNFunctionDescription")]
    [Kind(PopularKinds.Logic)]
    [Link("IsNaNFunctionLink")]
    sealed class IsNaNFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return new ScalarValue(Double.IsNaN(value.Re) || Double.IsNaN(value.Im));
        }
    }
}
