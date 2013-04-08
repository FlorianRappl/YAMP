using System;

namespace YAMP
{
    [Description("Generate X and Y matrices for three-dimensional plots.")]
    [Kind(PopularKinds.Function)]
    sealed class MeshgridFunction : ArgumentFunction
    {
        [Description("The function call is the same as [X, Y] = meshgrid(x, x), i.e. the input value is seen as both, X and Y vector.")]
        [Returns(typeof(MatrixValue), "The generated X value matrix.", 0)]
        [Returns(typeof(MatrixValue), "The generated Y value matrix.", 1)]
        public ArgumentsValue Function(MatrixValue x)
        {
            return Function(x, x);
        }

        [Description("Transforms the domain specified by vectors x and y into arrays X and Y, which can be used to evaluate functions of two variables and three-dimensional mesh/surface plots. The rows of the output array X are copies of the vector x; columns of the output array Y are copies of the vector y.")]
        [Example("meshgrid(1:3, 10:14)", "Creates the X and Y matrices with X having the values 1 to 3 in each row, while Y has the values 10 to 14 in each column.")]
        [Returns(typeof(MatrixValue), "The generated X value matrix.", 0)]
        [Returns(typeof(MatrixValue), "The generated Y value matrix.", 1)]
        public ArgumentsValue Function(MatrixValue x, MatrixValue y)
        {
            var M = x.Length;
            var N = y.Length;
            var X = new MatrixValue(N, M);
            var Y = new MatrixValue(N, M);

            for (var i = 1; i <= N; i++)
                for (var j = 1; j <= M; j++)
                    X[i, j] = x[j].Clone();

            for (var i = 1; i <= N; i++)
                for (var j = 1; j <= M; j++)
                    Y[i, j] = y[i].Clone();

            return new ArgumentsValue(X, Y);
        }
    }
}
