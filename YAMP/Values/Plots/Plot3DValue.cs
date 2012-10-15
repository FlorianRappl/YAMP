using System;
using System.Collections.Generic;

namespace YAMP
{
    public class Plot3DValue : PlotValue<YAMP.Plot3DValue.PointTriple>
    {
        #region Members

        string zlabel;
        double minz;
        double maxz;

        #endregion

        #region Properties

        public string ZLabel
        {
            get { return zlabel; }
            set
            {
                zlabel = value;
                Changed("ZLabel");
            }
        }

        public double MinZ
        {
            get { return minz; }
            set
            {
                minz = value;
                Changed("MinZ");
            }
        }

        public double MaxZ
        {
            get { return maxz; }
            set
            {
                maxz = value;
                Changed("MaxZ");
            }
        }

        #endregion

        #region Methods

        public void SetZRange(double min, double max)
        {
            MinZ = min;
            MaxZ = max;
        }

        public override void AddPoints(MatrixValue m)
        {
            if (m.DimensionY == 0 || m.DimensionX != 3)
                return;

            AddValues(m);
        }

        void AddValues(MatrixValue m)
        {
            var p = new Points<PointTriple>();
            var xmin = double.MaxValue;
            var xmax = double.MinValue;
            var ymin = double.MaxValue;
            var ymax = double.MinValue;
            var zmin = double.MaxValue;
            var zmax = double.MinValue;

            for (var i = 1; i <= m.DimensionY; i++)
            {
                var x = m[i, 1].Value;
                var y = m[i, 2].Value;
                var z = m[i, 3].Value;

                p.Add(new PointTriple
                {
                    X = x,
                    Y = y,
                    Z = z
                });

                if (x < xmin)
                    xmin = x;

                if (xmax < x)
                    xmax = x;

                if (y < ymin)
                    ymin = y;

                if (ymax < y)
                    ymax = y;

                if (z < zmin)
                    zmin = z;

                if (zmax < z)
                    zmax = z;
            }

            if (Count == 0 || xmin < MinX)
                MinX = xmin;

            if (Count == 0 || xmax > MaxX)
                MaxX = xmax;

            if (Count == 0 || ymin < MinY)
                MinY = ymin;

            if (Count == 0 || ymax > MaxY)
                MaxY = ymax;

            if (Count == 0 || zmin < MinZ)
                MinZ = zmin;

            if (Count == 0 || zmax > MaxZ)
                MaxZ = zmax;

            AddValues(p);
        }

        #endregion

        #region Nested Type

        public struct PointTriple
        {
            public double X;
            public double Y;
            public double Z;
        }

        #endregion
    }
}
