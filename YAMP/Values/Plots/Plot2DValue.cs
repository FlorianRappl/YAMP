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
            {
                var x = new RangeValue(1.0, (double)m.DimensionY, 1.0);
                AddValues(x, m);
            }
            else
            {
                var x = m.SubMatrix(0, m.DimensionY, 0, 1);

                for (var k = 2; k <= m.DimensionX; k++)
                {
                    var y = m.SubMatrix(0, m.DimensionY, k - 1, 1);
                    AddValues(x, y);
                }
            }
        }

        public void AddPoints(MatrixValue x, MatrixValue y)
        {
            if (x.DimensionX > 1 && x.DimensionY > 1)
            {
                AddPoints(x);
                AddPoints(y);
                return;
            }

            if(y.DimensionY == 1 && y.DimensionX > 1)
                y = y.Transpose();

            var dim = Math.Min(x.Length, y.DimensionY);

            for (var k = 1; k <= y.DimensionX; k++)
            {
                var values = y.SubMatrix(0, dim, k - 1, 1);
                AddValues(x, values);
            }
        }

        void AddValues(MatrixValue _x, MatrixValue _y)
        {
            var p = new Points<PointPair>();
            var xmin = double.MaxValue;
            var xmax = double.MinValue;
            var ymin = double.MaxValue;
            var ymax = double.MinValue;

            for (var i = 1; i <= _x.Length; i++)
            {
                var x = _x[i].Value;
                var y = _y[i].Value;

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
