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
    [Description("In mathematics, Struve functions H_alpha(x), are solutions y(x) of the non-homogenous Bessel's differential equation.")]
    [Kind(PopularKinds.Function)]
    [Link("http://en.wikipedia.org/wiki/Struve_function")]
    class StruveFunction : ArgumentFunction
    {
        [Description("Evaluates the Struve function with alpha = 0 or alpha = 1 at a certain x.")]
        [Example("struve(0, 5)", "Computes the value of the function H_0(x) at x = 5.")]
        [Example("struve(1, 2)", "Computes the value of the function H_1(x) at x = 2.")]
        public ScalarValue Function(ScalarValue alpha, ScalarValue x)
        {
            var a = alpha.GetIntegerOrThrowException("alpha", Name);

            if (a == 0)
                return new ScalarValue(StruveL0(x.Re));
            else if (a == 1)
                return new ScalarValue(StruveL1(x.Re));

            throw new YAMPArgumentRangeException("alpha", 0.0, 1.0);
        }

        #region Constants

        const double TWOBPI = 0.63661977236758134308;
        const double LNR2PI = 0.91893853320467274178;
        const double PI3BY2 = 4.71238898038468985769;

        static readonly double[] ARL0 = new[] {
            0.42127458349979924863,
            -0.33859536391220612188,
            0.21898994812710716064,
            -0.12349482820713185712,
            0.6214209793866958440e-1,
            -0.2817806028109547545e-1,
            0.1157419676638091209e-1,
            -0.431658574306921179e-2,
            0.146142349907298329e-2,
            -0.44794211805461478e-3,
             0.12364746105943761e-3,
             -0.3049028334797044e-4,
             0.663941401521146e-5,
             -0.125538357703889e-5,
             0.20073446451228e-6,
             -0.2588260170637e-7,
             0.241143742758e-8,
             -0.10159674352e-9,
             -0.1202430736e-10,
             0.262906137e-11,
             -0.15313190e-12,
             -0.1574760e-13,
             0.315635e-14,
             -0.4096e-16,
             -0.3620e-16,
             0.239e-17,
             0.36e-18,
             -0.4e-19,
        };

        static readonly double[] ARL0AS = new[] {
                2.00861308235605888600,
                0.403737966500438470e-2,
                -0.25199480286580267e-3,
                0.1605736682811176e-4,
                -0.103692182473444e-5,
                0.6765578876305e-7,
                -0.444999906756e-8,
                0.29468889228e-9,
                -0.1962180522e-10,
                0.131330306e-11,
                 -0.8819190e-13,
                 0.595376e-14,
                 -0.40389e-15,
                 0.2651e-16,
                 -0.208e-17,
                 0.11e-18
            };

        static readonly double[] AI0ML0 = new[] {
                2.00326510241160643125,
                0.195206851576492081e-2,
                0.38239523569908328e-3,
                0.7534280817054436e-4,
                0.1495957655897078e-4,
                0.299940531210557e-5,
                0.60769604822459e-6,
                0.12399495544506e-6,
                0.2523262552649e-7,
                0.504634857332e-8,
                0.97913236230e-9,
                0.18389115241e-9,
                0.3376309278e-10,
                0.611179703e-11,
                0.108472972e-11,
                0.18861271e-12,
                0.3280345e-13,
                0.565647e-14,
                0.93300e-15,
                0.15881e-15,
                0.2791e-16,
                0.389e-17,
                0.70e-18,
                0.16e-18
        };

        static readonly double[] ARL1 = new[] {
            0.38996027351229538208,
            -0.33658096101975749366,
            0.23012467912501645616,
            -0.13121594007960832327,
            0.6425922289912846518e-1,
            -0.2750032950616635833e-1,
            0.1040234148637208871e-1,
            -0.350532294936388080e-2,
            0.105748498421439717e-2,
            -0.28609426403666558e-3,
             0.6925708785942208e-4,
             -0.1489693951122717e-4,
             0.281035582597128e-5,
             -0.45503879297776e-6,
             0.6090171561770e-7,
             -0.623543724808e-8,
             0.38430012067e-9,
             0.790543916e-11,
             -0.489824083e-11,
             0.46356884e-12,
             0.684205e-14,
             -0.569748e-14,
             0.35324e-15,
             0.4244e-16,
             -0.644e-17,
             -0.21e-18,
             0.9e-19
        };

        static readonly double[] ARL1AS = new[] {
            1.97540378441652356868,
            -0.1195130555088294181e-1,
            0.33639485269196046e-3,
            -0.1009115655481549e-4,
            0.30638951321998e-6,
            -0.953704370396e-8,
            0.29524735558e-9,
            -0.951078318e-11,
            0.28203667e-12,
            -0.1134175e-13,
             0.147e-17,
             -0.6232e-16,
             -0.751e-17,
             -0.17e-18,
             0.51e-18,
             0.23e-18,
             0.5e-19
        };

        static readonly double[] AI1ML1 = new[] {
            1.99679361896789136501,
            -0.190663261409686132e-2,
            -0.36094622410174481e-3,
            -0.6841847304599820e-4,
            -0.1299008228509426e-4,
            -0.247152188705765e-5,
            -0.47147839691972e-6,
            -0.9020819982592e-7,
            -0.1730458637504e-7,
            -0.332323670159e-8,
             -0.63736421735e-9,
             -0.12180239756e-9,
             -0.2317346832e-10,
             -0.439068833e-11,
             -0.82847110e-12,
             -0.15562249e-12,
             -0.2913112e-13,
             -0.543965e-14,
             -0.101177e-14,
             -0.18767e-15,
             -0.3484e-16,
             -0.643e-17,
             -0.118e-17,
             -0.22e-18,
             -0.4e-19,
             -0.1e-19
        };

        #endregion

        #region Algorithms

        // Credit here goes to
        // https://github.com/mathnet/mathnet-numerics/blob/master/src/Numerics/SpecialFunctions/ModifiedStruve.cs
        // for creating, modifying and testing the struve function.

        public static double StruveL0(double x)
        {
            // MACHINE-DEPENDENT VALUES (Suitable for IEEE-arithmetic machines)
            const int NTERM1 = 25;
            const int NTERM2 = 14;
            const int NTERM3 = 21;

            const double XLOW = 4.4703484e-8;
            const double XMAX = 1.797693e308;

            const double XHIGH1 = 5.1982303e8;
            const double XHIGH2 = 2.5220158e17;

            if (x < 0.0)
                return -StruveL0(-x);

            // Code for |xvalue| <= 16
            if (x <= 16.0)
            {
                if (x < XLOW)
                    return TWOBPI * x;

                double T = (4.0 * x - 24.0) / (x + 24.0);
                return TWOBPI * x * Helpers.ChebEval(NTERM1, ARL0, T) * Math.Exp(x);
            }

            // Code for |xvalue| > 16
            double ch1;

            if (x > XHIGH2)
                ch1 = 1.0;
            else
            {
                double T = (x - 28.0) / (4.0 - x);
                ch1 = Helpers.ChebEval(NTERM2, ARL0AS, T);
            }

            double ch2;

            if (x > XHIGH1)
                ch2 = 1.0;
            else
            {
                double xsq = x * x;
                double T = (800.0 - xsq) / (288.0 + xsq);
                ch2 = Helpers.ChebEval(NTERM3, AI0ML0, T);
            }

            double test = Math.Log(ch1) - LNR2PI - Math.Log(x) / 2.0 + x;

            if (test > Math.Log(XMAX))
                throw new YAMPNotConvergedException("struve");

            return Math.Exp(test) - TWOBPI * ch2 / x;
        }

        /// <summary>
        /// Returns the modified Struve function of order 1.
        /// </summary>
        /// <param name="x">The value to compute the function of.</param>
        /// <returns></returns>
        public static double StruveL1(double x)
        {
            // MACHINE-DEPENDENT VALUES (Suitable for IEEE-arithmetic machines)
            const int NTERM1 = 24; 
            const int NTERM2 = 13; 
            const int NTERM3 = 22;

            const double XLOW1 = 5.7711949e-8; 
            const double XLOW2 = 3.3354714e-154; 
            const double XMAX = 1.797693e308;

            const double XHIGH1 = 5.19823025e8; 
            const double XHIGH2 = 2.7021597e17;

            if (x < 0.0)
                return StruveL1(-x);

            // CODE FOR |x| <= 16
            if (x <= 16.0)
            {
                if (x <= XLOW2)
                    return 0.0;

                double xsq = x * x;

                if (x < XLOW1)
                    return xsq / PI3BY2;

                double t = (4.0 * x - 24.0) / (x + 24.0);
                return xsq * Helpers.ChebEval(NTERM1, ARL1, t) * Math.Exp(x) / PI3BY2;
            }

            // CODE FOR |x| > 16
            double ch1;

            if (x > XHIGH2)
                ch1 = 1.0;
            else
            {
                double t = (x - 30.0) / (2.0 - x);
                ch1 = Helpers.ChebEval(NTERM2, ARL1AS, t);
            }

            double ch2;

            if (x > XHIGH1)
                ch2 = 1.0;
            else
            {
                double xsq = x * x;
                double t = (800.0 - xsq) / (288.0 + xsq);
                ch2 = Helpers.ChebEval(NTERM3, AI1ML1, t);
            }

            double test = Math.Log(ch1) - LNR2PI - Math.Log(x) / 2.0 + x;

            if (test > Math.Log(XMAX))
                throw new YAMPNotConvergedException("struve");

            return Math.Exp(test) - TWOBPI * ch2;
        }

        /// <summary>
        /// Returns the difference between the Bessel I0 and Struve L0 functions.
        /// </summary>
        /// <param name="x">The value to compute the function of.</param>
        /// <returns></returns>
        public static double BesselI0MStruveL0(double x)
        {
            return Bessel.j0(x) - StruveL0(x);
        }

        /// <summary>
        /// Returns the difference between the Bessel I1 and Struve L1 functions.
        /// </summary>
        /// <param name="x">The value to compute the function of.</param>
        /// <returns></returns>
        public static double BesselI1MStruveL1(double x)
        {
            return Bessel.j1(x) - StruveL1(x);
        }

        #endregion
    }
}
