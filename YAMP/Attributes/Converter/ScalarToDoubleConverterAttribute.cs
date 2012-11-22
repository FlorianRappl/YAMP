using System;

namespace YAMP.Converter
{
    public class ScalarToDoubleConverterAttribute : ValueConverterAttribute
    {
        public ScalarToDoubleConverterAttribute()
            : base(typeof(ScalarValue), v => (v as ScalarValue).Value)
        {
        }
    }
}
