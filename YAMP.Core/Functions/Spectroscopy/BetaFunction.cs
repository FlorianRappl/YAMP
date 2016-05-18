namespace YAMP
{
    using YAMP.Exceptions;
    using YAMP.Numerics;

    [Description("BetaFunctionDescription")]
    [Kind(PopularKinds.Function)]
    [Link("BetaFunctionLink")]
    sealed class BetaFunction : ArgumentFunction
    {
        [Description("BetaFunctionDescriptionForScalarScalar")]
        [Example("beta(5, 3)", "BetaFunctionExampleForScalarScalar1")]
        public ScalarValue Function(ScalarValue z, ScalarValue w)
        {
            return new ScalarValue(Gamma.Beta(z.Re, w.Re));
        }

        [Description("BetaFunctionDescriptionForMatrixMatrix")]
        [Example("beta(1:5, 3:7)", "BetaFunctionExampleForMatrixMatrix1")]
        public MatrixValue Function(MatrixValue Z, MatrixValue W)
        {
            if (Z.DimensionX != W.DimensionX || Z.DimensionY != W.DimensionY)
                throw new YAMPDifferentDimensionsException(Z.DimensionY, Z.DimensionX, W.DimensionY, W.DimensionX);

            var M = new MatrixValue(Z.DimensionY, Z.DimensionX);

            for (var i = 1; i <= Z.DimensionX; i++)
            {
                for (var j = 1; j <= Z.DimensionY; j++)
                {
                    M[j, i] = Function(Z[j, i], W[j, i]);
                }
            }

            return M;
        }

        [Description("BetaFunctionDescriptionForMatrixScalar")]
        [Example("beta(1:10, 3)", "BetaFunctionExampleForMatrixScalar1")]
        public MatrixValue Function(MatrixValue Z, ScalarValue w)
        {
            var M = new MatrixValue(Z.DimensionY, Z.DimensionX);

            for (var i = 1; i <= Z.DimensionX; i++)
            {
                for (var j = 1; j <= Z.DimensionY; j++)
                {
                    M[j, i] = Function(Z[j, i], w);
                }
            }

            return M;
        }

        [Description("BetaFunctionDescriptionForScalarMatrix")]
        [Example("beta(2, 1:10)", "BetaFunctionExampleForScalarMatrix1")]
        public MatrixValue Function(ScalarValue z, MatrixValue W)
        {
            return Function(W, z);
        }
    }
}
