namespace YAMP
{
    using System;

	[Description("PrintfFunctionDescription")]
	[Kind(PopularKinds.System)]
    sealed class PrintfFunction : SystemFunction
	{
        public PrintfFunction(ParseContext context)
            : base(context)
        {
        }

		[Description("PrintfFunctionDescriptionForStringArguments")]
		[Example("s = printf(\"{2} The result of {0} + {1} is {3}\", 2.0, 7i, \"Hi!\", 2.0 + 7i)", "PrintfFunctionExampleForStringArguments1")]
		[Arguments(1, 0)]
		public StringValue Function(StringValue text, ArgumentsValue args)
		{
			return new StringValue(String.Format(text.Value, args.ToArray()));
		}
	}
}
