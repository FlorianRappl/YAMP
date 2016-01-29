using System;
using YAMP.Numerics;

namespace YAMP
{
	[Description("Provides a fast-fourier-transform function for 2^n (complex) values.")]
	[Kind(PopularKinds.Function)]
    [Link("http://en.wikipedia.org/wiki/Fast_Fourier_transform")]
    sealed class FFTFunction : ArgumentFunction
	{
		[Description("Fourier transforms a matrix of elements.")]
		[Example("fft([0,1,0,5])", "Uses FFT on the vector [0,1,0,5].")]
		public MatrixValue Function(MatrixValue M)
		{
            bool transpose = false;

            if (M.DimensionX > M.DimensionY)
            {
                transpose = true;
                M = M.Transpose();
            }

            var fft = new Fourier(M.DimensionY);

            for (var i = 1; i <= M.DimensionX; i++)
            {
                var values = new ScalarValue[M.DimensionY];

                for (var j = 1; j <= M.DimensionY; j++)
                    values[j - 1] = M[j, i];

                values = fft.Transform(values);

                for (var j = 1; j <= M.DimensionY; j++)
                    M[j, i] = values[j - 1];
            }

            if (transpose)
                return M.Transpose();

            return M;
		}
	}
}
