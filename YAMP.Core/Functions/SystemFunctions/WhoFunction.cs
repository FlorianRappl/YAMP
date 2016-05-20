namespace YAMP
{
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using YAMP.Exceptions;

	[Description("WhoFunctionDescription")]
	[Kind(PopularKinds.System)]
    sealed class WhoFunction : SystemFunction
    {
        public WhoFunction(ParseContext context)
            : base(context)
        {
        }

        [Description("WhoFunctionDescriptionForVoid")]
        [Example("who()")]
        public StringValue Function()
        {
            var sb = new StringBuilder();
            var variables = Context.AllVariables.Keys.OrderBy(m => m).AsEnumerable();

            foreach (var variable in variables)
            {
                sb.AppendLine(variable);
            }

            return new StringValue(sb.ToString());
        }

        [Description("WhoFunctionDescriptionForString")]
        [Example("who(\"a*\")", "WhoFunctionExampleForString1")]
        [Example("who(\"x?b\")", "WhoFunctionExampleForString2")]
        public StringValue Function(StringValue filter)
        {
            var regex = new Regex("^" + Regex.Escape(filter.Value).Replace("\\*", ".*").Replace("\\?", ".{1}") + "$");
            var sb = new StringBuilder();
            var variables = Context.AllVariables.Keys.Where(m => regex.IsMatch(m)).OrderBy(m => m).AsEnumerable();

            foreach (var variable in variables)
            {
                sb.AppendLine(variable);
            }

            return new StringValue(sb.ToString());
        }

		[Description("WhoFunctionDescriptionForArguments")]
		[Example("who(\"a*\", \"x\")", "WhoFunctionExampleForArguments1")]
		[Example("who(\"x?b\", \"a\", \"b\")", "WhoFunctionExampleForArguments2")]
		[Arguments(0, 2)]
		public StringValue Function(ArgumentsValue filter)
		{
			var values = filter.Values;
			var index = 0;
			var sb = new StringBuilder();

            foreach (var value in values)
            {
                index++;

                if (value is StringValue)
                {
                    var str = value as StringValue;
                    sb.Append(Function(str));
                }
                else
                {
                    throw new YAMPArgumentWrongTypeException(value.Header, "String", Name);
                }
            }

			return new StringValue(sb.ToString());
		}
    }
}
