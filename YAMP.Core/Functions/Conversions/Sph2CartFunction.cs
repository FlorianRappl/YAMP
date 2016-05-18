namespace YAMP
{
    using System;
    using YAMP.Exceptions;

    [Description("Sph2CartFunctionDescription")]
    [Kind(PopularKinds.Conversion)]
    sealed class Sph2CartFunction : ArgumentFunction
    {
        [Description("Sph2CartFunctionDescriptionForScalarScalarScalar")]
        [Example("sph2cart(4, pi/2, pi/4)", "Sph2CartFunctionExampleForScalarScalarScalar1")]
        public MatrixValue Function(ScalarValue r, ScalarValue phi, ScalarValue theta)
        {
            var rt = r.Re * Math.Sin(theta.Re);
            var x = new ScalarValue(rt * Math.Cos(phi.Re));
            var y = new ScalarValue(rt * Math.Sin(phi.Re));
            var z = new ScalarValue(r.Re * Math.Cos(theta.Re));
            return new MatrixValue(new[] { x, y, z }, 3, 1);
        }

        [Description("Sph2CartFunctionDescriptionForMatrix")]
        [Example("sph2cart([1, pi/2, pi/4; 1, pi/3, pi/4; 1, pi/4, pi/4; 1, pi/5, pi/4])", "Sph2CartFunctionExampleForMatrix1")]
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
                var r = M[1, i].Re;
                var theta = M[2, i].Re;
                var phi = M[3, i].Re;
                var rt = r * Math.Sin(theta);
                m[1, i] = new ScalarValue(rt * Math.Cos(phi));
                m[2, i] = new ScalarValue(rt * Math.Sin(phi));
                m[3, i] = new ScalarValue(r * Math.Cos(theta));
            }

            return isTransposed ? m.Transpose() : m;
        }
    }
}
