namespace YAMP.Physics
{
    using System;
    using YAMP.Exceptions;

    [Description("In mathematics, the Carlson symmetric forms of elliptic integrals are a small canonical set of elliptic integrals to which all others may be reduced. They are a modern alternative to the Legendre forms. The Legendre forms may be expressed in terms of the Carlson forms and vice versa. This function computes the elliptic integral named R_D(x, y, z).")]
    [Kind(PopularKinds.Function)]
    class CarlsonDFunction : ArgumentFunction
    {
        [Description("Computes the value of the funtion R_D(x, y, z) by solving the elliptic integral for the real arguments x, y, z.")]
        [Example("carlsond(1, 2.5, 1.5)", "Evaluates R_D at x = 1, y = 2.5 and z = 1.5.")]
        public ScalarValue Function(ScalarValue x, ScalarValue y, ScalarValue z)
        {
            return new ScalarValue(CarlsonD(x.Re, y.Re, z.Re));
        }

        #region Algorithm

        public static double CarlsonD(Double x, Double y, Double z)
        {
            if (x < 0.0)
                throw new YAMPArgumentRangeException("x", 0.0);

            if (y < 0.0)
                throw new YAMPArgumentRangeException("y", 0.0);

            if (z < 0.0)
                throw new YAMPArgumentRangeException("z", 0.0);

            if ((x == 0.0) && (y == 0.0))
                return Double.PositiveInfinity;

            // variable to hold the sum of the second terms
            var t = 0.0; 
            var c4 = Math.Pow(2.0, 2.0 / 3.0);

            for (int n = 0; n < 250; n++)
            {
                // find out how close we are to the expansion point
                var m = (x + y + 3.0 * z) / 5.0;
                var dx = (x - m) / m;
                var dy = (y - m) / m;
                var dz = (z - m) / m;
                double e = Math.Max(Math.Abs(dx), Math.Max(Math.Abs(dy), Math.Abs(dz)));

                // Our series development (DLMF 19.36.2) goes up to O(e^6). In order that the neglected term e^7 <~ 1.0E-16, we need e <~ 0.005.
                if (e < 0.005)
                {
                    var xy = dx * dy; 
                    var zz = dz * dz;
                    var E2 = xy - 6.0 * zz;
                    var E3 = (3.0 * xy - 8.0 * zz) * dz;
                    var E4 = 3.0 * (xy - zz) * zz;
                    var E5 = xy * zz * dz;
                    var F = 1.0 - 3.0 / 14.0 * E2 - E3 / 6.0 + 9.0 / 88.0 * E2 * E2 - 3.0 / 22.0 * E4 + 9.0 / 52.0 * E2 * E3 - 3.0 / 26.0 * E5
                        - E2 * E2 * E2 / 16.0 + 3.0 / 40.0 * E3 * E3 + 3.0 / 20.0 * E2 * E4;

                    return F / Math.Pow(m, 3.0 / 2.0) + t;
                }

                // we are not close enough; use the duplication theory to move us closer
                var lambda = Math.Sqrt(x * y) + Math.Sqrt(x * z) + Math.Sqrt(y * z);

                t += 3.0 / Math.Sqrt(z) / (z + lambda);
                x = (x + lambda) / c4;
                y = (y + lambda) / c4;
                z = (z + lambda) / c4;
            }

            throw new YAMPNotConvergedException("CarlsonD");
        }

        #endregion
    }
}
