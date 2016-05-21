namespace YAMP
{
    using System;
    using YAMP.Numerics;

    [Description("RandrFunctionDescription")]
    [Kind(PopularKinds.Random)]
    [Link("RandrFunctionLink")]
    sealed class RandrFunction : ArgumentFunction
    {
        readonly RayleighDistribution Distribution = new RayleighDistribution(Rng);

        [Description("RandrFunctionDescriptionForVoid")]
        [Example("randr()", "RandrFunctionExampleForVoid1")]
        public ScalarValue Function()
        {
            return new ScalarValue(Rayleigh());
        }

        [Description("RandrFunctionDescriptionForScalar")]
        [Example("randr(3)", "RandrFunctionExampleForScalar1")]
        public MatrixValue Function(ScalarValue dim)
        {
            return Function(dim, dim);
        }

        [Description("RandrFunctionDescriptionForScalarScalar")]
        [Example("randr(3, 1)", "RandrFunctionExampleForScalarScalar1")]
        public MatrixValue Function(ScalarValue rows, ScalarValue cols)
        {
            return Function(rows, cols, new ScalarValue(1.0));
        }

        [Description("RandrFunctionDescriptionForScalarScalarScalar")]
        [Example("randr(3, 1, 10)", "RandrFunctionExampleForScalarScalarScalar1")]
        public MatrixValue Function(ScalarValue rows, ScalarValue cols, ScalarValue sigma)
        {
            var n = rows.GetIntegerOrThrowException("rows", Name);
            var l = cols.GetIntegerOrThrowException("cols", Name);
            var m = new MatrixValue(n, l);

            for (var i = 1; i <= l; i++)
            {
                for (var j = 1; j <= n; j++)
                {
                    m[j, i] = new ScalarValue(Rayleigh(sigma.Re));
                }
            }

            return m;
        }

        Double Rayleigh()
        {
            return Rayleigh(1.0);
        }

        Double Rayleigh(Double sigma)
        {
            Distribution.Sigma = sigma;
            return Distribution.NextDouble();
        }
    }
}
