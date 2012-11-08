using System;

namespace YAMP
{
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
