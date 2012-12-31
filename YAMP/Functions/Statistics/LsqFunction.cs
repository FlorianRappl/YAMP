﻿using System;

namespace YAMP
{
	[Description("The method of least squares is a standard approach to the approximate solution of overdetermined systems, i.e., sets of equations in which there are more equations than unknowns.")]
	[Kind(PopularKinds.Function)]
	class LsqFunction : ArgumentFunction
	{
		[Description("In statistics and mathematics, linear least squares is an approach to fitting a mathematical or statistical model to data in cases where the idealized value provided by the model for any data point is expressed linearly in terms of the unknown parameters of the model.")]
		[Example("lsq([1,6;2,5;3,7;4,10])", "Computes the slope and the offset for a linear function a * x + b that should fit the points (1, 6), (2, 5), (3, 7) and (4, 10). The result is a = 1.4 and b = 3.5.")]
		public ArgumentsValue Function(MatrixValue M)
		{
			if (M.DimensionX != 2 && M.DimensionY != 2)
				throw new OperationNotSupportedException("lsq", "because exactly two rows or columns are required.");

			if (M.DimensionX > M.DimensionY)
				return Function(M.SubMatrix(0, 1, 0, M.DimensionX), M.SubMatrix(1, 2, 0, M.DimensionX));

			return Function(M.SubMatrix(0, M.DimensionY, 0, 1), M.SubMatrix(0, M.DimensionY, 1, 2));
		}

		[Description("In statistics and mathematics, linear least squares is an approach to fitting a mathematical or statistical model to data in cases where the idealized value provided by the model for any data point is expressed linearly in terms of the unknown parameters of the model.")]
		[Example("lsq([1, 2, 3, 4], [6, 5, 7, 10])", "Computes the slope and the offset for a linear function a * x + b that should fit the points (1, 6), (2, 5), (3, 7) and (4, 10). The result is a = 1.4 and b = 3.5.")]
		public ArgumentsValue Function(MatrixValue X, MatrixValue Y)
		{
			if (X.Length != Y.Length)
				throw new DimensionException(X.Length, Y.Length);

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

			var J = ((double)X.Length * x2) - (x1 * x1);

			if (J.Value != 0.0)
			{
				slope = (((double)X.Length * xy) - (x1 * y1)) / J.Value;
				offset = ((y1 * x2) - (x1 * xy)) / J.Value;
			}

			return new ArgumentsValue(slope, offset);
		}
	}
}