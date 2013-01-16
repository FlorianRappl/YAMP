using System;

namespace YAMP
{
    [Description("Converts matrix data loaded from an image into three matrices which contain the red, green, and blue values separately. It is the inverse function to RGBToImageData.")]
	[Kind(PopularKinds.Function)]
    class ImageDataToRGBFunction : ArgumentFunction
    {
        const int rfactor = 256 * 256;
        const int gfactor = 256;
        const int bfactor = 1;

        [Description("Gets the RGB values of an image matrix.")]
        [Example("load(\"example.bmp\", \"bmp\"); [r, g, b] = imagedatatorgb(bmp)", "Returns three matrices containing the red, green, and blue values of the image.")]
        public ArgumentsValue Function(MatrixValue M)
        {
            var rvalues = new MatrixValue(M.DimensionY, M.DimensionX);
            var gvalues = new MatrixValue(M.DimensionY, M.DimensionX);
            var bvalues = new MatrixValue(M.DimensionY, M.DimensionX);

            for (int i = 1; i <= M.DimensionY; i++)
            {
                for (int j = 1; j <= M.DimensionX; j++)
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
