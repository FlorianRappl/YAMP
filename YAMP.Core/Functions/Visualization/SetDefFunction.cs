namespace YAMP
{
    using System.Text;
    using YAMP.Exceptions;

    [Description("SetDefFunctionDescription")]
    [Kind(PopularKinds.Plot)]
    sealed class SetDefFunction : VisualizationFunction
    {
        public SetDefFunction(ParseContext context)
            : base(context)
        {
        }

        [Description("SetDefFunctionDescriptionForStringValue")]
        [Example("setdef(\"title\", \"My default title\")", "SetDefFunctionExampleForStringValue1")]
        public void Function(StringValue property, Value value)
        {
            Context.SetDefaultProperty("plot", property.Value, value);
        }

        [Description("SetDefFunctionDescriptionForString")]
        [Example("setdef(\"plot\")", "SetDefFunctionExampleForString1")]
        public StringValue Function(StringValue type)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Defined for any plot:");
            var plotSettings = Context.GetDefaultProperties("plot");

            foreach (var pair in plotSettings)
            {
                sb.Append("-\t").Append("Name: ");
                sb.AppendLine(pair.Key);
                sb.Append("\t").Append("Value: ");
                sb.AppendLine(pair.Value.ToString());
            }

            sb.AppendLine("Defined for any series:");
            var seriesSettings = Context.GetDefaultProperties("series");

            foreach (var pair in seriesSettings)
            {
                sb.Append("-\t").Append("Name: ");
                sb.AppendLine(pair.Key);
                sb.Append("\t").Append("Value: ");
                sb.AppendLine(pair.Value.ToString());
            }

            return new StringValue(sb.ToString());
        }

        [Description("SetDefFunctionDescriptionForStringStringValue")]
        [Example("setdef(\"plot\", \"title\", \"My def title\")", "SetDefFunctionExampleForStringStringValue1")]
        [Example("setdef(\"series\", \"symbol\", \"none\")", "SetDefFunctionExampleForStringStringValue2")]
        [Example("setdef(\"series\", \"lines\", \"on\")", "SetDefFunctionExampleForStringStringValue3")]
        public void Function(StringValue type, StringValue property, Value value)
        {
            switch(type.Value.ToLower())
            {
                case "plot":
                    Function(property, value);
                    break;
                case "series":
                    Context.SetDefaultProperty("series", property.Value, value);
                    break;
                default:
                    throw new YAMPPropertyMissingException(type.Value, new[] { "Plot", "Series" });
            }
        }
    }
}
