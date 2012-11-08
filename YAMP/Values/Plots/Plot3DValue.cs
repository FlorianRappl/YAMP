/*
    Copyright (c) 2012, Florian Rappl.
    All rights reserved.

    Redistribution and use in source and binary forms, with or without
    modification, are permitted provided that the following conditions are met:
        * Redistributions of source code must retain the above copyright
          notice, this list of conditions and the following disclaimer.
        * Redistributions in binary form must reproduce the above copyright
          notice, this list of conditions and the following disclaimer in the
          documentation and/or other materials provided with the distribution.
        * Neither the name of the YAMP team nor the names of its contributors
          may be used to endorse or promote products derived from this
          software without specific prior written permission.

    THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
    ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
    WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
    DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
    DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
    (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
    LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
    ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
    (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
    SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

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

        public bool IsLogZ
        {
            get;
            internal set;
        }

        public string ZLabel
        {
            get { return zlabel; }
            set
            {
                zlabel = value;
            }
        }

        public double MinZ
        {
            get { return minz; }
            set
            {
                minz = value;
            }
        }

        public double MaxZ
        {
            get { return maxz; }
            set
            {
                maxz = value;
            }
        }

        public double[] ZRange
        {
            get { return new double[] { MinZ, MaxZ }; }
            set
            {
                MinZ = value[0];
                MaxZ = value[1];
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
