namespace YAMP.Converter
{
    using System;

    /// <summary>
    /// String to double converter.
    /// </summary>
    public class ScalarToDoubleConverterAttribute : ValueConverterAttribute
    {
        /// <summary>
        /// Creates a new Scalar To Double Converter.
        /// </summary>
        public ScalarToDoubleConverterAttribute()
            : base(typeof(ScalarValue), v => (v as ScalarValue).Re)
        {
        }
    }
}
