using System;

namespace YAMP
{
    [Description("The sphere function generates the x-, y-, and z-coordinates of a unit sphere for use with surf and mesh.")]
    [Kind(PopularKinds.Function)]
    sealed class SphereFunction : ArgumentFunction
    {
        [Description("Draws a surf plot of an 20-by-20 sphere in the current figure.")]
        [Example("[X, Y, Z] = sphere()", "Returns the three matrices with x-, y-, and Z coordinates for a unit sphere. The matrices are saved in the variables X, Y, Z.")]
        public ArgumentsValue Function()
        {
            return Function(new ScalarValue(20));
        }

        [Description("Draws a surf plot of an n-by-n sphere in the current figure.")]
        [Example("[X, Y, Z] = sphere(30)", "Returns the three matrices with x-, y-, and Z coordinates for a unit sphere with n = 30. The 31x31 matrices are saved in the variables X, Y, Z.")]
        public ArgumentsValue Function(ScalarValue n)
        {
            var nn = n.GetIntegerOrThrowException("n", Name);
            int dim = nn + 1;

            if (dim < 2)
                throw new YAMPArgumentRangeException("n", 1.0);

            var X = new MatrixValue(dim, dim); // x = sin(phi) * cos(theta)
            var Y = new MatrixValue(dim, dim); // y = sin(phi) * sin(theta)
            var Z = new MatrixValue(dim, dim); // z = cos(phi)

            var stheta = Table(0.0, 2.0 * Math.PI, dim, Math.Sin);
            var ctheta = Table(0.0, 2.0 * Math.PI, dim, Math.Cos);
            var sphi = Table(0.0, Math.PI, dim, Math.Sin);
            var cphi = Table(0.0, Math.PI, dim, Math.Cos);

            for (var j = 0; j < dim; j++)
            {
                var col = j + 1;

                for (var i = 0; i < dim; i++)
                {
                    var row = i + 1;

                    X[row, col] = new ScalarValue(sphi[j] * ctheta[i]);
                    Y[row, col] = new ScalarValue(sphi[j] * stheta[i]);
                    Z[row, col] = new ScalarValue(cphi[j]);
                }
            }

            return new ArgumentsValue(X, Y, Z);
        }

        double[] Table(double s, double e, int n, Func<double, double> f)
        {
            var a = new double[n];
            var c = s;
            var d = (e - s) / (n - 1);

            for (var i = 0; i < n; i++)
            {
                a[i] = f(c);
                c += d;
            }

            return a;
        }
    }
}
