/*
    Copyright (c) 2012-2013, Florian Rappl.
    All rights reserved.

    Redistribution and use in source and binary forms, with or without
    modification, are permitted provided that the following conditions are met:
        * Redistributions of source code must retain the above copyright
          notice, this list of conditions and the following disclaimer.
        * Redistributions in binary form must reproduce the above copyright
          notice, this list of conditions and the following disclaimer in the
          documentation and/or other materials provided with the distribution.
        * Neither the name of the YAMP team nor the names of its contributors
          may be used to endorse or promote products derived from this
          software without specific prior written permission.

    THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
    ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
    WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
    DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
    DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
    (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
    LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
    ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
    (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
    SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System;

namespace YAMP.Numerics
{
    /// <summary>
    /// Provides some commonly used methods for numeric algorithms.
    /// </summary>
    public static class Helpers
    {
        #region Some numbers

        /// <summary>
        /// Provides access to (2*pi)^(10*n) with n = 0, ..., 17.
        /// </summary>
        public static readonly double[] TwoPIpow = new double[] { 
            1.0,
            9.589560061550901348e+007,
            9.195966217409212684e+015,
            8.818527036583869903e+023,
            8.456579467173150313e+031,
            8.109487671573504384e+039,
            7.776641909496069036e+047,
            7.457457466828644277e+055,
            7.151373628461452286e+063,
            6.857852693272229709e+071,
            6.576379029540265771e+079,
            6.306458169130020789e+087,
            6.047615938853066678e+095,
            5.799397627482402614e+103,
            5.561367186955830005e+111,
            5.333106466365131227e+119,
            5.114214477385391780e+127,
            4.904306689854036836e+135
        };

        /// <summary>
        /// Provides access to the first 21 bernoulli numbers.
        /// </summary>
        public static readonly double[] BernoulliNumbers = new double[] {
            1.0,
            1.0 / 6.0,
            -1.0 / 30.0, 
            1.0 / 42.0,
            -1.0 / 30.0,
            5.0 / 66.0,
            -691.0 / 2730.0,
            7.0 / 6.0,
            -3617.0 / 510.0,
            43867.0 / 798.0,
            -174611.0 / 330.0, 
            854513.0 / 138.0,
            -236364091.0 / 2730.0,
            8553103.0 / 6.0, 
            -23749461029.0 / 870.0,
            8615841276005.0 / 14322.0,
            -7709321041217.0 / 510.0,
            2577687858367.0 / 6.0,
            -26315271553053477373.0 / 1919190.0,
            2929993913841559.0 / 6.0,
            -261082718496449122051.0 / 13530.0
        };

        /// <summary>
        /// Provides access do some Lanczos numbers.
        /// </summary>
        public static readonly double[] LanczosD = new double[] {
             2.48574089138753565546e-5,
             1.05142378581721974210,
            -3.45687097222016235469,
             4.51227709466894823700,
            -2.98285225323576655721,
             1.05639711577126713077,
            -1.95428773191645869583e-1,
             1.70970543404441224307e-2,
            -5.71926117404305781283e-4,
             4.63399473359905636708e-6,
            -2.71994908488607703910e-9
        };

        /// <summary>
        /// Value of the LaczosR number.
        /// </summary>
        public const double LanczosR = 10.900511;

        /// <summary>
        /// Value of 4 * Pi
        /// </summary>
        public const double FourPI = 4.0 * Math.PI;

        /// <summary>
        /// Value of 2 * Pi
        /// </summary>
        public const double TwoPI = 2.0 * Math.PI;

        /// <summary>
        /// Value of Pi / 2
        /// </summary>
        public const double HalfPI = Math.PI / 2.0;

        /// <summary>
        /// Value of Sqrt(2)
        /// </summary>
        public static readonly double SqrtTwo = Math.Sqrt(2.0);

        /// <summary>
        /// Value of Sqrt(3)
        /// </summary>
        public static readonly double SqrtThree = Math.Sqrt(3.0);

        /// <summary>
        /// Value of Sqrt(Pi)
        /// </summary>
        public static readonly double SqrtPI = Math.Sqrt(Math.PI);

        /// <summary>
        /// Value of Sqrt(2 * Pi)
        /// </summary>
        public static readonly double SqrtTwoPI = Math.Sqrt(2.0 * Math.PI);

        /// <summary>
        /// Value of ln(Pi)
        /// </summary>
        public static readonly double LogPI = Math.Log(Math.PI);

        /// <summary>
        /// Value of ln(2)
        /// </summary>
        public static readonly double LogTwo = Math.Log(2.0);

        #endregion

        #region Functions

        /// <summary>
        /// Computes a power of an integer in modular arithmetic.
        /// </summary>
        /// <param name="b">The base, which must be positive.</param>
        /// <param name="e">The exponent, which must be positive.</param>
        /// <param name="m">The modulus, which must be positive.</param>
        /// <returns>The value of b<sup>e</sup> mod m.</returns>
        public static int PowMod(int b, int e, int m)
        {
            if (b < 0)
                throw new YAMPArgumentRangeException("b", -1);

            if (e < 1)
                throw new YAMPArgumentRangeException("e", 0);

            if (m < 1)
                throw new YAMPArgumentRangeException("m", 0);

            long bb = Convert.ToInt64(b);
            long mm = Convert.ToInt64(m);
            long rr = 1;

            while (e > 0)
            {
                if ((e & 1) == 1)
                    rr = checked((rr * bb) % mm);

                e = e >> 1;
                bb = checked((bb * bb) % mm);
            }

            return Convert.ToInt32(rr);
        }

        /// <summary>
        /// Computes the greatest common divisor of two numbers.
        /// </summary>
        /// <param name="A">The first number.</param>
        /// <param name="B">The second number.</param>
        /// <returns>The greatest common divisor.</returns>
        public static int GCD(int A, int B)
        {
            if (A == B)
                return A;

            if (A == 1 || B == 1)
                return 1;

            if ((A % 2 == 0) && (B % 2 == 0))
                return 2 * GCD(A / 2, B / 2);
            else if ((A % 2 == 0) && (B % 2 != 0))
                return GCD(A / 2, B);
            else if ((A % 2 != 0) && (B % 2 == 0))
                return GCD(A, B / 2);

            if (A > B)
                return GCD((A - B) / 2, B);

            return GCD(A, (B - A) / 2);
        }  

        /// <summary>
        /// Computes the length of a right triangle's hypotenuse.
        /// </summary>
        /// <param name="a">The length of one side.</param>
        /// <param name="b">The length of another side.</param>
        /// <returns>The length of the hypotenuse, sqrt(x<sup>2</sup> + y<sup>2</sup>).</returns>
        public static double Hypot(double a, double b)
        {
            var r = 0.0;

            if (Math.Abs(a) > Math.Abs(b))
            {
                r = b / a;
                r = Math.Abs(a) * Math.Sqrt(1 + r * r);
            }
            else if (b != 0)
            {
                r = a / b;
                r = Math.Abs(b) * Math.Sqrt(1 + r * r);
            }

            return r;
        }

        /// <summary>
        /// Computes the factorial of an integer and returns a double with the result.
        /// </summary>
        /// <param name="n">The argument to take the factorial.</param>
        /// <returns>A double with the result of n!.</returns>
        public static double Factorial(int n)
        {
            var res = 1.0;

            while (n > 1)
                res *= n--;

            return res;
        }

        /// <summary>
        /// Computes the complex binomial coefficient (very general) given two values, n choose k.
        /// </summary>
        /// <param name="n">We have n elements.</param>
        /// <param name="k">We choose k elements.</param>
        /// <returns>The binomial coefficient.</returns>
        public static ScalarValue BinomialCoefficient(ScalarValue n, ScalarValue k)
        {
            return Gamma.LinearGamma(n + 1) / (Gamma.LinearGamma(k + 1) * Gamma.LinearGamma(n - k + 1));
        }

        /// <summary>
        /// Takes the power of z to an integer n.
        /// </summary>
        /// <param name="z">The complex value z in C.</param>
        /// <param name="n">The power n in N.</param>
        /// <returns>The result of z^n.</returns>
        public static ScalarValue Power(ScalarValue z, int n)
        {
            if (n == 0)
                return new ScalarValue(1);

            if (n > 0)
            {
                var result = z;

                for (int i = 1; i < n; i++)
                    result *= z;

                return result;
            }

            var inv = 1 / z;

            if (n < -1)
                return Power(inv, -n);

            return inv;
        }

        /// <summary>
        /// computes the N-th roots of unity, which are the factors in a length-N Fourier transform.
        /// </summary>
        /// <param name="N">What number of roots.</param>
        /// <param name="sign">The sign to take.</param>
        /// <returns>The number roots, i.e. a N  + 1 size array.</returns>
        public static ScalarValue[] ComputeRoots(int N, int sign)
        {
            var u = new ScalarValue[N + 1];
            double t = sign * TwoPI / N;
            u[0] = new ScalarValue(1.0);

            for (int r = 1; r < N; r++)
            {
                double rt = r * t;
                u[r] = new ScalarValue(Math.Cos(rt), Math.Sin(rt));
            }

            u[N] = new ScalarValue(1.0);
            return u;
        }

        /// <summary>
        /// Evaluate a real Chebyshev polynomial on an interval, given the coefficients.
        /// </summary>
        /// <param name="cs">The coefficients to consider.</param>
        /// <param name="x">The real evaluation argument.</param>
        /// <returns>The value.</returns>
        public static double ChebEval(ChebSeries cs, double x)
        {
            int j;
            double d = 0.0;
            double dd = 0.0;
            double y = (2.0 * x - cs.LowerPoint - cs.UpperPoint) / (cs.UpperPoint - cs.LowerPoint);
            double y2 = 2.0 * y;

            for (j = cs.Order; j >= 1; j--)
            {
                double temp = d;
                d = y2 * d - dd + cs.Coefficients[j];
                dd = temp;
            }

            return y * d - dd + 0.5 * cs.Coefficients[0];
        }

        /// <summary>
        /// Evaluate a complex Chebyshev polynomial on an interval, given the coefficients.
        /// </summary>
        /// <param name="cs">The coefficients to consider.</param>
        /// <param name="z">The complex evaluation argument.</param>
        /// <returns>The value.</returns>
        public static ScalarValue ChebEval(ChebSeries cs, ScalarValue z)
        {
            int j;
            var d = new ScalarValue();
            var dd = new ScalarValue();
            var y = (2.0 * z - cs.LowerPoint - cs.UpperPoint) / (cs.UpperPoint - cs.LowerPoint);
            var y2 = 2.0 * y;

            for (j = cs.Order; j >= 1; j--)
            {
                var temp = d;
                d = y2 * d - dd + cs.Coefficients[j];
                dd = temp;
            }

            return y * d - dd + 0.5 * cs.Coefficients[0];
        }

        #endregion

        #region Nested Structure

        /// <summary>
        /// The coefficients with order, and more information.
        /// </summary>
        public struct ChebSeries
        {
            /// <summary>
            /// The (real) coefficients of the Chebyshev polynomial.
            /// </summary>
            public double[] Coefficients;

            /// <summary>
            /// The order of the polynomial.
            /// </summary>
            public int Order;

            /// <summary>
            /// The lowest point in the interval.
            /// </summary>
            public double LowerPoint;

            /// <summary>
            /// The highest point in the interval.
            /// </summary>
            public double UpperPoint;

            /// <summary>
            /// The order of the single precision.
            /// </summary>
            public int SinglePrecisionOrder;
        };

        #endregion
    }
}
