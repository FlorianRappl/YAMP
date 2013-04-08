using System;

namespace YAMP
{
    [Description("The surf function displays both the connecting lines and the faces of the surface in color.")]
    [Kind(PopularKinds.Plot)]
    sealed class SurfFunction : VisualizationFunction
    {
        [Description("Creates a three-dimensional shaded surface from the z components in matrix Z, using x = 1:n and y = 1:m, where [m, n] = size(Z). The height, Z, is a single-valued function defined over a geometrically rectangular grid. Z specifies the color data, as well as surface height, so color is proportional to surface height.")]
        public SurfacePlotValue Function(MatrixValue Z)
        {
            var splot = new SurfacePlotValue();
            splot.IsSurf = true;
            splot.IsMesh = false;
            splot.AddPoints(Z);
            return splot;
        }

        [Description("Uses Z for the color data and surface height. X and Y are vectors or matrices defining the x and y components of a surface. If X and Y are vectors, length(X) = n and length(Y) = m, where [m, n] = size(Z). In this case, the vertices of the surface faces are (X(j), Y(i), Z(i,j)) triples. To create X and Y matrices for arbitrary domains, use the meshgrid function.")]
        [Example("[X, Y] = meshgrid(-8:.5:8); R = sqrt(X.^2 + Y.^2); Z = sin(R)./R; surf(X, Y, Z);", "Evaluates sin(r)/r, or the so called sinc function as a surface plot.")]
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
