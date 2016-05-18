namespace YAMP
{
    using System;
    using YAMP.Exceptions;

    [Description("Cart2PolFunctionDescription")]
    [Kind(PopularKinds.Conversion)]
    sealed class Cart2PolFunction : ArgumentFunction
    {
        [Description("Cart2PolFunctionDescriptionForScalarScalar")]
        [Example("cart2pol(3, 2)", "Cart2PolFunctionExampleForScalarScalar1")]
        public MatrixValue Function(ScalarValue x, ScalarValue y)
        {
            var phi = Math.Atan2(y.Re, x.Re);
            var r = x.Re == 0.0 ? y.Re : (y.Re == 0.0 ? x.Re : x.Re / Math.Cos(phi));
            return new MatrixValue(new[] { new ScalarValue(r), new ScalarValue(phi) }, 2, 1);
        }

        [Description("Cart2PolFunctionDescriptionForMatrix")]
        [Example("cart2pol([1, 2; 4, -2; 1, 0; -2, 2])", "Cart2PolFunctionExampleForMatrix1")]
        public MatrixValue Function(MatrixValue M)
        {
            if (M.DimensionX != 2 && M.DimensionY != 2)
                throw new YAMPMatrixDimensionException(2, M.DimensionX, M.DimensionY, M.DimensionX);

            var isTransposed = M.DimensionY != 2;

            if (isTransposed)
            {
                M = M.Transpose();
            }

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
