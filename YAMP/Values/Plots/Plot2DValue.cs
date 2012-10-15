using System;
using System.Collections.Generic;

namespace YAMP
{
    public class Plot2DValue : PlotValue<YAMP.Plot2DValue.PointPair>
    {
        #region Methods

        public override void AddPoints(MatrixValue m)
        {
            if (m.DimensionY == 0 || m.DimensionX == 0)
                return;

            if (m.DimensionX == 1)
                AddValues(m, (i, M) => (double)i, (i, M) => M[i, 1].Value);
            else
            {
                for (var k = 2; k <= m.DimensionX; k++)
                    AddValues(m, (i, M) => M[i, 1].Value, (i, M) => M[i, k].Value);
            }
        }

        void AddValues(MatrixValue m, Func<int, MatrixValue, double> getX, Func<int, MatrixValue, double> getY)
        {
            var p = new Points<PointPair>();
            var xmin = double.MaxValue;
            var xmax = double.MinValue;
            var ymin = double.MaxValue;
            var ymax = double.MinValue;

            for (var i = 1; i <= m.DimensionY; i++)
            {
                var x = getX(i, m);
                var y = getY(i, m);

                p.Add(new PointPair
                {
                    X = x,
                    Y = y
                });

                if (x < xmin)
                    xmin = x;

                if (xmax < x)
                    xmax = x;

                if (y < ymin)
                    ymin = y;

                if (ymax < y)
                    ymax = y;
            }

            if (Count == 0 || xmin < MinX)
                MinX = xmin;

            if (Count == 0 || xmax > MaxX)
                MaxX = xmax;

            if (Count == 0 || ymin < MinY)
                MinY = ymin;

            if (Count == 0 || ymax > MaxY)
                MaxY = ymax;

            AddValues(p);
        }

        #endregion

        #region Nested types

        public struct PointPair
        {
            public double X;
            public double Y;
        }

        #endregion
    }
}
