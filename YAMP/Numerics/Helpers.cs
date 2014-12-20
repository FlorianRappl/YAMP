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

namespace YAMP.Numerics
{
    /// <summary>
    /// Provides some commonly used methods for numeric algorithms.
    /// </summary>
    public static class Helpers
    {
        #region Some numbers

        static readonly double[] FACT_34_TO_170 = new[] {
            2.95232799039604140847618609644e38,
            1.03331479663861449296666513375e40,
            3.71993326789901217467999448151e41,
            1.37637530912263450463159795816e43,
            5.23022617466601111760007224100e44,
            2.03978820811974433586402817399e46,
            8.15915283247897734345611269600e47,
            3.34525266131638071081700620534e49,
            1.40500611775287989854314260624e51,
            6.04152630633738356373551320685e52,
            2.65827157478844876804362581101e54,
            1.19622220865480194561963161496e56,
            5.50262215981208894985030542880e57,
            2.58623241511168180642964355154e59,
            1.24139155925360726708622890474e61,
            6.08281864034267560872252163321e62,
            3.04140932017133780436126081661e64,
            1.55111875328738228022424301647e66,
            8.06581751709438785716606368564e67,
            4.27488328406002556429801375339e69,
            2.30843697339241380472092742683e71,
            1.26964033536582759259651008476e73,
            7.10998587804863451854045647464e74,
            4.05269195048772167556806019054e76,
            2.35056133128287857182947491052e78,
            1.38683118545689835737939019720e80,
            8.32098711274139014427634118320e81,
            5.07580213877224798800856812177e83,
            3.14699732603879375256531223550e85,
            1.982608315404440064116146708360e87,  
            1.268869321858841641034333893350e89,  
            8.247650592082470666723170306800e90,  
            5.443449390774430640037292402480e92,  
            3.647111091818868528824985909660e94,  
            2.480035542436830599600990418570e96,  
            1.711224524281413113724683388810e98,  
            1.197857166996989179607278372170e100, 
            8.504785885678623175211676442400e101, 
            6.123445837688608686152407038530e103, 
            4.470115461512684340891257138130e105, 
            3.307885441519386412259530282210e107, 
            2.480914081139539809194647711660e109, 
            1.885494701666050254987932260860e111, 
            1.451830920282858696340707840860e113, 
            1.132428117820629783145752115870e115, 
            8.946182130782975286851441715400e116, 
            7.156945704626380229481153372320e118, 
            5.797126020747367985879734231580e120, 
            4.753643337012841748421382069890e122, 
            3.945523969720658651189747118010e124, 
            3.314240134565353266999387579130e126, 
            2.817104114380550276949479442260e128, 
            2.422709538367273238176552320340e130, 
            2.107757298379527717213600518700e132, 
            1.854826422573984391147968456460e134, 
            1.650795516090846108121691926250e136, 
            1.485715964481761497309522733620e138, 
            1.352001527678402962551665687590e140, 
            1.243841405464130725547532432590e142, 
            1.156772507081641574759205162310e144, 
            1.087366156656743080273652852570e146, 
            1.032997848823905926259970209940e148, 
            9.916779348709496892095714015400e149, 
            9.619275968248211985332842594960e151, 
            9.426890448883247745626185743100e153, 
            9.332621544394415268169923885600e155, 
            9.33262154439441526816992388563e157,  
            9.42594775983835942085162312450e159,  
            9.61446671503512660926865558700e161,  
            9.90290071648618040754671525458e163,  
            1.02990167451456276238485838648e166,  
            1.08139675824029090050410130580e168,  
            1.146280563734708354534347384148e170, 
            1.226520203196137939351751701040e172, 
            1.324641819451828974499891837120e174, 
            1.443859583202493582204882102460e176, 
            1.588245541522742940425370312710e178, 
            1.762952551090244663872161047110e180, 
            1.974506857221074023536820372760e182, 
            2.231192748659813646596607021220e184, 
            2.543559733472187557120132004190e186, 
            2.925093693493015690688151804820e188, 
            3.393108684451898201198256093590e190, 
            3.96993716080872089540195962950e192,  
            4.68452584975429065657431236281e194,  
            5.57458576120760588132343171174e196,  
            6.68950291344912705758811805409e198,  
            8.09429852527344373968162284545e200,  
            9.87504420083360136241157987140e202,  
            1.21463043670253296757662432419e205,  
            1.50614174151114087979501416199e207,  
            1.88267717688892609974376770249e209,  
            2.37217324288004688567714730514e211,  
            3.01266001845765954480997707753e213,  
            3.85620482362580421735677065923e215,  
            4.97450422247728744039023415041e217,  
            6.46685548922047367250730439554e219,  
            8.47158069087882051098456875820e221,  
            1.11824865119600430744996307608e224,  
            1.48727070609068572890845089118e226,  
            1.99294274616151887673732419418e228,  
            2.69047270731805048359538766215e230,  
            3.65904288195254865768972722052e232,  
            5.01288874827499166103492629211e234,  
            6.91778647261948849222819828311e236,  
            9.61572319694108900419719561353e238,  
            1.34620124757175246058760738589e241,  
            1.89814375907617096942852641411e243,  
            2.69536413788816277658850750804e245,  
            3.85437071718007277052156573649e247,  
            5.55029383273930478955105466055e249,  
            8.04792605747199194484902925780e251,  
            1.17499720439091082394795827164e254,  
            1.72724589045463891120349865931e256,  
            2.55632391787286558858117801578e258,  
            3.80892263763056972698595524351e260,  
            5.71338395644585459047893286526e262,  
            8.62720977423324043162318862650e264,  
            1.31133588568345254560672467123e267,  
            2.00634390509568239477828874699e269,  
            3.08976961384735088795856467036e271,  
            4.78914290146339387633577523906e273,  
            7.47106292628289444708380937294e275,  
            1.17295687942641442819215807155e278,  
            1.85327186949373479654360975305e280,  
            2.94670227249503832650433950735e282,  
            4.71472363599206132240694321176e284,  
            7.59070505394721872907517857094e286,  
            1.22969421873944943411017892849e289,  
            2.00440157654530257759959165344e291,  
            3.28721858553429622726333031164e293,  
            5.42391066613158877498449501421e295,  
            9.00369170577843736647426172359e297,  
            1.50361651486499904020120170784e300,  
            2.52607574497319838753801886917e302,  
            4.26906800900470527493925188890e304,  
            7.25741561530799896739672821113e306 
        };

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
        /// Returns a boolean if the given integer is a prime number.
        /// </summary>
        /// <param name="n">The integer to examine.</param>
        /// <returns>The result of the test.</returns>
        public static bool IsPrimeNumber(int n)
        {
            if (n < 8)
                return ((n == 2) || (n == 3) || (n == 5) || (n == 7));
            else
            {
                if (n % 2 == 0)
                    return (false);

                int m = n - 1;
                int d = m;
                int s = 0;

                while (d % 2 == 0)
                {
                    s++;
                    d = d / 2;
                }

                if (n < 1373653)
                    return (IsProbablyPrime(n, m, s, d, 2) && IsProbablyPrime(n, m, s, d, 3));

                return (IsProbablyPrime(n, m, s, d, 2) && IsProbablyPrime(n, m, s, d, 7) && IsProbablyPrime(n, m, s, d, 61));

            }
        }

