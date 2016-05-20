namespace YAMP
{
    using System;
    using YAMP.Numerics;

    [Description("RandpFunctionDescription")]
    [Kind(PopularKinds.Random)]
    [Link("RandpFunctionLink")]
    sealed class RandpFunction : ArgumentFunction
    {
        readonly PoissonDistribution Distribution = new PoissonDistribution();

        [Description("RandpFunctionDescriptionForVoid")]
        [Example("randp()", "RandpFunctionExampleForVoid1")]
        public ScalarValue Function()
        {
            return new ScalarValue(Poisson());
        }

        [Description("RandpFunctionDescriptionForScalar")]
        [Example("randp(3)", "RandpFunctionExampleForScalar1")]
        public MatrixValue Function(ScalarValue dim)
        {
            return Function(dim, dim);
        }

        [Description("RandpFunctionDescriptionForScalarScalar")]
        [Example("randp(3, 1)", "RandpFunctionExampleForScalarScalar1")]
        public MatrixValue Function(ScalarValue rows, ScalarValue cols)
        {
            return Function(rows, cols, new ScalarValue(1.0));
        }

        [Description("RandpFunctionDescriptionForScalarScalarScalar")]
        [Example("randp(3, 1, 10)", "RandpFunctionExampleForScalarScalarScalar1")]
        public MatrixValue Function(ScalarValue rows, ScalarValue cols, ScalarValue lambda)
        {
            var n = rows.GetIntegerOrThrowException("rows", Name);
            var l = cols.GetIntegerOrThrowException("cols", Name);
            var m = new MatrixValue(n, l);

            for (var i = 1; i <= l; i++)
            {
                for (var j = 1; j <= n; j++)
                {
                    m[j, i] = new ScalarValue(Poisson(lambda.Re));
                }
            }

            return m;
        }

        Double Poisson()
        {
            return Poisson(1.0);
        }

        Double Poisson(Double lambda)
        {
            Distribution.Lambda = lambda;
            return Distribution.NextDouble();
        }
    }
}
