namespace YAMP
{
    using System;
    using YAMP.Numerics;

    [Description("RandwFunctionDescription")]
    [Kind(PopularKinds.Random)]
    [Link("RandwFunctionLink")]
    sealed class RandwFunction : ArgumentFunction
    {
        readonly WeibullDistribution Distribution = new WeibullDistribution(Rng);

        [Description("RandwFunctionDescriptionForVoid")]
        [Example("randw()", "RandwFunctionExampleForVoid1")]
        public ScalarValue Function()
        {
            return new ScalarValue(Weibull());
        }

        [Description("RandwFunctionDescriptionForScalar")]
        [Example("randw(3)", "RandwFunctionExampleForScalar1")]
        public MatrixValue Function(ScalarValue dim)
        {
            return Function(dim, dim);
        }

        [Description("RandwFunctionDescriptionForScalarScalar")]
        [Example("randw(3, 1)", "RandwFunctionExampleForScalarScalar1")]
        public MatrixValue Function(ScalarValue rows, ScalarValue cols)
        {
            return Function(rows, cols, new ScalarValue(1.0), new ScalarValue(1.0));
        }

        [Description("RandwFunctionDescriptionForScalarScalarScalarScalar")]
        [Example("randw(3, 1, 10, 2.5)", "RandwFunctionExampleForScalarScalarScalarScalar1")]
        public MatrixValue Function(ScalarValue rows, ScalarValue cols, ScalarValue lambda, ScalarValue k)
        {
            var n = rows.GetIntegerOrThrowException("rows", Name);
            var l = cols.GetIntegerOrThrowException("cols", Name);
            var m = new MatrixValue(n, l);

            for (var i = 1; i <= l; i++)
            {
                for (var j = 1; j <= n; j++)
                {
                    m[j, i] = new ScalarValue(Weibull(lambda.Re, k.Re));
                }
            }

            return m;
        }

        Double Weibull()
        {
            return Weibull(1.0, 1.0);
        }

        Double Weibull(Double lambda, Double k)
        {
            Distribution.Lambda = lambda;
            Distribution.Alpha = k;
            return Distribution.NextDouble();
        }
    }
}