        static bool IsProbablyPrime(int n, int m, int s, int d, int w)
        {
            int x = PowMod(w, d, n);

            if ((x == 1) || (x == m))
                return true;

            for (int i = 0; i < s; i++)
            {
                x = PowMod(x, 2, n);
                if (x == 1)
                    return false;
                if (x == m)

                    return true;
            }
            return false;
        }

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
            if (A == 0)
                return Math.Max(B, 1);

            if (B == 0)
                return Math.Max(A, 1);

            int f = 1;

            while (true)
            {
                if (A == B)
                    return f * A;

                if (A == 1 || B == 1)
                    return f;

                if ((A % 2 == 0) && (B % 2 == 0))
                {
                    f = 2 * f;
                    A = A / 2;
                    B = B / 2;
                }
                else if ((A % 2 == 0) && (B % 2 != 0))
                    A = A / 2;
                else if ((A % 2 != 0) && (B % 2 == 0))
                    B = B / 2;
                else if (A > B)
                    A = (A - B) / 2;
                else
                    B = (B - A) / 2;
            }
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
            if (n < 34)
            {
                var res = 1.0;

                while (n > 1)
                    res *= n--;

                return res;
            }
            else if (n < 171)
                return FACT_34_TO_170[n - 34];

            return Math.Exp(Gamma.LogGamma(n + 1.0));
        }

