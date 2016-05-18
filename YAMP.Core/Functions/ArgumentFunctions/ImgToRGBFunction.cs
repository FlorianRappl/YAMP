namespace YAMP
{
    [Description("ImgToRGBFunctionDescription")]
	[Kind(PopularKinds.Function)]
    sealed class ImgToRGBFunction : ArgumentFunction
    {
        const int rfactor = 256 * 256;
        const int gfactor = 256;
        const int bfactor = 1;

        [Description("ImgToRGBFunctionDescriptionForMatrix")]
        [Example("load(\"example.bmp\", \"bmp\"); [r, g, b] = imgtorgb(bmp)", "ImgToRGBFunctionExampleForMatrix1", true)]
        public ArgumentsValue Function(MatrixValue M)
        {
            var rvalues = new MatrixValue(M.DimensionY, M.DimensionX);
            var gvalues = new MatrixValue(M.DimensionY, M.DimensionX);
            var bvalues = new MatrixValue(M.DimensionY, M.DimensionX);

            for (var i = 1; i <= M.DimensionY; i++)
            {
                for (var j = 1; j <= M.DimensionX; j++)
                {
                    int value = M[i, j].IntValue;

                    rvalues[i, j] = new ScalarValue((value / rfactor) % 256);
                    gvalues[i, j] = new ScalarValue((value / gfactor) % 256);
                    bvalues[i, j] = new ScalarValue((value / bfactor) % 256);
                }
            }

            return new ArgumentsValue(rvalues, gvalues, bvalues);
        }
    }
}
