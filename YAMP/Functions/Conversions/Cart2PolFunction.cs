using System;

namespace YAMP
{
    [Description("Converts a vector given in cartesian coordinates to a polar coordinates.")]
    [Kind(PopularKinds.Conversion)]
    sealed class Cart2PolFunction : ArgumentFunction
    {
        [Description("Converts a set of values (x, y) to a column vector with 2 rows (r, phi).")]
        [Example("cart2pol(3, 2)", "Computes the polar coordinates of the given cartesian coordinates x = 3, y = 2.")]
        public MatrixValue Function(ScalarValue x, ScalarValue y)
        {
            var phi = Math.Atan2(y.Re, x.Re);
            var r = x.Re == 0.0 ? y.Re : (y.Re == 0.0 ? x.Re : x.Re / Math.Cos(phi));
            return new MatrixValue(new[] { new ScalarValue(r), new ScalarValue(phi) }, 2, 1);
        }

        [Description("Converts a matrix of values (x, y in the rows or columns) to a matrix of converted values.")]
        [Example("cart2pol([1, 2; 4, -2; 1, 0; -2, 2])", "Evaluates the 4x2 matrix, using the columns as vectors (a set of row vectors to be converted).")]
        public MatrixValue Function(MatrixValue M)
        {
            if (M.DimensionX != 2 && M.DimensionY != 2)
                throw new YAMPMatrixDimensionException(2, M.DimensionX, M.DimensionY, M.DimensionX);

            var isTransposed = M.DimensionY != 2;

            if (isTransposed)
                M = M.Transpose();

            var m = new MatrixValue(2, M.DimensionX);

            for (var i = 1; i <= M.DimensionX; i++)
            {
                var x = M[1, i].Re;
                var y = M[2, i].Re;
                var phi = Math.Atan2(y, x);
                var r = x == 0.0 ? y : (y == 0.0 ? x : x / Math.Cos(phi));
                m[1, i] = new ScalarValue(r * Math.Cos(phi));
                m[2, i] = new ScalarValue(r * Math.Sin(phi));
            }

            return isTransposed ? m.Transpose() : m;
        }
    }
}
