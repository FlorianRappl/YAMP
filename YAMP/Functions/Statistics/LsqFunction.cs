using System;

namespace YAMP
{
	[Description("The method of least squares is a standard approach to the approximate solution of overdetermined systems, i.e., sets of equations in which there are more equations than unknowns.")]
    [Kind(PopularKinds.Statistic)]
    [Link("http://en.wikipedia.org/wiki/Least_squares")]
    sealed class LsqFunction : ArgumentFunction
	{
		[Description("In statistics and mathematics, linear least squares is an approach to fitting a mathematical or statistical model to data in cases where the idealized value provided by the model for any data point is expressed linearly in terms of the unknown parameters of the model.")]
		[Example("lsq([1,6;2,5;3,7;4,10])", "Computes the slope and the offset for a linear function a * x + b that should fit the points (1, 6), (2, 5), (3, 7) and (4, 10). The result is a = 1.4 and b = 3.5.")]
		public ArgumentsValue Function(MatrixValue M)
		{
			if (M.DimensionX != 2 && M.DimensionY != 2)
				throw new YAMPOperationInvalidException("lsq", "because exactly two rows or columns are required.");

			if (M.DimensionX > M.DimensionY)
				return Function(M.GetSubMatrix(0, 1, 0, M.DimensionX), M.GetSubMatrix(1, 2, 0, M.DimensionX));

			return Function(M.GetSubMatrix(0, M.DimensionY, 0, 1), M.GetSubMatrix(0, M.DimensionY, 1, 2));
		}

		[Description("In statistics and mathematics, linear least squares is an approach to fitting a mathematical or statistical model to data in cases where the idealized value provided by the model for any data point is expressed linearly in terms of the unknown parameters of the model.")]
		[Example("lsq([1, 2, 3, 4], [6, 5, 7, 10])", "Computes the slope and the offset for a linear function a * x + b that should fit the points (1, 6), (2, 5), (3, 7) and (4, 10). The result is a = 1.4 and b = 3.5.")]
		public ArgumentsValue Function(MatrixValue X, MatrixValue Y)
		{
			if (X.Length != Y.Length)
				throw new YAMPDifferentLengthsException(X.Length, Y.Length);

            ScalarValue x1 = new ScalarValue();
            ScalarValue y1 = new ScalarValue();
            ScalarValue xy = new ScalarValue();
            ScalarValue x2 = new ScalarValue();
            ScalarValue slope = new ScalarValue();
            ScalarValue offset = new ScalarValue();

			for (var i = 1; i <= X.Length; i++)
			{
				x1 = x1 + X[i];
				y1 = y1 + Y[i];
				xy = xy + X[i] * Y[i];
				x2 = x2 + X[i] * X[i];
			}

			var J = ((double)X.Length * x2) - (x1 * x1);

			if (J.Re != 0.0)
			{
				slope = (((double)X.Length * xy) - (x1 * y1)) / J.Re;
				offset = ((y1 * x2) - (x1 * xy)) / J.Re;
			}

			return new ArgumentsValue(slope, offset);
		}
	}
}