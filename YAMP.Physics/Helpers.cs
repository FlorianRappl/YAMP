/*
    Copyright (c) 2012, Florian Rappl.
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
    public static class Helpers
    {
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
    }
}
