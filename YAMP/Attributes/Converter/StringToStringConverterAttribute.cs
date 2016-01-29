using System;

namespace YAMP.Converter
{
    /// <summary>
    /// String to StringValue converter.
    /// </summary>
    public class StringToStringConverterAttribute : ValueConverterAttribute
    {
        /// <summary>
        /// Creates a new String To String Converter.
        /// </summary>
        public StringToStringConverterAttribute()
            : base(typeof(StringValue), v => (v as StringValue).Value)
        {
        }
    }
}
