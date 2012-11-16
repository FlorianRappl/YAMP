using System;

namespace YAMP
{
	[Kind(PopularKinds.Plot)]
	[Description("Gets or sets the value of the title of a plot.")]
    class TitleFunction : PropertyFunction<StringValue>
    {
        public TitleFunction() : base("Title")
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
