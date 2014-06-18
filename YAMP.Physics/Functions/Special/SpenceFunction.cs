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
    [Description("In mathematics, Spence's function, or dilogarithm, denoted as Li2(z), is a particular case of the polylogarithm. Lobachevsky's function and Clausen's function are closely related functions. Two related special functions are referred to as Spence's function, the dilogarithm itself, and its reflection with the variable negated.")]
    [Kind(PopularKinds.Function)]
    class SpenceFunction : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return DiLog(value);
        }

        #region Algorithms

        /// <summary>
        /// Computes the complex dilogarithm function, also called Spence's function.
        /// </summary>
        /// <param name="z">The complex argument.</param>
        /// <returns>The value Li<sub>2</sub>(z).</returns>
        public static ScalarValue DiLog(ScalarValue z)
        {
            ScalarValue f;
            double a0 = z.Abs();

            if (a0 > 1.0)
            {
                // outside the unit disk, reflect into the unit disk
                var ln = (-z).Ln();
                f = -Math.PI * Math.PI / 6.0 - ln * ln / 2.0 - DiLog(1.0 / z);
            }
            else
            {
                // inside the unit disk...
                if (a0 < 0.75)
                    f = DiLog0(z);
                else if (z.Re < 0.0)
                    f = DiLog(z * z) / 2.0 - DiLog(-z);
                else
                {
                    var e = 1.0 - z;
                    f = e.Abs() < 0.5 ? DiLog1(e) : DiLog_Log_Series(z);
                }
            }

            if ((z.Re > 1.0) && (Math.Sign(f.Im) != Math.Sign(z.Im))) 
                f = f.Conjugate();

            return f;
        }

        static ScalarValue DiLog0(ScalarValue z)
        {
            var zz = z.Clone();
            var f = zz.Clone();

            for (int k = 2; k < 250; k++)
            {
                var f_old = f.Clone();
                zz *= z;
                f += zz / (k * k);

                if (f == f_old)
                    return f;
            }

            throw new YAMPNotConvergedException("spence");
        }

        static ScalarValue DiLog1(ScalarValue e)
        {
            ScalarValue f = new ScalarValue(Math.PI * Math.PI / 6.0);

            if (e == 0.0) 
                return f;

            var L = e.Ln();
            var ek = ScalarValue.One;

            for (int k = 1; k < 250; k++)
            {
                var f_old = f.Clone();
                ek *= e;
                var df = ek * (L - 1.0 / k) / k;
                f += df;

                if (f == f_old)
                    return f;
            }

            throw new YAMPNotConvergedException("spence");
        }

        static ScalarValue DiLog_Log_Series(ScalarValue z)
        {
            var ln = z.Ln();
            var ln2 = ln * ln;
            var f = Math.PI * Math.PI / 6.0 + ln * (1.0 - (-ln).Ln()) - ln2 / 4.0;
            var p = ln.Clone();

            for (int k = 1; k < 17; k++)
            {
                var f_old = f.Clone();
                p *= ln2 / (2 * k + 1) / (2 * k);
                f += (-Helpers.BernoulliNumbers[k] / (2 * k)) * p;

                if (f == f_old) 
                    return f;
            }

            throw new YAMPNotConvergedException("spence");
        }

        #endregion
    }
}
