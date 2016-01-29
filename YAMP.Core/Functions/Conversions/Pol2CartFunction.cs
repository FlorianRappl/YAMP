using System;

namespace YAMP
{
    [Description("Converts a vector given in polar coordinates to a cartesian coordinates.")]
    [Kind(PopularKinds.Conversion)]
    sealed class Pol2CartFunction : ArgumentFunction
    {
        [Description("Converts a set of values (r, phi) to a column vector with 2 rows (x, y).")]
        [Example("pol2cart(4, pi/2)", "Computes the cartesian coordinates of the given polar coordinates r = 4, phi = pi / 2.")]
        public MatrixValue Function(ScalarValue r, ScalarValue phi)
        {
            var x = new ScalarValue(r.Re * Math.Cos(phi.Re));
            var y = new ScalarValue(r.Re * Math.Sin(phi.Re));
            return new MatrixValue(new[] { x, y }, 2, 1);
        }

        [Description("Converts a matrix of values (r, phi, in the rows or columns) to a matrix of converted values.")]
        [Example("pol2cart([1, pi/2; 1, pi/3; 1, pi/4; 1, pi/5])", "Evaluates the 4x2 matrix, using the columns as vectors (a set of row vectors to be converted).")]
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
                var r = M[1, i].Re;
                var phi = M[2, i].Re;
                m[1, i] = new ScalarValue(r * Math.Cos(phi));
                m[2, i] = new ScalarValue(r * Math.Sin(phi));
            }

            return isTransposed ? m.Transpose() : m;
        }
    }
}
