namespace YAMP
{
    using YAMP.Numerics;

    [Kind(PopularKinds.Function)]
    [Link("SvdFunctionLink")]
	[Description("SvdFunctionDescription")]
    sealed class SvdFunction : ArgumentFunction
	{
		[Description("SvdFunctionDescriptionForMatrix")]
		[Example("svd([1, 0, 0, 0, 2; 0, 0, 3, 0, 0; 0, 0, 0, 0, 0; 0, 4, 0, 0, 0])", "SvdFunctionExampleForMatrix1")]
		[Example("[S, U, V] = svd([1, 0, 0, 0, 2; 0, 0, 3, 0, 0; 0, 0, 0, 0, 0; 0, 4, 0, 0, 0])", "SvdFunctionExampleForMatrix2")]
		[Returns(typeof(MatrixValue), "SvdFunctionReturnForMatrix1", 0)]
        [Returns(typeof(MatrixValue), "SvdFunctionReturnForMatrix2", 1)]
        [Returns(typeof(MatrixValue), "SvdFunctionReturnForMatrix3", 2)]
		public ArgumentsValue Function(MatrixValue M)
		{
			var svd = new SingularValueDecomposition(M);
			return new ArgumentsValue(svd.S, svd.GetU(), svd.GetV());
		}
	}
}
