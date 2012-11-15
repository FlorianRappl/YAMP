using System;

namespace YAMP
{
	[Kind(PopularKinds.Plot)]
    class YScaleFunction : PropertyFunction<ScalarValue>
    {
        public YScaleFunction() : base("IsLogY")
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
