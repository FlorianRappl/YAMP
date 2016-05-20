namespace YAMP
{
    using YAMP.Numerics;

    [Description("SplineFunctionDescription")]
	[Kind(PopularKinds.Function)]
    [Link("SplineFunctionLink")]
    sealed class SplineFunction : ArgumentFunction
    {
        [Description("SplineFunctionDescriptionForMatrixScalar")]
        [Example("spline([-3,9;-2,4;-1,1;0,0;1,1;3,9], 2)", "SplineFunctionExampleForMatrixScalar1")]
        public MatrixValue Function(MatrixValue original, ScalarValue x)
        {
            var spline = new SplineInterpolation(original);
            var M = new MatrixValue(1, 2);
            M[1, 1].Re = x.Re;
            M[1, 2].Re = spline.ComputeValue(x.Re);
            return M;
        }

        [Description("SplineFunctionDescriptionForMatrixMatrix")]
        [Example("spline([-3,9;-2,4;-1,1;0,0;1,1;3,9], [-1.5, -0.5, 0, 0.5, 1.5])", "SplineFunctionExampleForMatrixMatrix1")]
        public MatrixValue Function(MatrixValue original, MatrixValue x)
        {
            var spline = new SplineInterpolation(original);
            return spline.ComputeValues(x);
        }
    }
}
