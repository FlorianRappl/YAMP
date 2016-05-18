namespace YAMP
{
    using YAMP.Numerics;

    [Description("FFTFunctionDescription")]
	[Kind(PopularKinds.Function)]
    [Link("FFTFunctionLink")]
    sealed class FFTFunction : ArgumentFunction
	{
		[Description("FFTFunctionDescriptionForMatrix")]
		[Example("fft([0,1,0,5])", "FFTFunctionExampleForMatrix1")]
		public MatrixValue Function(MatrixValue M)
		{
            var transpose = false;

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
                {
                    values[j - 1] = M[j, i];
                }

                values = fft.Transform(values);

                for (var j = 1; j <= M.DimensionY; j++)
                {
                    M[j, i] = values[j - 1];
                }
            }

            if (transpose)
            {
                return M.Transpose();
            }

            return M;
		}
	}
}
