namespace YAMP
{
    using System;
    using YAMP.Exceptions;

	[Description("LsqFunctionDescription")]
    [Kind(PopularKinds.Statistic)]
    [Link("LsqFunctionLink")]
    sealed class LsqFunction : ArgumentFunction
	{
		[Description("LsqFunctionDescriptionForMatrix")]
		[Example("lsq([1,6;2,5;3,7;4,10])", "LsqFunctionExampleForMatrix1")]
		public ArgumentsValue Function(MatrixValue M)
		{
			if (M.DimensionX != 2 && M.DimensionY != 2)
				throw new YAMPOperationInvalidException("lsq", "because exactly two rows or columns are required.");

			if (M.DimensionX > M.DimensionY)
				return Function(M.GetSubMatrix(0, 1, 0, M.DimensionX), M.GetSubMatrix(1, 2, 0, M.DimensionX));

			return Function(M.GetSubMatrix(0, M.DimensionY, 0, 1), M.GetSubMatrix(0, M.DimensionY, 1, 2));
		}

		[Description("LsqFunctionDescriptionForMatrixMatrix")]
		[Example("lsq([1, 2, 3, 4], [6, 5, 7, 10])", "LsqFunctionExampleForMatrixMatrix1")]
		public ArgumentsValue Function(MatrixValue X, MatrixValue Y)
		{
			if (X.Length != Y.Length)
				throw new YAMPDifferentLengthsException(X.Length, Y.Length);

            var x1 = new ScalarValue();
            var y1 = new ScalarValue();
            var xy = new ScalarValue();
            var x2 = new ScalarValue();
            var slope = new ScalarValue();
            var offset = new ScalarValue();

			for (var i = 1; i <= X.Length; i++)
			{
				x1 = x1 + X[i];
				y1 = y1 + Y[i];
				xy = xy + X[i] * Y[i];
				x2 = x2 + X[i] * X[i];
			}

			var J = ((Double)X.Length * x2) - (x1 * x1);

			if (J.Re != 0.0)
			{
                slope = (((Double)X.Length * xy) - (x1 * y1)) / J.Re;
				offset = ((y1 * x2) - (x1 * xy)) / J.Re;
			}

			return new ArgumentsValue(slope, offset);
		}
	}
}