using System;

namespace YAMP
{
    [Description("Produces wireframe surfaces that color only the lines connecting the defining points.")]
    [Kind(PopularKinds.Plot)]
    sealed class MeshFunction : VisualizationFunction
    {
        [Description("Draws a wireframe mesh using X = 1:n and Y = 1:m, where [m, n] = size(Z). The height, Z, is a single-valued function defined over a rectangular grid. Color is proportional to surface height.")]
        public SurfacePlotValue Function(MatrixValue Z)
        {
            var splot = new SurfacePlotValue();
            splot.AddPoints(Z);
            splot.IsMesh = true;
            splot.IsSurf = false;
            return splot;
        }

        [Description("Draws a wireframe mesh with color determined by Z, so color is proportional to surface height. If X and Y are vectors, length(X) = n and length(Y) = m, where [m, n] = size(Z). In this case, (X(j), Y(i), Z(i,j)) are the intersections of the wireframe grid lines; X and Y correspond to the columns and rows of Z, respectively. If X and Y are matrices, (X(i, j), Y(i, j), Z(i, j)) are the intersections of the wireframe grid lines.")]
        [Example("[X, Y] = meshgrid(-8:.5:8); R = sqrt(X.^2 + Y.^2); Z = sin(R)./R; mesh(Z);", "Evaluates sin(r)/r, or the so called sinc function as a mesh plot.")]
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
