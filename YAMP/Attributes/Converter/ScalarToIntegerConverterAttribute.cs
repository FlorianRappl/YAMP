using System;

namespace YAMP.Converter
{
    /// <summary>
    /// String to integer converter.
    /// </summary>
    public class ScalarToIntegerConverterAttribute : ValueConverterAttribute
    {
        /// <summary>
        /// Creates a new Scalar To Int32 Converter.
        /// </summary>
        public ScalarToIntegerConverterAttribute()
            : base(typeof(ScalarValue), v => (v as ScalarValue).IntValue)
        {
        }
    }
}
