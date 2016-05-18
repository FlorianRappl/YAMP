namespace YAMP
{
    using System;
    using YAMP.Exceptions;

    [Description("RGBToImgFunctionDescription")]
	[Kind(PopularKinds.Function)]
    sealed class RGBToImgFunction : ArgumentFunction
    {
        const Int32 rfactor = 256 * 256;
        const Int32 gfactor = 256;
        const Int32 bfactor = 1;

        [Description("RGBToImgFunctionDescriptionForMatrixMatrixMatrix")]
        [Example("load(\"example.bmp\", \"bmp\"); [r, g, b] = imgtorgb(bmp); bmp2 = rgbtoimg(r, g, b)", "RGBToImgFunctionExampleForMatrixMatrixMatrix1", true)]
        public MatrixValue Function(MatrixValue R, MatrixValue G, MatrixValue B)
        {
            if (R.DimensionX != B.DimensionX || R.DimensionY != B.DimensionY)
                throw new YAMPDifferentDimensionsException(R, B);

            if (B.DimensionX != G.DimensionX || B.DimensionY != G.DimensionY)
                throw new YAMPDifferentDimensionsException(B, G);

            if (G.DimensionX != R.DimensionX || G.DimensionY != R.DimensionY)
                throw new YAMPDifferentDimensionsException(G, R);

            var rgbvalues = new MatrixValue(R.DimensionY, R.DimensionX);

            for (var i = 1; i <= R.DimensionY; i++)
            {
                for (var j = 1; j <= R.DimensionX; j++)
                {
                    rgbvalues[i, j] = new ScalarValue(rfactor * R[i, j].IntValue + gfactor * G[i, j].IntValue + bfactor * B[i, j].IntValue);
                }
            }

            return rgbvalues;
        }
    }
}
