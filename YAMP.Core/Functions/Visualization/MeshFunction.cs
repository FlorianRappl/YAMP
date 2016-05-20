namespace YAMP
{
    [Description("MeshFunctionDescription")]
    [Kind(PopularKinds.Plot)]
    sealed class MeshFunction : VisualizationFunction
    {
        public MeshFunction(ParseContext context)
            : base(context)
        {
        }

        [Description("MeshFunctionDescriptionForMatrix")]
        public SurfacePlotValue Function(MatrixValue Z)
        {
            var splot = new SurfacePlotValue();
            splot.AddPoints(Z);
            splot.IsMesh = true;
            splot.IsSurf = false;
            return splot;
        }

        [Description("MeshFunctionDescriptionForMatrixMatrixMatrix")]
        [Example("[X, Y] = meshgrid(-8:.5:8); R = sqrt(X.^2 + Y.^2); Z = sin(R)./R; mesh(Z);", "MeshFunctionExampleForMatrixMatrixMatrix1")]
        public SurfacePlotValue Function(MatrixValue X, MatrixValue Y, MatrixValue Z)
        {
            var splot = new SurfacePlotValue();
            splot.AddPoints(X, Y, Z);
            splot.IsMesh = true;
            splot.IsSurf = false;
            return splot;
        }
    }
}
