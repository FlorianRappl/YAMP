using System;

namespace YAMP
{
	[Description("Formats some text using placeholders like {0} etc. and returns the formatted string.")]
	[Kind(PopularKinds.System)]
    sealed class PrintfFunction : SystemFunction
	{
		[Description("Formats some text using placeholders like {0} etc. and returns the resulting string.")]
		[Example("s = printf(\"{2} The result of {0} + {1} is {3}\", 2.0, 7i, \"Hi!\", 2.0 + 7i)", "Evaluates the arguments and includes them in the string given by the first argument.")]
		[Arguments(1, 0)]
		public StringValue Function(StringValue text, ArgumentsValue args)
		{
			return new StringValue(string.Format(text.Value, args.ToArray()));
		}
	}
}
