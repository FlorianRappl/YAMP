using System;

namespace YAMP
{
    class PlotFunction : VisualizationFunction
    {
        public Plot2DValue Function(MatrixValue m)
        {
            var plot = new Plot2DValue();
            plot.AddPoints(m);
            return plot;
        }

        public Plot2DValue Function(MatrixValue m, MatrixValue n)
        {
            var plot = new Plot2DValue();
            plot.AddPoints(m, n);
            return plot;
        }
    }
}
