using System;
using YAMP.Numerics;

namespace YAMP
{
	[Description("Provides a fast-fourier-transform function for 2^n (complex) values.")]
	[Kind(PopularKinds.Function)]
	class FFTFunction : ArgumentFunction
	{
		[Description("Fourier transforms a matrix of elements.")]
		[Example("fft([0,1,0,5])", "Uses FFT on the vector [0,1,0,5]")]
		public MatrixValue Function(MatrixValue argument)
		{
			var m = argument as MatrixValue;
			var fft = new FFT(m);

			if (m.DimensionX == 1 || m.DimensionY == 1)
				return fft.Transform1D();

			return fft.Transform2D();
		}
	}
}
