namespace YAMP
{
    using YAMP.Numerics;

    [Description("EigValFunctionDescription")]
	[Kind(PopularKinds.Function)]
    sealed class EigValFunction : ArgumentFunction
	{
		[Description("EigValFunctionDescriptionForMatrix")]
		[Example("eigval([1,2,3;4,5,6;7,8,9])", "EigValFunctionExampleForMatrix1")]
		public MatrixValue Function(MatrixValue M)
		{
			var ev = new Eigenvalues(M as MatrixValue);
			var m = new MatrixValue(ev.RealEigenvalues.Length, 1);

            for (var i = 1; i <= ev.RealEigenvalues.Length; i++)
            {
                m[i, 1] = new ScalarValue(ev.RealEigenvalues[i - 1], ev.ImagEigenvalues[i - 1]);
            }

			return m;
		}
	}
}
