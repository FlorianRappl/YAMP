namespace YAMP
{
    using System;
    using System.Collections.Generic;
    using YAMP.Exceptions;

    [Description("Bin2DecFunctionDescription")]
    [Kind(PopularKinds.Conversion)]
    sealed class Bin2DecFunction : ArgumentFunction
    {
        [Description("Bin2DecFunctionDescriptionForString")]
        [Example("bin2dec(\"010111\")", "Bin2DecFunctionExampleForString1")]
        public ScalarValue Function(StringValue binarystr)
        {
            var sum = 0;
            var binary = new Stack<Boolean>();
            var weight = 1;

            for (var i = 1; i <= binarystr.Length; i++)
            {
                var chr = binarystr[i];

                if (!ParseEngine.IsWhiteSpace(chr) && !ParseEngine.IsNewLine(chr))
                {
                    if (chr == '0')
                    {
                        binary.Push(false);
                    }
                    else if (chr == '1')
                    {
                        binary.Push(true);
                    }
                    else
                    {
                        throw new YAMPRuntimeException("bin2dec can only interpret binary strings.");
                    }
                }
            }

            while (binary.Count != 0)
            {
                var el = binary.Pop();

                if (el)
                {
                    sum += weight;
                }

                weight *= 2;
            }

            return new ScalarValue(sum);
        }
    }
}
