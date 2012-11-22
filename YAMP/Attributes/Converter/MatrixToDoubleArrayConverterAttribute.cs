using System;

namespace YAMP.Converter
{
    public class MatrixToDoubleArrayConverterAttribute : ValueConverterAttribute
    {
        public MatrixToDoubleArrayConverterAttribute()
            : base(typeof(MatrixValue))
        {
            Converter = w =>
            {
                var v = w as MatrixValue;
                var m = new double[v.Length];

                for (var i = 0; i < m.Length; i++)
                {
                    m[i] = v[i + 1].Value;
                }

                return m;
            };
        }
    }
}
