namespace YAMP
{
    using System;
    using YAMP.Exceptions;

    [Description("Pol2CartFunctionDescription")]
    [Kind(PopularKinds.Conversion)]
    sealed class Pol2CartFunction : ArgumentFunction
    {
        [Description("Pol2CartFunctionDescriptionForScalarScalar")]
        [Example("pol2cart(4, pi/2)", "Pol2CartFunctionExampleForScalarScalar1")]
        public MatrixValue Function(ScalarValue r, ScalarValue phi)
        {
            var x = new ScalarValue(r.Re * Math.Cos(phi.Re));
            var y = new ScalarValue(r.Re * Math.Sin(phi.Re));
            return new MatrixValue(new[] { x, y }, 2, 1);
        }

        [Description("Pol2CartFunctionDescriptionForMatrix")]
        [Example("pol2cart([1, pi/2; 1, pi/3; 1, pi/4; 1, pi/5])", "Pol2CartFunctionExampleForMatrix1")]
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
                var r = M[1, i].Re;
                var phi = M[2, i].Re;
                m[1, i] = new ScalarValue(r * Math.Cos(phi));
                m[2, i] = new ScalarValue(r * Math.Sin(phi));
            }

            return isTransposed ? m.Transpose() : m;
        }
    }
}
