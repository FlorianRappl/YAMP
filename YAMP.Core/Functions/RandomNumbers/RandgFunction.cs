namespace YAMP
{
    using System;
    using YAMP.Numerics;

    [Description("RandgFunctionDescription")]
    [Kind(PopularKinds.Random)]
    [Link("RandgFunctionLink")]
    sealed class RandgFunction : ArgumentFunction
    {
        readonly GammaDistribution Distribution = new GammaDistribution(Rng);

        [Description("RandgFunctionDescriptionForVoid")]
        [Example("randg()", "RandgFunctionExampleForVoid1")]
        public ScalarValue Function()
        {
            return new ScalarValue(Gamma());
        }

        [Description("RandgFunctionDescriptionForScalar")]
        [Example("randg(3)", "RandgFunctionExampleForScalar1")]
        public MatrixValue Function(ScalarValue dim)
        {
            return Function(dim, dim);
        }

        [Description("RandgFunctionDescriptionForScalarScalar")]
        [Example("randg(3, 1)", "RandgFunctionExampleForScalarScalar1")]
        public MatrixValue Function(ScalarValue rows, ScalarValue cols)
        {
            return Function(rows, cols, new ScalarValue(1.0), new ScalarValue(1.0));
        }

        [Description("RandgFunctionDescriptionForScalarScalarScalarScalar")]
        [Example("randg(3, 1, 10, 2.5)", "RandgFunctionExampleForScalarScalarScalarScalar1")]
        public MatrixValue Function(ScalarValue rows, ScalarValue cols, ScalarValue theta, ScalarValue k)
        {
            var n = rows.GetIntegerOrThrowException("rows", Name);
            var l = cols.GetIntegerOrThrowException("cols", Name);
            var m = new MatrixValue(n, l);

            for (var i = 1; i <= l; i++)
            {
                for (var j = 1; j <= n; j++)
                {
                    m[j, i] = new ScalarValue(Gamma(theta.Re, k.Re));
                }
            }

            return m;
        }

        Double Gamma()
        {
            return Gamma(1.0, 1.0);
        }

        Double Gamma(Double theta, Double k)
        {
            Distribution.Theta = theta;
            Distribution.Alpha = k;
            return Distribution.NextDouble();
        }
    }
}
