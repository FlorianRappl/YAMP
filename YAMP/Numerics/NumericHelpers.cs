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

        public static readonly double[] twopi_pow = new double[] { 1.0,
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

        #endregion

        #region Functions

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

        public static double Factorial(int arg)
        {
            var res = 1;

            while (arg > 1)
                res *= arg--;

            return res;
        }

        public static ScalarValue BinomialCoefficient(ScalarValue x, ScalarValue y)
        {
            return Gamma.LinearGamma(x + 1) / (Gamma.LinearGamma(y + 1) * Gamma.LinearGamma(x - y + 1));
        }

        public static ScalarValue Power(ScalarValue x, int power)
        {
            if (power == 0)
                return new ScalarValue(1);

            if (power > 0)
            {
                var result = x;

                for (int i = 1; i < power; i++)
                    result *= x;

                return result;
            }

            var inv = 1 / x;

            if (power < -1)
                return Power(inv, -power);

            return inv;
        }

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

        public struct ChebSeries
        {
            public double[] Coefficients;

            public int Order;

            public double LowerPoint;

            public double UpperPoint;

            public int SinglePrecisionOrder;
        };

        #endregion
    }
}
