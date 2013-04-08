using System;

namespace YAMP
{
    [Description("Fuses three matrices containing the reg, green, and blue values of an image separately into one single matrix which contains the information of the three matrices. It is the inverse function to ImgToRgb.")]
	[Kind(PopularKinds.Function)]
    sealed class RGBToImgFunction : ArgumentFunction
    {
        const int rfactor = 256 * 256;
        const int gfactor = 256;
        const int bfactor = 1;

        [Description("Given the red, green, and blue matrices (with values between 0 and 255) this function calculates blue + green * 256 + red * 256^2 using only the real integer part of the matrices.")]
        [Example("load(\"example.bmp\", \"bmp\"); [r, g, b] = imgtorgb(bmp); bmp2 = rgbtoimg(r, g, b)", "Reverses the transformation by the ImgToRgb function.", true)]
        public MatrixValue Function(MatrixValue R, MatrixValue G, MatrixValue B)
        {
            if (R.DimensionX != B.DimensionX || R.DimensionY != B.DimensionY)
                throw new YAMPDifferentDimensionsException(R, B);
            if (B.DimensionX != G.DimensionX || B.DimensionY != G.DimensionY)
                throw new YAMPDifferentDimensionsException(B, G);
            if (G.DimensionX != R.DimensionX || G.DimensionY != R.DimensionY)
                throw new YAMPDifferentDimensionsException(G, R);

            var rgbvalues = new MatrixValue(R.DimensionY, R.DimensionX);

            for (int i = 1; i <= R.DimensionY; i++)
            {
                for (int j = 1; j <= R.DimensionX; j++)
                {
                    rgbvalues[i, j] = new ScalarValue(rfactor * R[i, j].IntValue + gfactor * G[i, j].IntValue + bfactor * B[i, j].IntValue);
                }
            }

            return rgbvalues;
        }
    }
}
