using System;
using System.Collections.Generic;

namespace YAMP
{
    [Description("Converts a hexadecimal number to a decimal number.")]
    [Kind(PopularKinds.Conversion)]
    sealed class Hex2DecFunction : ArgumentFunction
    {
        [Description("The function ignores white spaces and converts the given hexadecimal input to the equivalent decimal number.")]
        [Example("hex2dec(\"FF\")", "Hexadecimal FF converts to decimal 255.")]
        public ScalarValue Function(StringValue hexstr)
        {
            var sum = 0;
            var hex = new Stack<int>();
            var weight = 1;

            for (var i = 1; i <= hexstr.Length; i++)
            {
                var chr = hexstr[i];

                if (ParseEngine.IsWhiteSpace(chr))
                    continue;
                else if (ParseEngine.IsNewLine(chr))
                    continue;
                else if (chr >= '0' && chr <= '9')
                    hex.Push((int)(chr - '0'));
                else if (chr >= 'A' && chr <= 'F')
                    hex.Push((int)(chr - 'A') + 10);
                else if (chr >= 'a' && chr <= 'f')
                    hex.Push((int)(chr - 'a') + 10);
                else
                    throw new YAMPRuntimeException("hex2dec can only interpret hexadecimal strings.");
            }

            while (hex.Count != 0)
            {
                var el = hex.Pop();
                sum += weight * el;
                weight *= 16;
            }

            return new ScalarValue(sum);
        }
    }
}
