namespace YAMP.Converter
{
    using System;

    /// <summary>
    /// Matrix (only the real parts) to double array converter.
    /// </summary>
    public class MatrixToDoubleArrayConverterAttribute : ValueConverterAttribute
    {
        /// <summary>
        /// Creates a new Matrix To Double Converter.
        /// </summary>
        public MatrixToDoubleArrayConverterAttribute()
            : base(typeof(MatrixValue))
        {
            Converter = w =>
            {
                var v = w as MatrixValue;
                var m = new double[v.Length];

                for (var i = 0; i < m.Length; i++)
                {
                    m[i] = v[i + 1].Re;
                }

                return m;
            };
        }
    }
}
