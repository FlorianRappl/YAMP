using System;
using YAMP.Numerics;

namespace YAMP
{
    [Description("Linear fitting by finding coefficients for a linear combination of functions. Curve fitting is the process of constructing a curve, or mathematical function, that has the best fit to a series of data points, possibly subject to constraints. Curve fitting can involve either interpolation, where an exact fit to the data is required, or smoothing, in which a smooth function is constructed that approximately fits the data.")]
    [Kind(PopularKinds.Function)]
    [Link("http://en.wikipedia.org/wiki/Curve_fitting")]
    sealed class LinfitFunction : SystemFunction
    {
        [Description("Linfit finds the coefficients of a linear combination of n functions, f_j(x(i)) to y(i), in a least squares sense. The result p is a row vector of length n containing the coefficients, i.e. p(1) * f_1 + p(2) * f_2 + p(3) * f_3 ... + p(n) * f_n.")]
        [Example("x=(-2.5:0.1:2.5); linfit(x, erf(x), [x, x.^3, tanh(x)])", "fits the error function with the function p(1) * x + p(2) * x.^3 + p(3) * tan(x). The result is p = [-0.149151; 0.006249; 1.291217]")]
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
                b[j, 1] = y[j];

            var qr = QRDecomposition.Create(M);
            return qr.Solve(b);
        }

        [Description("Linfit finds the coefficients of a linear combination of n functions, f_j(x(i)) to y(i), in a least squares sense. The result is the fit function p(1) * f_1 + p(2) * f_2 + p(3) * f_3 ... + p(n) * f_n.")]
        [Example("x=(-2.5:0.1:2.5); linfit(x, erf(x), x => [x, x.^3, tanh(x)])", "fits the error function with the function p(1) * x + p(2) * x.^3 + p(3) * tan(x). The result is a function evaluating -0.149151 * x + 0.006249 * x.^3 + 1.291217 * tanh(x)]")]
        public FunctionValue Function(MatrixValue x, MatrixValue y, FunctionValue f)
        {
            var context = Context;
            
            if (x.Length != y.Length)
                throw new YAMPDifferentLengthsException(x.Length, y.Length);
            var _fx = f.Perform(context, x[1]);

            if (!(_fx is MatrixValue))
                throw new YAMPArgumentInvalidException("Linfit", "f", 3);

            var fx = _fx as MatrixValue;

            var m = fx.Length;

            if (m < 2)
                throw new YAMPArgumentInvalidException("Linfit", "f", 3);

            var M = new MatrixValue(x.Length, m);

            for (var j = 1; j <= M.Rows; j++)
            {
                if (j > 1)
                    fx = f.Perform(context, x[j]) as MatrixValue;

                for (var i = 1; i <= M.Columns; i++)
                    M[j, i] = fx[i];
            }

            var p = Function(x, y, M);

            return new FunctionValue((parseContext, variable) => ((f.Perform(parseContext, variable) as MatrixValue) * p)[1], true);
        }
    }
}
