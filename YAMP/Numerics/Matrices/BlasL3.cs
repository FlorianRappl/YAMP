using System;
using YAMP;

namespace YAMP.Numerics
{
    /// <summary>
    /// Blas Level 3 Matrix Matrix multiplication.
    /// </summary>
    public class BlasL3
    {
        /// <summary>
        /// Performs a matrix times matrix operation C = A * B with real matrices.
        /// </summary>
        /// <param name="aStore">The 1-dimensional array for the matrix A.</param>
        /// <param name="aOffset">The offset in the array for A.</param>
        /// <param name="aRowStride">The difference for skipping one row.</param>
        /// <param name="aColStride">The difference for skipping one column.</param>
        /// <param name="bStore">The 1-dimensional array for the matrix B.</param>
        /// <param name="bOffset">The offset in the array for B.</param>
        /// <param name="bRowStride">The difference for skipping one row.</param>
        /// <param name="bColStride">The difference for skipping one column.</param>
        /// <param name="cStore">The 1-dimensional array for the matrix C.</param>
        /// <param name="cOffset">The offset in the array for C.</param>
        /// <param name="cRowStride">The difference for skipping one row.</param>
        /// <param name="cColStride">The difference for skipping one column.</param>
        /// <param name="rowsA">The rows to handle in the matrix A.</param>
        /// <param name="colsB">The coluumns to handle in the matrix B.</param>
        /// <param name="length">The columns of A / rows of B - or length for the multiplication.</param>
        public static void dGemm(double[] aStore, int aOffset, int aRowStride, int aColStride,
                                double[] bStore, int bOffset, int bRowStride, int bColStride,
                                double[] cStore, int cOffset, int cRowStride, int cColStride,
                                int rowsA, int colsB, int length)
        {
            int aIndex = aOffset;
            int cStart = cOffset;

            for (int n = 0; n < rowsA; n++)
            {
                int bIndex = bOffset;
                int cIndex = cStart;

                for (int m = 0; m < colsB; m++)
                {
                    cStore[cIndex] = BlasL1.dDot(aStore, aIndex, aColStride, bStore, bIndex, bRowStride, length);
                    bIndex += bColStride;
                    cIndex += cColStride;
                }

                aIndex += aRowStride;
                cStart += cRowStride;
            }
        }

        /// <summary>
        /// Performs a matrix times matrix operation C = A * B with complex matrices.
        /// </summary>
        /// <param name="aStore">The 1-dimensional complex array for the matrix A.</param>
        /// <param name="aOffset">The offset in the array for A.</param>
        /// <param name="aRowStride">The difference for skipping one row.</param>
        /// <param name="aColStride">The difference for skipping one column.</param>
        /// <param name="bStore">The 1-dimensional complex array for the matrix B.</param>
        /// <param name="bOffset">The offset in the array for B.</param>
        /// <param name="bRowStride">The difference for skipping one row.</param>
        /// <param name="bColStride">The difference for skipping one column.</param>
        /// <param name="cStore">The 1-dimensional complex array for the matrix C.</param>
        /// <param name="cOffset">The offset in the array for C.</param>
        /// <param name="cRowStride">The difference for skipping one row.</param>
        /// <param name="cColStride">The difference for skipping one column.</param>
        /// <param name="rowsA">The rows to handle in the matrix A.</param>
        /// <param name="colsB">The coluumns to handle in the matrix B.</param>
        /// <param name="length">The columns of A / rows of B - or length for the multiplication.</param>
        public static void cGemm(ScalarValue[] aStore, int aOffset, int aRowStride, int aColStride,
                                ScalarValue[] bStore, int bOffset, int bRowStride, int bColStride,
                                ScalarValue[] cStore, int cOffset, int cRowStride, int cColStride,
                                int rowsA, int colsB, int length)
        {
            int aIndex = aOffset;
            int cStart = cOffset;

            for (int n = 0; n < rowsA; n++)
            {
                int bIndex = bOffset;
                int cIndex = cStart;

                for (int m = 0; m < colsB; m++)
                {
                    cStore[cIndex] = BlasL1.cDot(aStore, aIndex, aColStride, bStore, bIndex, bRowStride, length);
                    bIndex += bColStride;
                    cIndex += cColStride;
                }

                aIndex += aRowStride;
                cStart += cRowStride;
            }
        }
    }
}
