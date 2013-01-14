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
using YAMP;
using YAMP.Numerics;

namespace YAMP.Physics
{
    [Description("In mathematics, the polygamma function of order m is a meromorphic function on C and defined as the (m+1)-th derivative of the logarithm of the gamma function. This function represents the first derivative, Psi(0), of the Gamma function.")]
    [Kind(PopularKinds.Function)]
    class Psi0Function : StandardFunction
    {
        protected override ScalarValue GetValue(ScalarValue value)
        {
            return new ScalarValue(Psi0(value.Re));
        }

        #region Number

        static readonly double[] psics_data = new double[] {
           -.038057080835217922,
            .491415393029387130, 
           -.056815747821244730,
            .008357821225914313,
           -.001333232857994342,
            .000220313287069308,
           -.000037040238178456,
            .000006283793654854,
           -.000001071263908506,
            .000000183128394654,
           -.000000031353509361,
            .000000005372808776,
           -.000000000921168141,
            .000000000157981265,
           -.000000000027098646,
            .000000000004648722,
           -.000000000000797527,
            .000000000000136827,
           -.000000000000023475,
            .000000000000004027,
           -.000000000000000691,
            .000000000000000118,
           -.000000000000000020
        };

        static readonly double[] apsics_data = new double[] {    
           -.0204749044678185,
           -.0101801271534859,
            .0000559718725387,
           -.0000012917176570,
            .0000000572858606,
           -.0000000038213539,
            .0000000003397434,
           -.0000000000374838,
            .0000000000048990,
           -.0000000000007344,
            .0000000000001233,
           -.0000000000000228,
            .0000000000000045,
           -.0000000000000009,
            .0000000000000002,
           -.0000000000000000 
         };

        #endregion

        #region Helpers

        static Helpers.ChebSeries psi_cs = new Helpers.ChebSeries
        {
            Coefficients = psics_data,
            Order = 22,
            LowerPoint = -1,
            UpperPoint = 1,
            SinglePrecisionOrder = 17
        };

        static Helpers.ChebSeries apsi_cs = new Helpers.ChebSeries
        {
            Coefficients = apsics_data,
            Order = 15,
            LowerPoint = -1,
            UpperPoint = 1,
            SinglePrecisionOrder = 9
        };

        #endregion

        #region Methods

        public static double Psi0(double x)
        {
            double y = Math.Abs(x);

            if (x == 0.0 || x == -1.0 || x == -2.0)
                throw new YAMPException("Domain error!");
            else if (y >= 2.0)
            {
                double t = 8.0 / (y * y) - 1.0;
                var c = Helpers.ChebEval(apsi_cs, t);

                if (x < 0.0)
                {
                    double si = Math.Sin(Math.PI * x);
                    double co = Math.Cos(Math.PI * x);

                    if (Math.Abs(si) < 2.0 * 1.4916681462400413e-154)
                        throw new YAMPException("Domain error!");
                    else
                        return Math.Log(y) - 0.5 / x + c - Math.PI * co / si;
                }

                return Math.Log(y) - 0.5 / x + c;
            }
            else
            {
                /* -2 < x < 2 */

                if (x < -1.0)
                {
                    /* x = -2 + v */
                    double v = x + 2.0;
                    double t1 = 1.0 / x;
                    double t2 = 1.0 / (x + 1.0);
                    double t3 = 1.0 / v;
                    var c = Helpers.ChebEval(psi_cs, 2.0 * v - 1.0);
                    return -(t1 + t2 + t3) + c;
                }
                else if (x < 0.0)
                {
                    /* x = -1 + v */
                    double v = x + 1.0;
                    double t1 = 1.0 / x;
                    double t2 = 1.0 / v;
                    var c = Helpers.ChebEval(psi_cs, 2.0 * v - 1.0);
                    return -(t1 + t2) + c;
                }
                else if (x < 1.0)
                {
                    /* x = v */
                    double t1 = 1.0 / x;
                    var c = Helpers.ChebEval(psi_cs, 2.0 * x - 1.0);
                    return -t1 + c;
                }

                /* x = 1 + v */
                return Helpers.ChebEval(psi_cs, 2.0 * x - 3.0);
            }
        }

        #endregion
    }
}
