namespace YAMP
{
    [Description("MeshgridFunctionDescription")]
    [Kind(PopularKinds.Function)]
    sealed class MeshgridFunction : ArgumentFunction
    {
        [Description("MeshgridFunctionDescriptionForMatrix")]
        [Returns(typeof(MatrixValue), "MeshgridFunctionReturnForMatrix1", 0)]
        [Returns(typeof(MatrixValue), "MeshgridFunctionReturnForMatrix2", 1)]
        public ArgumentsValue Function(MatrixValue x)
        {
            return Function(x, x);
        }

        [Description("MeshgridFunctionDescriptionForMatrixMatrix")]
        [Example("meshgrid(1:3, 10:14)", "MeshgridFunctionExampleForMatrixMatrix1")]
        [Returns(typeof(MatrixValue), "MeshgridFunctionReturnForMatrixMatrix1", 0)]
        [Returns(typeof(MatrixValue), "MeshgridFunctionReturnForMatrixMatrix2", 1)]
        public ArgumentsValue Function(MatrixValue x, MatrixValue y)
        {
            var M = x.Length;
            var N = y.Length;
            var X = new MatrixValue(N, M);
            var Y = new MatrixValue(N, M);

            for (var i = 1; i <= N; i++)
            {
                for (var j = 1; j <= M; j++)
                {
                    X[i, j] = x[j].Clone();
                }
            }

            for (var i = 1; i <= N; i++)
            {
                for (var j = 1; j <= M; j++)
                {
                    Y[i, j] = y[i].Clone();
                }
            }

            return new ArgumentsValue(X, Y);
        }
    }
}
