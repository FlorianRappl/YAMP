/*
    Copyright (c) 2012-2014, Florian Rappl.
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
using YAMP;
using YAMP.Numerics;

namespace YAMP.Physics
{
    [Description("In mathematics, the polylogarithm (also known as Jonquière's function) is a special function Li_s(z) that is defined by the infinite sum, or power series. Only for special values of the order s does the polylogarithm reduce to an elementary function such as the logarithm function.")]
    [Kind(PopularKinds.Function)]
    [Link("http://en.wikipedia.org/wiki/Polylogarithm")]
    class PolyLogFunction : ArgumentFunction
    {
        [Description("Computes the usual polylogarithm with the index s at the argument z. For s = 2 the polylogarithm is the dilogarithm, called Spence function. The argument z = 1 represents the special case of the Riemann Zeta function. A more general case is the Nielson generalized polylogarithm, which can be used with more argument p.")]
        [Example("polylog(1, 0.3)", "Evaluates the polylogarithm for s = 1 and z = 0.3.")]
        public ScalarValue Function(ScalarValue s, ScalarValue z)
        {
            var n = s.GetIntegerOrThrowException("s", Name);
            return Polylog(n, z);
        }

        [Description("Computes the usual polylogarithm with the index s for every value of the matrix Z.")]
        [Example("polylog(1, [0.1 0.3 0.5 1.0])", "Evaluates the polylogarithm for s = 1 at z = 0.1, z = 0.3, z = 0.5 and z = 1.0.")]
        public MatrixValue Function(ScalarValue s, MatrixValue Z)
        {
            var n = s.GetIntegerOrThrowException("s", Name);
            var M = new MatrixValue(Z.DimensionY, Z.DimensionX);

            for (var j = 1; j <= Z.DimensionY; j++)
                for (var i = 1; i <= Z.DimensionX; i++)
                    M[j, i] = Polylog(n, Z[j, i]);

            return M;
        }

        [Description("A generalization of the polylogarithm function. The function reduces to the usual polylogarithm for the case n -> n-1 and p = 1. Therefore S_(n - 1, 1)(z) = Li_n(z).")]
        [Example("polylog(1, 2, 0.5)", "Evaluates the Nielson generalized polylogarithm at n = 1, p = 2 and z = 0.5. For p = 1 we have the ordinary polylogarithm.")]
        public ScalarValue Function(ScalarValue n, ScalarValue p, ScalarValue z)
        {
            if (p == ScalarValue.One)
                return Function(n + 1.0, z);

            //TODO
            return null;
        }

        #region Constants

        static int[] A4 = new[] {
            0x3f9f,0x4b80,0x23b0,0x3d29,
            0x3fd4,0xc179,0x0fb0,0x7fa6,
            0x3fd2,0x6b10,0xa2eb,0x45a8,
            0x3fb2,0x2755,0x5046,0x8d61,
            0x3f7a,0x7c93,0x2883,0x71d4,
            0x3f30,0x0ecf,0x118c,0x37ac,
            0x3ed0,0xe8f4,0xfcf7,0x4b5c,
            0x3e5e,0xf7f1,0x9edf,0xce99,
            0x3dd7,0xdc0d,0xdc57,0xc710,
            0x3d3c,0xe0fd,0x54c8,0xcd17,
            0x3c88,0x677f,0x1c0f,0x7fe8,
            0x3bb7,0x6b92,0xc6f8,0x96ab,
            0x3ac0,0x6494,0x9f50,0xaa8e,
        };

        static int[] B4 = new[] {
            0x4006,0x91f2,0x05e7,0x2e77,
            0x3ffc,0x7bc9,0x2570,0x37d6,
            0x3fd8,0x2f54,0x9821,0x5c12,
            0x3fa0,0x5a4a,0xa7af,0x4bce,
            0x3f53,0x06a5,0x4a3a,0xfc75,
            0x3ef3,0x94a9,0xa71a,0x7eb2,
            0x3e81,0xb420,0xb4f6,0x00ce,
            0x3dfb,0x182b,0x1c44,0xba10,
            0x3d60,0x573c,0xe32c,0x40a9,
            0x3cab,0x92f6,0xc77b,0x077b,
            0x3bda,0x721c,0x6851,0xe1fe,
            0x3ae2,0x81e1,0x5f6f,0xbc13,
        };

        #endregion

        #region Algorithms

        /*  Inversion formula:
         *                                                   [n/2]   n-2r
         *                n                  1     n           -  log    (z)
         *  Li (-z) + (-1)  Li (-1/z)  =  - --- log (z)  +  2  >  ----------- Li  (-1)
         *    n               n              n!                -   (n - 2r)!    2r
         *                                                    r=1
         */
        static ScalarValue PolylogInversion(int n, ScalarValue z)
        {
            int j;
            int nh = n / 2;
            ScalarValue p = ScalarValue.Zero;
            ScalarValue q = ScalarValue.Zero;
            ScalarValue w = (-z).Log();
            ScalarValue s = ScalarValue.Zero;
            ScalarValue m1 = new ScalarValue(-1.0);

            for (var r = 1; r <= nh; r++)
            {
                j = 2 * r;
                p = PolylogZetaNegative(j);
                j = n - j;

                if (j == 0)
                {
                    s += p;
                    break;
                }

                q = w.Pow(new ScalarValue(j)) * (p.Re / Helpers.Factorial(j));
                s += q;
            }

            s = 2.0 * s;
            q = Polylog(n, 1.0 / z);

            if ((n & 1) == 1)
                q = -q;

            return s - q - w.Pow(new ScalarValue(n)) / Helpers.Factorial(n);
        }

        static ScalarValue PolylogNeg1(ScalarValue z)
        {
            var p = 1.0 - z;
            return z / p.Square();
        }

        static ScalarValue PolylogNeg2(ScalarValue z)
        {
            var p = 1.0 - z;
            return z * (1.0 + z) / (p * p * p);
        }

        static ScalarValue PolylogNeg3(ScalarValue z)
        {
            var p = 1.0 - z;
            var pp = p * p;
            var zz = z * z;
            return (z + 4.0 * zz + z * zz) / pp.Square();
        }

        static ScalarValue PolylogNeg4(ScalarValue z)
        {
            var p = 1.0 - z;
            var pp = p * p;
            var pppp = pp * pp;
            var zz = z * z;
            return (z + 11.0 * zz + 11.0 * zz * z + zz * zz) / (pppp * p);
        }

        static ScalarValue PolylogZero(ScalarValue z)
        {
            return z / (1.0 - z);
        }

        static ScalarValue PolylogPos1(ScalarValue z)
        {
            var a = 1 - z;
            var s = a.Ln();
            return -s;
        }

        static double Stirling(int np1, int k)
        {
            var sum = 0.0;

            for (var j = 0; j <= k; j++)
            {
                var sign = Math.Pow(-1, k - j);
                var ncr = Helpers.BinomialCoefficient(k, j);
                var pow = Math.Pow(j, np1);
                sum += (sign * ncr * pow);
            }

            return sum;
        }

        static ScalarValue PolylogNegative(int n, ScalarValue z)
        {
            var p = z / (1.0 - z);
            var np1 = Math.Abs(n) + 1;
            var c = ScalarValue.One;
            var sum = ScalarValue.Zero;

            for (var k = 1; k <= np1; k++)
            {
                c = c * p;
                var S = Stirling(np1, k);
                sum += c * S / k;
            }

            return sum;
        }

        /* Argument -1.
         *                     1-n
         *  Li (-z)  = - (1 - 2   ) Li (z)
         *    n                       n
         */
        static ScalarValue PolylogZetaNegative(int n)
        {
            var s = PolylogZetaPositive(n);
            return s * (Math.Pow(2.0, 1 - n) - 1.0);
        }

        static ScalarValue PolylogZetaPositive(int n)
        {
            var s = Zeta.RiemannZeta(n);
            return new ScalarValue(s);
        }

        public static ScalarValue Polylog(int n, ScalarValue z)
        {
            if (n == 2)
                return SpenceFunction.DiLog(z);

            if (n == -1)
                return PolylogNeg1(z);

            if (n == 0)
                return PolylogZero(z);

            if (n == 1)
                return PolylogPos1(z);

            if (n == -2)
                return PolylogNeg2(z);

            if (n == -3)
                return PolylogNeg3(z);

            if (n == -4)
                return PolylogNeg4(z);

            if (n < -4)
                return PolylogNegative(n, z);

            if (z == ScalarValue.One && n > 1)
                return PolylogZetaPositive(n);

            if (-z == ScalarValue.One && n > 1)
                return PolylogZetaNegative(n);

            /*  This recurrence provides formulas for n < 2.
             *
             *   d                 1
             *   --   Li (x)  =   ---  Li   (x)  .
             *   dx     n          x     n-1
             *
             */
            var s = ScalarValue.Zero;
            var ah = Math.Abs(z.Re) + Math.Abs(z.Im);
            int i, j;

            if (ah > 3.0)
                return PolylogInversion(n, z);
            else if (ah >= 0.75)
            {
                var ad = 0.0;
                var x = z.Log();
                var h = -((-x).Log());

                for (i = 1; i < n; i++)
                    h += 1.0 / i;

                var p = ScalarValue.One;
                s = PolylogZetaPositive(n);

                for (j = 1; j <= n + 1; j++)
                {
                    p = p * x / j;

                    if (j == n - 1)
                        s += h * p;
                    else
                        s += PolylogZetaPositive(n - j) * p;
                }

                j = n + 3;
                x = x * x;

                for (; ; )
                {
                    p = p * x / ((j - 1) * j);
                    h = PolylogZetaPositive(n - j);
                    h = h * p;
                    s += h;
                    ah = Math.Abs(h.Re) + Math.Abs(h.Im);
                    ad = Math.Abs(s.Re) + Math.Abs(s.Im);

                    if (ah < ad * double.Epsilon)
                        break;

                    j += 2;
                }

                return s;
            }
            else if(ah >= 1e-6)
            {
                var p = z * z * z;
                var ad = 0.0;
                var k = 3.0;
                var h = ScalarValue.Zero;

                do
                {
                    p = p * z;
                    k += 1.0;
                    h = p / Math.Pow(k, n);
                    s += h;
                    ah = Math.Abs(h.Re) + Math.Abs(h.Im);
                    ad = Math.Abs(s.Re) + Math.Abs(s.Im);
                }
                while (ah > ad * 1.1e-16);
            }

            s += z * z * z / Math.Pow(3.0, n);
            s += z * z / Math.Pow(2.0, n);
            return s + z;
        }

        #endregion
    }
}