        /// <summary>
        /// Computes the complex binomial coefficient (very general) given two values, n choose k.
        /// </summary>
        /// <param name="n">We have n elements.</param>
        /// <param name="k">We choose k elements.</param>
        /// <returns>The binomial coefficient.</returns>
        public static ScalarValue BinomialCoefficient(ScalarValue n, ScalarValue k)
        {
            return Gamma.LinearGamma(n + 1.0) / (Gamma.LinearGamma(k + 1.0) * Gamma.LinearGamma(n - k + 1.0));
        }

        /// <summary>
        /// Computes the real binomial coefficient (almost general) given two values, n choose k.
        /// </summary>
        /// <param name="n">We have n elements.</param>
        /// <param name="k">We choose k elements.</param>
        /// <returns>The binomial coefficient.</returns>
        public static double BinomialCoefficient(double n, double k)
        {
            return Gamma.LinearGamma(n + 1.0) / (Gamma.LinearGamma(k + 1.0) * Gamma.LinearGamma(n - k + 1.0));
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
        /// <param name="n">The order for the summation of the polynomial.</param>
        /// <param name="coefficients">The coefficients to consider.</param>
        /// <param name="x">The real evaluation argument.</param>
        /// <returns>The value.</returns>
        public static double ChebEval(int n, double[] coefficients, double x)
        {
            // If |x|  < 0.6 use the standard Clenshaw method
            if (Math.Abs(x) < 0.6)
            {
                double u0 = 0.0;
                double u1 = 0.0;
                double u2 = 0.0;
                double xx = x + x;

                for (int i = n; i >= 0; i--)
                {
                    u2 = u1;
                    u1 = u0;
                    u0 = xx * u1 + coefficients[i] - u2;
                }

                return (u0 - u2) / 2.0;
            }

            // If ABS ( T )  > =  0.6 use the Reinsch modification
            // T > =  0.6 code
            if (x > 0.0)
            {
                double u1 = 0.0;
                double d1 = 0.0;
                double d2 = 0.0;
                double xx = (x - 0.5) - 0.5;
                xx = xx + xx;

                for (int i = n; i >= 0; i--)
                {
                    d2 = d1;
                    double u2 = u1;
                    d1 = xx * u2 + coefficients[i] + d2;
                    u1 = d1 + u2;
                }

                return (d1 + d2) / 2.0;
            }
            else
            {
                // T < =  -0.6 code
                double u1 = 0.0;
                double d1 = 0.0;
                double d2 = 0.0;
                double xx = (x + 0.5) + 0.5;
                xx = xx + xx;

                for (int i = n; i >= 0; i--)
                {
                    d2 = d1;
                    double u2 = u1;
                    d1 = xx * u2 + coefficients[i] - d2;
                    u1 = d1 - u2;
                }

                return (d1 - d2) / 2.0;
            }
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
            var d = ScalarValue.Zero;
            var dd = ScalarValue.Zero;
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
