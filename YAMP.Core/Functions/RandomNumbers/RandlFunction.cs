namespace YAMP
{
    using System;
    using YAMP.Numerics;

    [Description("RandlFunctionDescription")]
    [Kind(PopularKinds.Random)]
    [Link("RandlFunctionLink")]
    sealed class RandlFunction : ArgumentFunction
    {
        readonly LaplaceDistribution Distribution = new LaplaceDistribution();

        [Description("RandlFunctionDescriptionForVoid")]
        [Example("randl()", "RandlFunctionExampleForVoid1")]
        public ScalarValue Function()
        {
            return new ScalarValue(Laplace());
        }

        [Description("RandlFunctionDescriptionForScalar")]
        [Example("randl(3)", "RandlFunctionExampleForScalar1")]
        public MatrixValue Function(ScalarValue dim)
        {
            return Function(dim, dim);
        }

        [Description("RandlFunctionDescriptionForScalarScalar")]
        [Example("randl(3, 1)", "RandlFunctionExampleForScalarScalar1")]
        public MatrixValue Function(ScalarValue rows, ScalarValue cols)
        {
            return Function(rows, cols, new ScalarValue(), new ScalarValue(1.0));
        }

        [Description("RandlFunctionDescriptionForScalarScalarScalarScalar")]
        [Example("randl(3, 1, 10, 2.5)", "RandlFunctionExampleForScalarScalarScalarScalar1")]
        public MatrixValue Function(ScalarValue rows, ScalarValue cols, ScalarValue mu, ScalarValue b)
        {
            var n = rows.GetIntegerOrThrowException("rows", Name);
            var l = cols.GetIntegerOrThrowException("cols", Name);
            var m = new MatrixValue(n, l);

            for (var i = 1; i <= l; i++)
            {
                for (var j = 1; j <= n; j++)
                {
                    m[j, i] = new ScalarValue(Laplace(mu.Re, b.Re));
                }
            }

            return m;
        }

        Double Laplace()
        {
            return Laplace(0.0, 1.0);
        }

        Double Laplace(Double mu, Double b)
        {
            Distribution.Mu = mu;
            Distribution.Alpha = b;
            return Distribution.NextDouble();
        }
    }
}
