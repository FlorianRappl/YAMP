using System;

namespace YAMP
{
	[Kind(PopularKinds.Plot)]
    class YLabelFunction : PropertyFunction<StringValue>
    {
        public YLabelFunction(): base("YLabel")
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
