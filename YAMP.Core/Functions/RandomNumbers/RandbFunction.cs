namespace YAMP
{
    using System;
    using YAMP.Numerics;

    [Description("RandbFunctionDescription")]
    [Kind(PopularKinds.Random)]
    [Link("RandbFunctionLink")]
    sealed class RandbFunction : ArgumentFunction
    {
        readonly BinomialDistribution Distribution = new BinomialDistribution(Rng);

        [Description("RandbFunctionDescriptionForVoid")]
        [Example("randw()", "RandbFunctionExampleForVoid1")]
        public ScalarValue Function()
        {
            return new ScalarValue(Binomial());
        }

        [Description("RandbFunctionDescriptionForScalar")]
        [Example("randw(3)", "RandbFunctionExampleForVoid1")]
        public MatrixValue Function(ScalarValue dim)
        {
            return Function(dim, dim);
        }

        [Description("RandbFunctionDescriptionForScalarScalar")]
        [Example("randw(3, 1)", "RandbFunctionExampleForScalarScalar1")]
        public MatrixValue Function(ScalarValue rows, ScalarValue cols)
        {
            return Function(rows, cols, new ScalarValue(0.5), new ScalarValue(1.0));
        }

        [Description("RandbFunctionDescriptionForScalarScalarScalarScalar")]
        [Example("randw(3, 1, 0.5, 20)", "RandbFunctionExampleForScalarScalarScalarScalar1")]
        public MatrixValue Function(ScalarValue rows, ScalarValue cols, ScalarValue p, ScalarValue n)
        {
            var k = rows.GetIntegerOrThrowException("rows", Name);
            var l = cols.GetIntegerOrThrowException("cols", Name);
            var nn = n.GetIntegerOrThrowException("n", Name);
            var m = new MatrixValue(k, l);

            for (var i = 1; i <= l; i++)
            {
                for (var j = 1; j <= k; j++)
                {
                    m[j, i] = new ScalarValue(Binomial(p.Re, nn));
                }
            }

            return m;
        }

        Double Binomial()
        {
            return Binomial(0.5, 1);
        }

        Double Binomial(Double p, Int32 n)
        {
            Distribution.Alpha = p;
            Distribution.Beta = n;
            return Distribution.NextDouble();
        }
    }
}
