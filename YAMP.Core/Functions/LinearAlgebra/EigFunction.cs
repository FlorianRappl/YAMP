namespace YAMP
{
    using YAMP.Numerics;

    [Description("EigFunctionDescription")]
    [Kind(PopularKinds.Function)]
    [Link("EigFunctionLink")]
    sealed class EigFunction : ArgumentFunction
	{
		[Description("EigFunctionDescriptionForMatrix")]
        [Example("eig([1,2,3;4,5,6;7,8,9])", "EigFunctionExampleForMatrix1")]
        [Example("[vals, vecs] = eig([1,2,3;4,5,6;7,8,9])", "EigFunctionExampleForMatrix2")]
		[Returns(typeof(MatrixValue), "EigFunctionReturnForMatrix1", 0)]
        [Returns(typeof(MatrixValue), "EigFunctionReturnForMatrix2", 1)]
		public ArgumentsValue Function(MatrixValue M)
		{
			var ev = new Eigenvalues(M as MatrixValue);
			var m = new MatrixValue(ev.RealEigenvalues.Length, 1);
			var n = ev.GetV();

            for (var i = 1; i <= ev.RealEigenvalues.Length; i++)
            {
                m[i, 1] = new ScalarValue(ev.RealEigenvalues[i - 1], ev.ImagEigenvalues[i - 1]);
            }

			return new ArgumentsValue(m, n);
		}
	}
}
