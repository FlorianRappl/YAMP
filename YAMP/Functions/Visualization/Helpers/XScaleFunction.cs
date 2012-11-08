using System;

namespace YAMP
{
    class XScaleFunction : PropertyFunction<ScalarValue>
    {
        public XScaleFunction() : base("IsLogX")
        {
        }

        protected override object GetValue(ScalarValue parameter)
        {
            return parameter.Value == 1.0;
        }

        protected override ScalarValue GetValue(object original)
        {
            return new ScalarValue((bool)original);
        }
    }
}
