using System;
using YAMP;

namespace YAMP.Numerics
{
    /// <summary>
    /// A simple FFT implemention that uses Cooley-Tukey FFT (i.e. 2^n elements required).
    /// </summary>
    public class FFT
    {
        #region Members

        MatrixValue _values;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="values">The data.</param>
        public FFT(MatrixValue values)
        {
            _values = values;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Transforms the data as 1D.
        /// </summary>
        /// <returns>The transformed values.</returns>
        public MatrixValue Transform1D()
        {
            return ifft(_values);
        }

        /// <summary>
        /// Transforms the data as 2D.
        /// </summary>
        /// <returns>The transformed values.</returns>
        public MatrixValue Transform2D()
        {
            return ifft2d(_values);
        }

        #endregion

        #region Calculations

        MatrixValue fft(MatrixValue x)
        {
            var length = x.Length;

            if (length == 1) 
                return x.Clone();

            // Cooley-Tukey FFT
            if (length % 2 != 0) 
                throw new YAMPDifferentLengthsException(length, "2^n");

            // even fft
            var even = new MatrixValue(length / 2, 1);

            for (int k = 1; k <= even.Length; k++)
                even[k] = x[2 * k];
            
            var q = fft(even);

            // odd fft;
            var odd = even; 

            for (int k = 1; k <= odd.Length; k++)
                odd[k] = x[2 * k - 1];

            var r = fft(odd);

            // combine
            var y = new MatrixValue(length, 1);

            for (int k = 1; k <= odd.Length; k++)
            {
                var value = -2 * (k - 1) * Math.PI / length;
                var wk = new ScalarValue(Math.Cos(value), Math.Sin(value));
                y[k] = q[k] + (wk * r[k]);
                y[k + odd.Length] = q[k] - (wk * r[k]);
            }

            return y;
        }

        MatrixValue ifft(MatrixValue x)
        {
            var length = x.Length;

            // Cooley-Tukey FFT
            if (length % 2 != 0)
                throw new YAMPDifferentLengthsException(length, "2^n");

            var y = new MatrixValue(length, 1);

            //conjugate
            for (int i = 1; i <= length; i++)
                y[i] = x[i].Conjugate();

            // compute forward FFT
            y = fft(y);

            // take conjugate again and divide by N
            for (int i = 1; i <= length; i++)
                y[i] = y[i].Conjugate() / (double)length;

            return y;
        }

        MatrixValue fft2d(MatrixValue input)
        {
            var output = input.Clone();

            // Rows first:
            var x = new MatrixValue(output.DimensionY, 1);

            for (int h = 1; h <= output.DimensionX; h++)
            {
                for (int i = 1; i <= output.DimensionY; i++)
                    x[i] = output[i, h];
                
                x = fft(x);

                for (int i = 1; i <= output.DimensionY; i++)
                    output[i, h] = x[i];
            }

            //Columns last
            var y = new MatrixValue(output.DimensionX, 1);

            for (int h = 0; h < output.DimensionY; h++)
            {
                for (int i = 1; i <= output.DimensionX; i++)
                    y[i] = output[h, i];

                y = fft(y);

                for (int i = 1; i <= output.DimensionX; i++)
                    output[h, i] = y[i];
            }

            return output;
        }

        MatrixValue ifft2d(MatrixValue input)
        {
            var output = input.Clone();

            // Rows first:
            var x = new MatrixValue(output.DimensionY, 1);

            for (int h = 1; h <= output.DimensionX; h++)
            {
                for (int i = 1; i <= output.DimensionY; i++)
                    x[i] = output[i, h];
                
                x = ifft(x);

                for (int i = 1; i <= output.DimensionY; i++)
                    output[i, h] = x[i];
            }

            //Columns last
            var y = new MatrixValue(output.DimensionX, 1);

            for (int h = 1; h <= output.DimensionY; h++)
            {
                for (int i = 1; i <= output.DimensionX; i++)
                    y[i] = output[h, i];
                
                y = ifft(y);

                for (int i = 1; i <= output.DimensionX; i++)
                    output[h, i] = y[i];
            }

            return output;
        }

        #endregion
    }
}
