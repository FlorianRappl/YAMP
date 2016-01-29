using System;
using YAMP;

namespace YAMP.Numerics
{
    /// <summary>
    /// Provides BLAS Level 1 Access, this level contains vector operations of the form y = a * x + y.
    /// </summary>
    public class BlasL1
    {
        /// <summary>
        /// Copies some x vector to some other y vector considering the given offsets.
        /// </summary>
        /// <param name="xStore">The source vector.</param>
        /// <param name="xOffset">Offset in the source.</param>
        /// <param name="xStride">The offset between two elements in the source.</param>
        /// <param name="yStore">The target vector.</param>
        /// <param name="yOffset">Offset in the target.</param>
        /// <param name="yStride">The offset between two elements in the target.</param>
        /// <param name="count">Number of elements to copy.</param>
        public static void dCopy(double[] xStore, int xOffset, int xStride, double[] yStore, int yOffset, int yStride, int count)
        {
            if ((xStride == 1) && (yStride == 1))
                Array.Copy(xStore, xOffset, yStore, yOffset, count);
            else
            {
                int n = 0;
                int x = xOffset;
                int y = yOffset;

                while (n < count)
                {
                    yStore[y] = xStore[x];
                    n++;
                    x += xStride;
                    y += yStride;
                }
            }
        }

        /// <summary>
        /// Swaps the elements of some vector x and some vector y.
        /// </summary>
        /// <param name="xStore">The first vector.</param>
        /// <param name="xOffset">Offset in the first vector.</param>
        /// <param name="xStride">The offset between two elements of the first vector.</param>
        /// <param name="yStore">The second vector.</param>
        /// <param name="yOffset">Offset in the second vector.</param>
        /// <param name="yStride">The offset between two elements of the second vector.</param>
        /// <param name="count">Number of elements to swap.</param>
        public static void dSwap(double[] xStore, int xOffset, int xStride, double[] yStore, int yOffset, int yStride, int count)
        {
            int n = 0;
            int x = xOffset;
            int y = yOffset;

            while (n < count)
            {
                double t = xStore[x];
                xStore[x] = yStore[y];
                yStore[y] = t;
                n++;
                x += xStride;
                y += yStride;
            }
        }

        /// <summary>
        /// Returns the sum_i |x_i| norm.
        /// </summary>
        /// <param name="store">The source vector.</param>
        /// <param name="offset">The offset in the source.</param>
        /// <param name="stride">The offset between two elements.</param>
        /// <param name="count">The number of elements to consider.</param>
        /// <returns>The L1 norm.</returns>
        public static double dNrm1(double[] store, int offset, int stride, int count)
        {
            double m = 0.0;
            int n = 0;
            int i = offset;

            while (n < count)
            {
                m += Math.Abs(store[i]);
                n++;
                i += stride;
            }

            return m;
        }

        /// <summary>
        /// Returns |x| = sqrt( sum_i x_i^2 ) norm.
        /// </summary>
        /// <param name="store">The source vector.</param>
        /// <param name="offset">The offset in the source.</param>
        /// <param name="stride">The offset between two elements.</param>
        /// <param name="count">The number of elements to consider.</param>
        /// <returns>The L2 norm.</returns>
        public static double dNrm2(double[] store, int offset, int stride, int count)
        {
            double m = 0.0;
            int n = 0;
            int i = offset;

            while (n < count)
            {
                double x = store[i];
                m += x * x;
                n++;
                i += stride;
            }

            return Math.Sqrt(m);
        }

        /// <summary>
        /// Returns the dot product (a, b).
        /// </summary>
        /// <param name="aStore">The first vector a.</param>
        /// <param name="aOffset">Offset in the vector a.</param>
        /// <param name="aStride">The offset between two elements of the vector a.</param>
        /// <param name="bStore">The second vector b.</param>
        /// <param name="bOffset">Offset in the vector a.</param>
        /// <param name="bStride">The offset between two elements of the vector a.</param>
        /// <param name="count">The number of elements to consider.</param>
        /// <returns>The result of the dot product.</returns>
        public static double dDot(double[] aStore, int aOffset, int aStride, double[] bStore, int bOffset, int bStride, int count)
        {
            double m = 0.0;
            int n = 0;
            int a = aOffset;
            int b = bOffset;

            while (n < count)
            {
                m += aStore[a] * bStore[b];
                n++;
                a += aStride;
                b += bStride;
            }

            return m;
        }

