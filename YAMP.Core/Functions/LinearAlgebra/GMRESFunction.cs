namespace YAMP
{
    using YAMP.Numerics;

    [Description("GMRESFunctionDescription")]
    [Kind(PopularKinds.Function)]
    [Link("GMRESFunctionLink")]
    sealed class GMRESFunction : ArgumentFunction
    {
        [Description("GMRESFunctionDescriptionForMatrixMatrix")]
        [Example("gmres(rand(3), rand(3,1))", "GMRESFunctionExampleForMatrixMatrix1")]
        public MatrixValue Function(MatrixValue A, MatrixValue b)
        {
            var gmres = new GMRESkSolver(A);
            return gmres.Solve(b);
        }

        [Description("GMRESFunctionDescriptionForMatrixMatrixMatrix")]
        [Example("gmres(rand(3), [1;0;0], rand(3,1))", "GMRESFunctionExampleForMatrixMatrixMatrix1")]
        public MatrixValue Function(MatrixValue A, MatrixValue x, MatrixValue b)
        {
            var gmres = new GMRESkSolver(A);
            gmres.X0 = x;
            return gmres.Solve(b);
        }
    }
}
