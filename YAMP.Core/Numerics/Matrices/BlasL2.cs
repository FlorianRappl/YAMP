using System;
using YAMP;

namespace YAMP.Numerics
{
    /// <summary>
    /// Provides BLAS Level 2 Access, this level contains matrix-vector operations of the form y = A * x + y.
    /// </summary>
    public class BlasL2
    {
        /// <summary>
        /// y = A * x + y, where x, y are vectors and A is a matrix.
        /// </summary>
        /// <param name="aStore">1 dim double array for A.</param>
        /// <param name="aOffset">Offset in the array for A.</param>
        /// <param name="aRowStride">Rows in A.</param>
        /// <param name="aColStride">Columns in A.</param>
        /// <param name="xStore">1 dim double array for x.</param>
        /// <param name="xOffset">Offset in the array for x.</param>
        /// <param name="xStride">Number of entries in x.</param>
        /// <param name="yStore">1 dim double array for y.</param>
        /// <param name="yOffset">Offset in the array for y.</param>
        /// <param name="yStride">Number of entries in y.</param>
        /// <param name="rows">Geometry information for the rows.</param>
        /// <param name="cols">Geometry information for the columns.</param>
        public static void dGemv(
            double[] aStore, int aOffset, int aRowStride, int aColStride,
            double[] xStore, int xOffset, int xStride,
            double[] yStore, int yOffset, int yStride,
            int rows, int cols
        )
        {
            int aIndex = aOffset;
            int yIndex = yOffset;

            for (int n = 0; n < rows; n++)
            {
                yStore[yIndex] += BlasL1.dDot(aStore, aIndex, aColStride, xStore, xOffset, xStride, cols);
                aIndex += aRowStride;
                yIndex += yStride;
            }
        }

        /// <summary>
        /// y = A * x + y, where x, y are vectors and A is a matrix.
        /// </summary>
        /// <param name="aStore">1 dim double array for A.</param>
        /// <param name="aOffset">Offset in the array for A.</param>
        /// <param name="aRowStride">Rows in A.</param>
        /// <param name="aColStride">Columns in A.</param>
        /// <param name="xStore">1 dim double array for x.</param>
        /// <param name="xOffset">Offset in the array for x.</param>
        /// <param name="xStride">Number of entries in x.</param>
        /// <param name="yStore">1 dim double array for y.</param>
        /// <param name="yOffset">Offset in the array for y.</param>
        /// <param name="yStride">Number of entries in y.</param>
        /// <param name="rows">Geometry information for the rows.</param>
        /// <param name="cols">Geometry information for the columns.</param>
        public static void cGemv(
            ScalarValue[] aStore, int aOffset, int aRowStride, int aColStride,
            ScalarValue[] xStore, int xOffset, int xStride,
            ScalarValue[] yStore, int yOffset, int yStride,
            int rows, int cols
        )
        {
            int aIndex = aOffset;
            int yIndex = yOffset;

            for (int n = 0; n < rows; n++)
            {
                yStore[yIndex] += BlasL1.cDot(aStore, aIndex, aColStride, xStore, xOffset, xStride, cols);
                aIndex += aRowStride;
                yIndex += yStride;
            }
        }
    }
}
