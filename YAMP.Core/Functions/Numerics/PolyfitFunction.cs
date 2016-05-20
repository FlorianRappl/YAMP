namespace YAMP
{
    using YAMP.Exceptions;
    using YAMP.Numerics;

    [Description("PolyfitFunctionDescription")]
    [Kind(PopularKinds.Function)]
    [Link("PolyfitFunctionLink")]
    sealed class PolyfitFunction : ArgumentFunction
    {
        [Description("PolyfitFunctionDescriptionForMatrixMatrixScalar")]
        [Example("polyfit(0:0.1:2.5, erf(0:0.1:2.5), 6)", "PolyfitFunctionExampleForMatrixMatrixScalar1")]
        public MatrixValue Function(MatrixValue x, MatrixValue y, ScalarValue n)
        {
            if (x.Length != y.Length)
                throw new YAMPDifferentLengthsException(x.Length, y.Length);

            var nn = n.GetIntegerOrThrowException("n", Name);
            var m = nn + 1;

            if (m < 2)
                throw new YAMPArgumentRangeException("n", 0.0);

            var M = new MatrixValue(x.Length, m);
            var b = new MatrixValue(x.Length, 1);

            for (var j = 1; j <= M.Rows; j++)
            {
                var el = ScalarValue.One;
                var z = x[j];

                for (var i = 1; i <= M.Columns; i++)
                {
                    M[j, i] = el;
                    el *= z;
                }

                b[j, 1] = y[j];
            }

            var qr = QRDecomposition.Create(M);
            return qr.Solve(b);
        }
    }
}
