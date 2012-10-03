using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace YAMP
{
    [Description("Lists the available variables.")]
    class WhoFunction : ArgumentFunction
    {
        [Description("Lists all variables from the current global workspace.")]
        [Example("who()")]
        public StringValue Function()
        {
            var sb = new StringBuilder();
            var variables = Tokens.Instance.Variables.Keys.OrderBy(m => m).AsEnumerable();

            foreach (var variable in variables)
                sb.AppendLine(variable);

            return new StringValue(sb.ToString());
        }

        [Description("Lists variables from the current global workspace using a filter.")]
        [Example("who(\"a*\")", "Lists all variables, which start with a small 'a'.")]
        [Example("who(\"x?b\")", "Lists all variables, which contain 3 letters, starting with a small x, ending with a small b and any letter in between.")]
        public StringValue Function(StringValue filter)
        {
            var regex = new Regex("^" + Regex.Escape(filter.Value).Replace("\\*", ".*").Replace("\\?", ".{1}") + "$");
            var sb = new StringBuilder();
            var variables = Tokens.Instance.Variables.Keys.Where(m => regex.IsMatch(m)).OrderBy(m => m).AsEnumerable();

            foreach (var variable in variables)
                sb.AppendLine(variable);

            return new StringValue(sb.ToString());
        }
    }
}
