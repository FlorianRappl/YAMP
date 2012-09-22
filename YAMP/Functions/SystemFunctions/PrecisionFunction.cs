using System;

namespace YAMP
{
    class PrecisionFunction : ArgumentFunction
    {
        public Value Function(ScalarValue precision)
        {
            Tokens.Precision = precision.IntValue;
            return new StringValue("Output precision changed to " + Tokens.Precision + " digits.");
        }
    }
}
