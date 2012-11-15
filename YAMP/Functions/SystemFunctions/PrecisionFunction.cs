using System;

namespace YAMP
{
	[Description("Gets or sets the precision set for display purposes.")]
	[Kind(PopularKinds.System)]
    class PrecisionFunction : SystemFunction
    {
        [Description("Gets the currently set precision in digits.")]
        [Example("precision()")]
        public ScalarValue Function()
        {
            return new ScalarValue(Context.Precision);
        }

        [Description("Sets the output precision to x digits.")]
        [Example("precision(5)", "Sets the precision to 5 digits.")]
        public StringValue Function(ScalarValue precision)
        {
            Context.Precision = precision.IntValue;
            return new StringValue("Output precision changed to " + Context.Precision + " digits.");
        }
    }
}
