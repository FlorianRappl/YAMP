using System;
using System.Text;

namespace YAMP
{
    [Description("The setdef function can be used to define template values for created plots. Every plot that is created after a certain template property has been set will be assigned the value of the property.")]
    [Kind(PopularKinds.Plot)]
    sealed class SetDefFunction : VisualizationFunction
    {
        [Description("Sets the template value of the property to a certain value. This function assumes that the category is plot, i.e. general plot settings like title or gridlines.")]
        [Example("setdef(\"title\", \"My default title\")", "Sets the title of each new plot to a default value of My default title.")]
        public void Function(StringValue property, Value value)
        {
            Context.SetDefaultProperty("plot", property.Value, value);
        }

        [Description("Gets the default values that have been set for the given type.")]
        [Example("setdef(\"plot\")", "Gets all the default property names with values that have been set for the category plot.")]
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

        [Description("Sets the template value of the name type and property to a certain value. This function does not assume that the category is plot, such that you can enter series as well. The type series will set the template values for all new series.")]
        [Example("setdef(\"plot\", \"title\", \"My def title\")", "Sets the title of each new plot to a default value of My def title.")]
        [Example("setdef(\"series\", \"symbol\", \"none\")", "Does not show any symbols for every series of every new plot.")]
        [Example("setdef(\"series\", \"lines\", \"on\")", "Does show lines for every series of every new plot.")]
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
