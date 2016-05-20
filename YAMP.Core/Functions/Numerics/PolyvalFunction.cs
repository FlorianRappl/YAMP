namespace YAMP
{
    using System;

    [Description("PolyvalFunctionDescription")]
    [Kind(PopularKinds.Function)]
    [Link("PolyvalFunctionLink")]
    sealed class PolyvalFunction : ArgumentFunction
    {
        [Description("PolyvalFunctionDescriptionForMatrixMatrix")]
        [Example("polyval([1 2 3], [5 7 9])", "PolyvalFunctionExampleForMatrixMatrix1")]
        public MatrixValue Function(MatrixValue p, MatrixValue X)
        {
            var coeff = p.ToArray();
            var M = new MatrixValue(X.Rows, X.Columns);
            var poly = BuildPolynom(coeff);

            for (var j = 1; j <= X.Rows; j++)
            {
                for (var i = 1; i <= X.Columns; i++)
                {
                    M[j, i] = poly(X[j, i]);
                }
            }

            return M;
        }

        [Description("PolyvalFunctionDescriptionForMatrixScalar")]
        [Example("polyval([1 2 3], 5)", "PolyvalFunctionExampleForMatrixScalar1")]
        public ScalarValue Function(MatrixValue p, ScalarValue z)
        {
            var coeff = p.ToArray();
            var poly = BuildPolynom(coeff);
            return poly(z);
        }

        static Func<ScalarValue, ScalarValue> BuildPolynom(ScalarValue[] coeff)
        {
            return z =>
            {
                var pow = ScalarValue.One;
                var sum = ScalarValue.Zero;

                for (var i = 0; i < coeff.Length; i++)
                {
                    var c = coeff[i];
                    sum += c * pow;
                    pow *= z;
                }

                return sum;
            };
        }
    }
}
