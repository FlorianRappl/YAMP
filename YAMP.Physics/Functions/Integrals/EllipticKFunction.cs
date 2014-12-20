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
    [Description("Elliptic Integrals are said to be 'complete' when the amplitude φ = π/2 and therefore x = 1. The complete elliptic integral of the first kind K is evaluated by this function.")]
    [Kind(PopularKinds.Function)]
    class EllipticKFunction : StandardFunction
    {
        [Description("Computes the complete elliptic integral of the first kind at the argument x.")]
        [Example("elliptick(3)", "Evaluates the complete elliptic integral at x = 3.")]
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return new ScalarValue(EllipticK(value.Re));
        }

        #region Algorithms

        public static double EllipticK(double k)
        {
            if ((k < 0) || (k > 1.0))
                throw new YAMPArgumentRangeException("k", 0.0, 1.0);

            if (k < 0.25)
                return EllipticSeries(k);
            else if (k > 0.875)
            {
                // for large k, use the asymptotic expansion near k~1, k'~0
                double k1 = Math.Sqrt(1.0 - k * k);

                // k'=0.484 at k=0.875
                if (k1 == 0.0) 
                    return double.PositiveInfinity;

                return EllipticAsymptotic(k1);
            }

            return EllipticAGM(k);
        }

        static double EllipticSeries(double k)
        {
            double z = 1.0;
            double f = 1.0;

            for (int n = 1; n < 250; n++)
            {
                double f_old = f;
                z = z * (2 * n - 1) / (2 * n) * k;
                f += z * z;

                if (f == f_old) 
                    return Helpers.HalfPI * f;
            }

            throw new YAMPNotConvergedException("EllipticK");
        }

        static double EllipticAsymptotic(double k1)
        {
            double p = 1.0;
            double q = Math.Log(1.0 / k1) + 2.0 * Helpers.LogTwo;
            double f = q;

            for (int m = 1; m < 250; m++)
            {
                double f_old = f;
                p *= k1 / m * (m - 0.5);
                q -= 1.0 / m / (2 * m - 1);
                double df = p * p * q;
                f += df;

                if (f == f_old)
                    return f;
            }

            throw new YAMPNotConvergedException("EllipticK");
        }

        static double EllipticAGM(double k)
        {
            double tol = Math.Pow(2.0, -24);
            double a = 1.0 - k;
            double b = 1.0 + k;

            for (int n = 0; n < 250; n++)
            {
                double am = (a + b) / 2.0;

                if (Math.Abs(a - b) < tol)
                    return Helpers.HalfPI / am;

                double gm = Math.Sqrt(a * b);
                a = am;
                b = gm;
            }

            throw new YAMPNotConvergedException("EllipticK");
        }

        #endregion
    }
}
