using System;

namespace YAMP
{
    [Description("Gets or sets the precision set for display purposes.")]
    class PrecisionFunction : ArgumentFunction
    {
        [Description("Gets the currently set precision in digits.")]
        [Example("precision()")]
        public ScalarValue Function()
        {
            return new ScalarValue(Tokens.Precision);
        }

        [Description("Sets the output precision to x digits.")]
        [Example("precision(5)", "Sets the precision to 5 digits.")]
        public StringValue Function(ScalarValue precision)
        {
            Tokens.Precision = precision.IntValue;
            return new StringValue("Output precision changed to " + Tokens.Precision + " digits.");
        }
    }
}
