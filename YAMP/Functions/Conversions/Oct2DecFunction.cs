using System;
using System.Collections.Generic;

namespace YAMP
{
    [Description("Converts a octal number to a decimal number.")]
    [Kind(PopularKinds.Conversion)]
    sealed class Oct2DecFunction : ArgumentFunction
    {
        [Description("The function ignores white spaces and converts the given octal input to the equivalent decimal number.")]
        [Example("oct2dec(\"1627\")", "Octal 1627 converts to decimal 919.")]
        public ScalarValue Function(StringValue octstr)
        {
            var sum = 0;
            var hex = new Stack<int>();
            var weight = 1;

            for (var i = 1; i <= octstr.Length; i++)
            {
                var chr = octstr[i];

                if (ParseEngine.IsWhiteSpace(chr))
                    continue;
                else if (ParseEngine.IsNewLine(chr))
                    continue;
                else if (chr >= '0' && chr <= '7')
                    hex.Push((int)(chr - '0'));
                else
                    throw new YAMPRuntimeException("oct2dec can only interpret octal strings.");
            }

            while (hex.Count != 0)
            {
                var el = hex.Pop();
                sum += weight * el;
                weight *= 8;
            }

            return new ScalarValue(sum);
        }
    }
}
