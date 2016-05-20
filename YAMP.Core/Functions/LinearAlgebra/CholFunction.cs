namespace YAMP
{
    using YAMP.Numerics;

    [Kind(PopularKinds.Function)]
    [Link("CholFunctionLink")]
    [Description("CholFunctionDescription")]
    sealed class CholFunction : ArgumentFunction
	{
		[Description("CholFunctionDescriptionForMatrix")]
		[Example("chol([4, 12, -16; 12, 37, -43; -16, -43, 98])", "CholFunctionExampleForMatrix1")]
		public MatrixValue Function(MatrixValue M)
		{
			var chol = new CholeskyDecomposition(M);
            return chol.GetL();
		}
	}
}
