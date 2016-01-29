using System;

namespace YAMP
{
    [Description("Converts a vector given in cartesian coordinates to a spherical coordinates.")]
    [Kind(PopularKinds.Conversion)]
    sealed class Cart2SphFunction : ArgumentFunction
    {
        [Description("Converts a set of values (x, y, z) to a column vector with 3 rows (r, phi, theta).")]
        [Example("cart2sph(3, 2, 1)", "Computes the spherical coordinates of the given cartesian coordinates x = 3, y = 2 and z = 1.")]
        public MatrixValue Function(ScalarValue x, ScalarValue y, ScalarValue z)
        {
            var r = Math.Sqrt(x.Re * x.Re + y.Re * y.Re + z.Re * z.Re);
            var phi = Math.Atan2(y.Re, x.Re);
            var theta = Math.Acos(z.Re / r);
            return new MatrixValue(new[] { new ScalarValue(r), new ScalarValue(theta), new ScalarValue(phi) }, 3, 1);
        }

        [Description("Converts a matrix of values (x, y, z in the rows or columns) to a matrix of converted values.")]
        [Example("cart2sph([1, 2, 3; 4, -2, 0; 1, 0, -1; -2, 2, 1])", "Evaluates the 4x3 matrix, using the columns as vectors (a set of row vectors to be converted).")]
        public MatrixValue Function(MatrixValue M)
        {
            if (M.DimensionX != 3 && M.DimensionY != 3)
                throw new YAMPMatrixDimensionException(3, M.DimensionX, M.DimensionY, M.DimensionX);

            var isTransposed = M.DimensionY != 3;

            if (isTransposed)
                M = M.Transpose();

            var m = new MatrixValue(3, M.DimensionX);

            for (var i = 1; i <= M.DimensionX; i++)
            {
                var x = M[1, i].Re;
                var y = M[2, i].Re;
                var z = M[3, i].Re;
                var r = Math.Sqrt(x * x + y * y + z * z);
                var phi = Math.Atan2(y, x);
                var theta = Math.Acos(z / r);
                m[1, i] = new ScalarValue(r);
                m[2, i] = new ScalarValue(theta);
                m[3, i] = new ScalarValue(phi);
            }

            return isTransposed ? m.Transpose() : m;
        }
    }
}
