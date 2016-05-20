namespace YAMP
{
    using System;
    using System.Linq;

    [Description("SplitFunctionDescription")]
	[Kind(PopularKinds.Function)]
    sealed class SplitFunction : ArgumentFunction
	{
        [Description("SplitFunctionDescriptionForStringArguments")]
		[Example("tokens = split(\"hello world!\", \"o\", \"e\")", "SplitFunctionExampleForStringArguments1")]
		[Arguments(1, 0)]
        public ArgumentsValue Function(StringValue text, ArgumentsValue args)
		{
            var separators = args.Select(v => v.ToString()).ToArray();
            var tokens = text.ToString().Split(separators, StringSplitOptions.RemoveEmptyEntries);
            return new ArgumentsValue(tokens.Select(s => new StringValue(s)).ToArray());
		}
	}
}
