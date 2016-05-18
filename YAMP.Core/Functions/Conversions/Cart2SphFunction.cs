namespace YAMP
{
    using System;
    using YAMP.Exceptions;

    [Description("Cart2SphFunctionDescription")]
    [Kind(PopularKinds.Conversion)]
    sealed class Cart2SphFunction : ArgumentFunction
    {
        [Description("Cart2SphFunctionDescriptionForScalarScalarScalar")]
        [Example("cart2sph(3, 2, 1)", "Cart2SphFunctionExampleForScalarScalarScalar1")]
        public MatrixValue Function(ScalarValue x, ScalarValue y, ScalarValue z)
        {
            var r = Math.Sqrt(x.Re * x.Re + y.Re * y.Re + z.Re * z.Re);
            var phi = Math.Atan2(y.Re, x.Re);
            var theta = Math.Acos(z.Re / r);
            return new MatrixValue(new[] { new ScalarValue(r), new ScalarValue(theta), new ScalarValue(phi) }, 3, 1);
        }

        [Description("Cart2SphFunctionDescriptionForMatrix")]
        [Example("cart2sph([1, 2, 3; 4, -2, 0; 1, 0, -1; -2, 2, 1])", "Cart2SphFunctionExampleForMatrix1")]
        public MatrixValue Function(MatrixValue M)
        {
            if (M.DimensionX != 3 && M.DimensionY != 3)
                throw new YAMPMatrixDimensionException(3, M.DimensionX, M.DimensionY, M.DimensionX);

            var isTransposed = M.DimensionY != 3;

            if (isTransposed)
            {
                M = M.Transpose();
            }

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
