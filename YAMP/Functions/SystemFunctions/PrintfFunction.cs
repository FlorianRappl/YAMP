using System;

namespace YAMP
{
    class PrintfFunction : ArgumentFunction
    {
        public PrintfFunction() : base(2)
        {
        }

        public StringValue Function(StringValue formatString, ArgumentsValue args)
        {
            var content = string.Format(formatString.Value, args.ToArray());
            return new StringValue(content);
        }
    }
}
