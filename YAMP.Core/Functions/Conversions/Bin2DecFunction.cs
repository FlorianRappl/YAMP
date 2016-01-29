using System;
using System.Collections.Generic;

namespace YAMP
{
    [Description("Converts a binary number to a decimal number.")]
    [Kind(PopularKinds.Conversion)]
    sealed class Bin2DecFunction : ArgumentFunction
    {
        [Description("The function ignores white spaces and converts the given binary input to the equivalent decimal number.")]
        [Example("bin2dec(\"010111\")", "Binary 010111 converts to decimal 23.")]
        public ScalarValue Function(StringValue binarystr)
        {
            var sum = 0;
            var binary = new Stack<bool>();
            var weight = 1;

            for (var i = 1; i <= binarystr.Length; i++)
            {
                var chr = binarystr[i];

                if (ParseEngine.IsWhiteSpace(chr))
                    continue;
                else if (ParseEngine.IsNewLine(chr))
                    continue;
                else if (chr == '0')
                    binary.Push(false);
                else if (chr == '1')
                    binary.Push(true);
                else
                    throw new YAMPRuntimeException("bin2dec can only interpret binary strings.");
            }

            while (binary.Count != 0)
            {
                var el = binary.Pop();

                if (el)
                    sum += weight;

                weight *= 2;
            }

            return new ScalarValue(sum);
        }
    }
}
