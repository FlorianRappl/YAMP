using System;

namespace YAMP.Converter
{
    /// <summary>
    /// scalar to boolean (1.0, rest) converter.
    /// </summary>
    public class ScalarToBooleanConverterAttribute : ValueConverterAttribute
    {
        /// <summary>
        /// Creates a new Scalar To Bool Converter.
        /// </summary>
        public ScalarToBooleanConverterAttribute()
            : base(typeof(ScalarValue), v => (v as ScalarValue).IntValue == 1.0)
        {
        }
    }
}
