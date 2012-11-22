using System;

namespace YAMP.Converter
{
    public class ScalarToBooleanConverterAttribute : ValueConverterAttribute
    {
        public ScalarToBooleanConverterAttribute()
            : base(typeof(ScalarValue), v => (v as ScalarValue).IntValue == 1.0)
        {
        }
    }
}
