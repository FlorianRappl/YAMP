namespace YAMP
{
    using System;

    [Description("SurfFunctionDescription")]
    [Kind(PopularKinds.Plot)]
    sealed class SurfFunction : VisualizationFunction
    {
        public SurfFunction(ParseContext context)
            : base(context)
        {
        }

        [Description("SurfFunctionDescriptionForMatrix")]
        public SurfacePlotValue Function(MatrixValue Z)
        {
            var splot = new SurfacePlotValue();
            splot.IsSurf = true;
            splot.IsMesh = false;
            splot.AddPoints(Z);
            return splot;
        }

        [Description("SurfFunctionDescriptionForMatrixMatrixMatrix")]
        [Example("[X, Y] = meshgrid(-8:.5:8); R = sqrt(X.^2 + Y.^2); Z = sin(R)./R; surf(X, Y, Z);", "SurfFunctionExampleForMatrixMatrixMatrix1")]
        public SurfacePlotValue Function(MatrixValue X, MatrixValue Y, MatrixValue Z)
        {
            var splot = new SurfacePlotValue();
            splot.IsSurf = true;
            splot.IsMesh = false;
            splot.AddPoints(X, Y, Z);
            return splot;
        }
    }
}
