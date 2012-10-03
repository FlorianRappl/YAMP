using System;
using System.Text;
using System.Linq;
using System.Reflection;

namespace YAMP
{
    [Description("Shows detailled help for all functions.")]
    class HelpFunction : ArgumentFunction
    {
        const string SPACING = "   ";

        [Description("Shows a list of all out-of-the-box provided functions.")]
        [Example("help()", "Lists all functions.")]
        public StringValue Function()
        {
            var sb = new StringBuilder();
            var methods = Tokens.Instance.Methods.Select(m => m.GetType().Name.RemoveFunctionConvention().ToLower()).OrderBy(m => m).AsEnumerable();

            foreach (var method in methods)
                sb.AppendLine(method);

            return new StringValue(sb.ToString());
        }

        [Description("Shows detailled help for a specific function.")]
        [Example("help(\"help\")", "You already typed that in!")]
        [Example("help(\"sin\")", "Shows the detailled help for the sinus function.")]
        public StringValue Function(StringValue method)
        {
            var sb = new StringBuilder();
            var func = method.Value.ToLower();
            var element = Tokens.Instance.Methods.Where(m => m.GetType().Name.Equals(func + "Function", StringComparison.InvariantCultureIgnoreCase)).Select(m => m.GetType()).FirstOrDefault();

            if (element == null)
                throw new FunctionNotFoundException(method.Value);

            sb.AppendLine(GetDescription(element));
            var functions = element.GetMethods();

            foreach (var function in functions)
            {
                if (element.IsSubclassOf(typeof(ArgumentFunction)))
                {
                    if (function.Name.IsArgumentFunction())
                    {
                        sb.AppendLine("Usage:");
                        sb.Append(SPACING).Append(func).Append("(");
                        var length = function.GetParameters().Length;
                        var s = new string[length];

                        for (var i = 0; i < length; )
                            s[i] = "x" + (++i);

                        sb.Append(string.Join(",", s));
                        sb.AppendLine(")");
                        sb.Append(GetDescription(function));
                        sb.Append(GetMethodSignature(function));
                        sb.AppendLine(GetMethodExample(function));
                    }
                }
                else if(function.Name.Equals("Perform"))
                {
                    sb.AppendLine("Usage:");
                    sb.Append(SPACING).Append(func).AppendLine("(x)");
                    sb.Append(GetDescription(function));
                    sb.Append(GetMethodSignature(function));
                    sb.AppendLine(GetMethodExample(function));
                }
            }

            return new StringValue(sb.ToString());
        }

        string GetMethodExample(MemberInfo function)
        {
            StringBuilder sb = new StringBuilder();
            var objects = function.GetCustomAttributes(typeof(ExampleAttribute), false);
            sb.AppendLine("Example(s):");
            var index = 1;

            if (objects.Length == 0)
                sb.Append(SPACING).AppendLine("No example available.");

            foreach (ExampleAttribute attribute in objects)
            {
                sb.Append(SPACING).Append(index++).AppendLine(", example:");
                sb.Append(SPACING).Append(SPACING).AppendLine(attribute.Example);

                if (!string.IsNullOrEmpty(attribute.Description))
                {
                    sb.Append(SPACING).Append(SPACING).AppendLine("Description:");
                    sb.Append(SPACING).Append(SPACING).AppendLine(attribute.Description);
                }
            }

            return sb.ToString();
        }

        string GetMethodSignature(MethodInfo function)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Returns:");
            sb.Append(SPACING).AppendLine(ModifyValueType(function.ReturnType));
            sb.AppendLine("Argument(s):");
            var args = function.GetParameters();

            if (args.Length == 0)
                sb.Append(SPACING).AppendLine("No arguments required.");
            
            foreach (var arg in args)
                sb.Append(SPACING).AppendLine(ModifyValueType(arg.ParameterType));

            return sb.ToString();
        }

        string GetDescription(MemberInfo element)
        {
            StringBuilder sb = new StringBuilder();
            var objects = element.GetCustomAttributes(typeof(DescriptionAttribute), false);
            sb.AppendLine("Description:");

            if (objects.Length == 0)
                sb.Append(SPACING).AppendLine("No description available.");
            else
                foreach (DescriptionAttribute attribute in objects)
                    sb.Append(SPACING).AppendLine(attribute.Description);

            return sb.ToString();
        }

        string ModifyValueType(Type type)
        {
            return type.Name.RemoveValueConvention();
        }
    }
}
