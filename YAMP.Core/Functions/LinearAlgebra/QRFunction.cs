namespace YAMP
{
    using YAMP.Numerics;

    [Kind(PopularKinds.Function)]
    [Link("QRFunctionLink")]
	[Description("QRFunctionDescription")]
    sealed class QRFunction : ArgumentFunction
	{
		[Description("QRFunctionDescriptionForMatrix")]
		[Example("qr([12, -51, 4; 6, 167, -68; -4, 24, -41])", "QRFunctionExampleForMatrix1")]
		[Example("[Q, R] = qr([12, -51, 4; 6, 167, -68; -4, 24, -41])", "QRFunctionExampleForMatrix2")]
		[Returns(typeof(MatrixValue), "QRFunctionReturnForMatrix1", 0)]
		[Returns(typeof(MatrixValue), "QRFunctionReturnForMatrix2", 1)]
		public ArgumentsValue Function(MatrixValue M)
		{
			var qr = QRDecomposition.Create(M);
			return new ArgumentsValue(qr.Q, qr.R);
		}
	}
}
