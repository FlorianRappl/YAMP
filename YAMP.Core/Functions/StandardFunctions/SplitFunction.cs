using System;
using System.Linq;

namespace YAMP
{
	[Description("Splits a string into tokens using a list of separators.")]
	[Kind(PopularKinds.Function)]
    sealed class SplitFunction : ArgumentFunction
	{
        [Description("Splits a string into tokens using a list of separators.")]
		[Example("tokens = split(\"hello world!\", \"o\", \"e\")", "Returns a list of all tokens.")]
		[Arguments(1, 0)]
        public ArgumentsValue Function(StringValue text, ArgumentsValue args)
		{
            var separators = args.Select(v => v.ToString()).ToArray();
            var tokens = text.ToString().Split(separators, StringSplitOptions.RemoveEmptyEntries);
            return new ArgumentsValue(tokens.Select(s => new StringValue(s)).ToArray());
		}
	}
}
