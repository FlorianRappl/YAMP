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
