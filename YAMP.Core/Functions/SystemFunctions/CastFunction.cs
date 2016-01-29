using System;

namespace YAMP
{
    [Description("Casts a given string value to a real scalar.")]
    [Kind(PopularKinds.System)]
    sealed class CastFunction : SystemFunction
    {
        [Description("Uses a given string and casts it into a real scalar.")]
        [Example("cast(\"5\")", "Casts a string 5 to a number 5.")]
        [Example("cast(\"0.1\")", "Casts a string 0.1 to a number 0.1.")]
        [Example("cast(\"3e-5\")", "Casts a string 3e-5 to a number 0.00003.")]
        [Example("cast(prompt(\"Enter something\"))", "Casts the input of a user to a number.")]
        public ScalarValue Function(StringValue original)
        {
            return new ScalarValue(Value.CastStringToDouble(original.Value));
        }
    }
}
