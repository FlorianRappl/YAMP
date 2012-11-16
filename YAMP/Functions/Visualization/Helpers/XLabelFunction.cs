using System;

namespace YAMP
{
	[Kind(PopularKinds.Plot)]
	[Description("Gets or sets the value for the xlabel (title of the x-axis) of a plot.")]
    class XLabelFunction : PropertyFunction<StringValue>
    {
        public XLabelFunction() : base("XLabel")
        {
        }

        protected override object GetValue(StringValue parameter)
        {
            return parameter.Value;
        }

        protected override StringValue GetValue(object original)
        {
            return new StringValue(original.ToString());
        }
    }
}
