namespace YAMP.Functions
{
    using YAMP.Exceptions;
    using YAMP.Numerics;

    [Description("PochFunctionDescription")]
    [Kind(PopularKinds.Function)]
    [Link("PochFunctionLink")]
    sealed class PochFunction : ArgumentFunction
    {
        [Description("PochFunctionDescriptionForScalarScalar")]
        [Example("poch(1, 0.5)", "PochFunctionExampleForScalarScalar1")]
        public ScalarValue Function(ScalarValue z, ScalarValue n)
        {
            return Gamma.LinearGamma(z + n) / Gamma.LinearGamma(z);
        }

        [Description("PochFunctionDescriptionForMatrixScalar")]
        [Example("poch([1, 2, 3, 4, 5, 6], 0.5)", "PochFunctionExampleForMatrixScalar1")]
        public MatrixValue Function(MatrixValue Z, ScalarValue n)
        {
            var N = new MatrixValue(Z.DimensionY, Z.DimensionX, n);
            return Function(Z, N);
        }

        [Description("PochFunctionDescriptionForScalarMatrix")]
        [Example("poch(1, [0.1, 0.3, 0.5, 0.7])", "PochFunctionExampleForScalarMatrix1")]
        public MatrixValue Function(ScalarValue z, MatrixValue N)
        {
            var Z = new MatrixValue(N.DimensionY, N.DimensionX, z);
            return Function(Z, N);
        }

        [Description("PochFunctionDescriptionForMatrixMatrix")]
        [Example("poch([1, 2, 3, 4, 5, 6], [0.1, 0.3, 0.5, 0.7, 0.9, 1.1])", "PochFunctionExampleForMatrixMatrix1")]
        public MatrixValue Function(MatrixValue Z, MatrixValue N)
        {
            if (Z.DimensionY != N.DimensionY || Z.DimensionX != N.DimensionX)
                throw new YAMPDifferentDimensionsException(Z, N);

            var M = new MatrixValue(Z.DimensionY, Z.DimensionX);

            for (var i = 1; i <= M.DimensionX; i++)
            {
                for (var j = 1; j <= M.DimensionY; j++)
                {
                    M[j, i] = Gamma.LinearGamma(Z[j, i] + N[j, i]) / Gamma.LinearGamma(Z[j, i]);
                }
            }

            return M;
        }
    }
}
