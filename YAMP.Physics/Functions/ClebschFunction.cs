using System;
using YAMP;

namespace YAMP.Physics
{
    [Description("In physics, the Clebsch–Gordan coefficients are sets of numbers that arise in angular momentum coupling under the laws of quantum mechanics. In more mathematical terms, the CG coefficients are used in representation theory, particularly of compact Lie groups, to perform the explicit direct sum decomposition of the tensor product of two irreducible representations into irreducible representations, in cases where the numbers and types of irreducible components are already known abstractly. The name derives from the German mathematicians Alfred Clebsch (1833–1872) and Paul Gordan (1837–1912), who encountered an equivalent problem in invariant theory.")]
    [Kind(PopularKinds.Function)]
    class ClebschFunction : ArgumentFunction
    {
        [Description("Computes the Clebsch-Gordan coefficients for two spins j1 and j2 and returns a matrix M with column values m (Column 1), m1 (Column 2), m2 (Column 3), j (Column 4) and the resulting value (Column 5 - last column).")]
        [Example("clebsch(0.5, 0.5)", "Computes the Clebsch-Gordan coeff. for two spin 1/2 particles (e.g. electrons), which results in a 6 x 5 matrix that lists all possible combinations with their respective values.")]
        public MatrixValue Function(ScalarValue j1, ScalarValue j2)
        {
            if (isNotHalf(j1))
                throw new ArgumentException("0, +-0.5, +-1, +-1.5, ...", j1.Value.ToString());

            if (isNotHalf(j2))
                throw new ArgumentException("0, +-0.5, +-1, +-1.5, ...", j2.Value.ToString());

            var l = 1;
            var M = new MatrixValue();

            for (var m1 = -j1.Value; m1 <= j1.Value; m1 += 1.0)
            {
                for (var m2 = -j2.Value; m2 <= j2.Value; m2 += 1.0)
                {
                    var m = m1 + m2;
                    var ja = j1.Value + j2.Value;

                    for (var j = Math.Abs(m); j <= ja; j += 1.0)
                    {
                        var v = Coeff(j1.Value, j2.Value, j, m1, m2);
                        M[l, 1] = new ScalarValue(m);
                        M[l, 2] = new ScalarValue(m1);
                        M[l, 3] = new ScalarValue(m2);
                        M[l, 4] = new ScalarValue(j);
                        M[l, 5] = new ScalarValue(v);
                        l++;
                    }
                }
            }

            return M;
        }

        double Coeff(double j1, double j2, double j, double m1, double m2)
        {
            if (j2 == 0.0)
                return (m2 == 0.0 && j1 == j) ? 1.0 : 0.0;

            double m = m1 + m2;

            if(m < 0)
                return sign(j, j1, j2) * Coeff(j1, j2, j, -m1, -m2);

            if(j1 < j2)
                return sign(j, j1, j2) * Coeff(j2, j1, j, m1, m2);

            var f1 = factorial(1 + j1 + j2 + j) / (factorial(j1 + j2 - j) * factorial(j1 - j2 + j) * factorial(j - j1 + j2));
            var f2 = factorial(j1 + m1) * factorial(j1 - m1) * factorial(j + m) * factorial(j - m) * (2.0 * j + 1.0);
            var f3 = factorial(j2 + m2) * factorial(j2 - m2);
            var f4 = 0.0;
            var kmax = Math.Min(j - m, j1 - m1);

            for (var k = 0.0; k <= kmax; k += 1.0)
            {
                var a = factorial(j1 + j2 - m - k) * factorial(j2 + j - m1 - k);
                var b = factorial(k) * factorial(j1 - m1 - k) * factorial(j - m - k) * factorial(j1 + j2 + j + 1 - k);
                f4 += Math.Pow(-1, j1 - m1 + k) * a / b;
            }

            return Math.Sqrt(f1 * f2 / f3) * f4;
        }

        double sign(double j, double j1, double j2)
        {
            return Math.Pow(-1.0, j - j1 - j2);
        }

        double factorial(double arg)
        {
            var res = 1.0;

            while (arg > 1)
                res *= arg--;

            return res;
        }

        bool isNotHalf(ScalarValue j)
        {
            return Math.IEEERemainder(j.Value, 0.5) != 0.0;
        }
    }
}
