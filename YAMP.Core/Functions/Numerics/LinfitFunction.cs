namespace YAMP
{
    using YAMP.Exceptions;
    using YAMP.Numerics;

    [Description("LinfitFunctionDescription")]
    [Kind(PopularKinds.Function)]
    [Link("LinfitFunctionLink")]
    sealed class LinfitFunction : SystemFunction
    {
        public LinfitFunction(ParseContext context)
            : base(context)
        {
        }

        [Description("LinfitFunctionDescriptionForMatrixMatrixMatrix")]
        [Example("x=(-2.5:0.1:2.5); linfit(x, erf(x), [x, x.^3, tanh(x)])", "LinfitFunctionExampleForMatrixMatrixMatrix1")]
        public MatrixValue Function(MatrixValue x, MatrixValue y, MatrixValue f)
        {
            if (x.Length != y.Length)
                throw new YAMPDifferentLengthsException(x.Length, y.Length);

            if (x.Length != f.DimensionY)
                throw new YAMPDifferentLengthsException(x.Length, f.DimensionY);

            var m = f.DimensionX;

            if (m < 2)
                throw new YAMPArgumentInvalidException("Linfit", "f", 3);

            var M = f;
            var b = new MatrixValue(x.Length, 1);

            for (var j = 1; j <= M.Rows; j++)
            {
                b[j, 1] = y[j];
            }

            var qr = QRDecomposition.Create(M);
            return qr.Solve(b);
        }

        [Description("LinfitFunctionDescriptionForMatrixMatrixFunction")]
        [Example("x=(-2.5:0.1:2.5); linfit(x, erf(x), x => [x, x.^3, tanh(x)])", "LinfitFunctionExampleForMatrixMatrixFunction1")]
        public FunctionValue Function(MatrixValue x, MatrixValue y, FunctionValue f)
        {
            var context = Context;
            
            if (x.Length != y.Length)
                throw new YAMPDifferentLengthsException(x.Length, y.Length);

            var _fx = f.Perform(context, x[1]);

            if (_fx is MatrixValue == false)
                throw new YAMPArgumentInvalidException("Linfit", "f", 3);

            var fx = _fx as MatrixValue;

            var m = fx.Length;

            if (m < 2)
                throw new YAMPArgumentInvalidException("Linfit", "f", 3);

            var M = new MatrixValue(x.Length, m);

            for (var j = 1; j <= M.Rows; j++)
            {
                if (j > 1)
                {
                    fx = f.Perform(context, x[j]) as MatrixValue;
                }

                for (var i = 1; i <= M.Columns; i++)
                {
                    M[j, i] = fx[i];
                }
            }

            var p = Function(x, y, M);
            return new FunctionValue((parseContext, variable) => ((f.Perform(parseContext, variable) as MatrixValue) * p)[1], true);
        }
    }
}
