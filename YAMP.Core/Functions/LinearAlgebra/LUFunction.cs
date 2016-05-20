namespace YAMP
{
    using YAMP.Numerics;

    [Kind(PopularKinds.Function)]
    [Link("LUFunctionLink")]
	[Description("LUFunctionDescription")]
    sealed class LUFunction : ArgumentFunction
	{
		[Description("LUFunctionDescriptionForMatrix")]
		[Example("lu([4, 3; 6, 3])", "LUFunctionExampleForMatrix1")]
		[Example("[L, U, P] = lu([4, 3; 6, 3])", "LUFunctionExampleForMatrix2")]
		[Returns(typeof(MatrixValue), "LUFunctionReturnForMatrix1", 0)]
		[Returns(typeof(MatrixValue), "LUFunctionReturnForMatrix2", 1)]
		[Returns(typeof(MatrixValue), "LUFunctionReturnForMatrix3", 2)]
		public ArgumentsValue Function(MatrixValue M)
		{
			var lu = new LUDecomposition(M);
			return new ArgumentsValue(lu.L, lu.U, lu.Pivot);
		}
	}
}
