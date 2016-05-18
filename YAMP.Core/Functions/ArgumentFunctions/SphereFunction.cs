namespace YAMP
{
    using System;
    using YAMP.Exceptions;

    [Description("SphereFunctionDescription")]
    [Kind(PopularKinds.Function)]
    sealed class SphereFunction : ArgumentFunction
    {
        [Description("SphereFunctionDescriptionForVoid")]
        [Example("[X, Y, Z] = sphere()", "SphereFunctionExampleForVoid1")]
        public ArgumentsValue Function()
        {
            return Function(new ScalarValue(20));
        }

        [Description("SphereFunctionDescriptionForScalar")]
        [Example("[X, Y, Z] = sphere(30)", "SphereFunctionExampleForScalar1")]
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

        static Double[] Table(Double s, Double e, Int32 n, Func<Double, Double> f)
        {
            var a = new Double[n];
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