        /// <summary>
        /// Returns the complex dot product (a, b).
        /// </summary>
        /// <param name="aStore">The first vector a.</param>
        /// <param name="aOffset">Offset in the vector a.</param>
        /// <param name="aStride">The offset between two elements of the vector a.</param>
        /// <param name="bStore">The second vector b.</param>
        /// <param name="bOffset">Offset in the vector a.</param>
        /// <param name="bStride">The offset between two elements of the vector a.</param>
        /// <param name="count">The number of elements to consider.</param>
        /// <returns>The result of the complex dot product.</returns>
        public static ScalarValue cDot(ScalarValue[] aStore, int aOffset, int aStride, ScalarValue[] bStore, int bOffset, int bStride, int count)
        {
            double re = 0.0;
            double im = 0.0;
            int n = 0;
            int a = aOffset;
            int b = bOffset;

            while (n < count)
            {
                var re1 = aStore[a].Re;
                var im1 = aStore[a].Im;
                var re2 = bStore[b].Re;
                var im2 = bStore[b].Im;

                re += (re1 * re2 - im1 * im2);
                im += (re1 * im2 + im1 * re2);

                n++;
                a += aStride;
                b += bStride;
            }

            return new ScalarValue(re, im);
        }

        /// <summary>
        /// Returns the result of the product x = a * x, with a scalar a and a vector x.
        /// </summary>
        /// <param name="alpha">Some arbitrary real scalar.</param>
        /// <param name="store">The vector x.</param>
        /// <param name="offset">The offset in the vector x.</param>
        /// <param name="stride">The offset between two elements in x.</param>
        /// <param name="count">The number of elements to consider (from x).</param>
        public static void dScal(double alpha, double[] store, int offset, int stride, int count)
        {
            int n = 0;
            int i = offset;

            while (n < count)
            {
                store[i] *= alpha;
                n++;
                i += stride;
            }
        }

        /// <summary>
        /// Returns the result of the product x = a * x, with a complex a and a complex vector x.
        /// </summary>
        /// <param name="alpha">Some arbitrary complex scalar.</param>
        /// <param name="store">The complex vector x.</param>
        /// <param name="offset">The offset in the vector x.</param>
        /// <param name="stride">The offset between two elements in x.</param>
        /// <param name="count">The number of elements to consider (from x).</param>
        public static void cScal(ScalarValue alpha, ScalarValue[] store, int offset, int stride, int count)
        {
            int n = 0;
            int i = offset;

            while (n < count)
            {
                store[i] *= alpha;
                n++;
                i += stride;
            }
        }

        /// <summary>
        /// Computes y = a x + y, where x, y are vectors and a is a real scalar.
        /// </summary>
        /// <param name="alpha">Some arbitrary real scalar.</param>
        /// <param name="xStore">The vector x.</param>
        /// <param name="xOffset">The offset in the vector x.</param>
        /// <param name="xStride">The offset between two elements in x.</param>
        /// <param name="yStore">The vector y.</param>
        /// <param name="yOffset">The offset in the vector y.</param>
        /// <param name="yStride">The offset between two elements in y.</param>
        /// <param name="count">The number of elements to take.</param>
        public static void dAxpy(double alpha, double[] xStore, int xOffset, int xStride, double[] yStore, int yOffset, int yStride, int count)
        {
            int n = 0;
            int x = xOffset;
            int y = yOffset;

            while (n < count)
            {
                yStore[y] += alpha * xStore[x];
                n++;
                x += xStride;
                y += yStride;
            }
        }

        /// <summary>
        /// Computes y = a x + y, where x, y are complex vectors and a is a complex scalar.
        /// </summary>
        /// <param name="alpha">Some arbitrary complex scalar.</param>
        /// <param name="xStore">The complex vector x.</param>
        /// <param name="xOffset">The offset in the vector x.</param>
        /// <param name="xStride">The offset between two elements in x.</param>
        /// <param name="yStore">The complex vector y.</param>
        /// <param name="yOffset">The offset in the vector y.</param>
        /// <param name="yStride">The offset between two elements in y.</param>
        /// <param name="count">The number of elements to take.</param>
        public static void cAxpy(ScalarValue alpha, ScalarValue[] xStore, int xOffset, int xStride, ScalarValue[] yStore, int yOffset, int yStride, int count)
        {
            int n = 0;
            int x = xOffset;
            int y = yOffset;

            while (n < count)
            {
                yStore[y] += alpha * xStore[x];
                n++;
                x += xStride;
                y += yStride;
            }
        }
    }
}
