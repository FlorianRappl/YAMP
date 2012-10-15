using System;

namespace YAMP
{
    class TitleFunction : PropertyFunction<StringValue>
    {
        public TitleFunction() : base("Title")
        {
        }

        protected override object GetValue(StringValue parameter)
        {
            return parameter.Value;
        }
    }
}
