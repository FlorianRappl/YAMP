namespace YAMP
{
    using YAMP.Numerics;

    [Description("CGFunctionDescription")]
	[Kind(PopularKinds.Function)]
    [Link("CGFunctionLink")]
    sealed class CGFunction : ArgumentFunction
    {
        [Description("CGFunctionDescriptionForMatrixMatrix")]
        [Example("cg([1,2,1;2,3,4;1,4,2], rand(3,1))", "CGFunctionExampleForMatrixMatrix1")]
        public MatrixValue Function(MatrixValue A, MatrixValue b)
        {
            var cg = new CGSolver(A);
            return cg.Solve(b);
        }

        [Description("CGFunctionDescriptionForMatrixMatrixMatrix")]
        [Example("cg([1,2,1;2,3,4;1,4,2], [1;0;0], rand(3,1))", "CGFunctionExampleForMatrixMatrixMatrix1")]
        public MatrixValue Function(MatrixValue A, MatrixValue x, MatrixValue b)
        {
            var cg = new CGSolver(A);
            cg.X0 = x;
            return cg.Solve(b);
        }
    }
}
