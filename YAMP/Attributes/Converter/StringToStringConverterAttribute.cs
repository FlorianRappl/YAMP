using System;

namespace YAMP.Converter
{
    public class StringToStringConverterAttribute : ValueConverterAttribute
    {
        public StringToStringConverterAttribute()
            : base(typeof(StringValue), v => (v as StringValue).Value)
        {
        }
    }
}
