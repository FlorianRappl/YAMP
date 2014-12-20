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
    [Description("In integral calculus, elliptic integrals originally arose in connection with the problem of giving the arc length of an ellipse. They were first studied by Giulio Fagnano and Leonhard Euler. The incomplete elliptic integral of the first kind F is evaluated by this function.")]
    [Kind(PopularKinds.Function)]
    class EllipticFFunction : ArgumentFunction
    {
        [Description("Computes the incomplete elliptic integral of the first kind at the arguments phi and k.")]
        [Example("ellipticf(pi / 2, 1)", "Evaluates the incomplete elliptic integral at phi = pi / 2 and k = 1.")]
        public ScalarValue Function(ScalarValue phi, ScalarValue k)
        {
            return new ScalarValue(EllipticF(phi.Re, k.Re));
        }

        #region Algorithm

        public static double EllipticF(double phi, double k)
        {
            if (Math.Abs(phi) > Helpers.HalfPI) 
                throw new YAMPArgumentRangeException("phi", -Helpers.HalfPI, Helpers.HalfPI);

            if ((k < 0) || (k > 1.0))
                throw new YAMPArgumentRangeException("k", 0, 1);

            double s = Math.Sin(phi);
            double c = Math.Cos(phi);
            double z = s * k;
            return s * CarlsonFFunction.CarlsonF(c * c, 1.0 - z * z, 1.0);
        }

        #endregion
    }
}
