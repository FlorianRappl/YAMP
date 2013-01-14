/*
    Copyright (c) 2012-2013, Florian Rappl.
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
using YAMP.Converter;

namespace YAMP
{
    /// <summary>
    /// Is the type for errorbar plots.
    /// </summary>
    public sealed class ErrorPlotValue : XYPlotValue
    {
        #region ctor

        public ErrorPlotValue()
        {
            IsLogX = false;
            IsLogY = false;
        }

        #endregion

        #region Properties

        [ScalarToBooleanConverter]
        [StringToBooleanConverter]
        public bool IsLogX
        {
            get;
            set;
        }

        [ScalarToBooleanConverter]
        [StringToBooleanConverter]
        public bool IsLogY
        {
            get;
            set;
        }

        #endregion

        #region Methods

        public override void AddPoints(MatrixValue m)
        {
            if (m.DimensionY == 0 || m.DimensionX == 0)
                return;

            if (m.IsVector)
            {
                var x = Generate(1.0, 1.0, m.Length);
                var y = Convert(m, 0, m.Length);
                AddValues(x, y, new double[0], new double[0]);
            }
            else if(m.DimensionX <= m.DimensionY)
            {
                var x = ConvertY(m, 0, m.DimensionY, 0);

                for (var k = 2; k <= m.DimensionX; k++)
                {
                    var y = ConvertY(m, 0, m.DimensionY, k - 1);
                    AddValues(x, y, new double[0], new double[0]);
                }
            }
            else
            {
                var x = ConvertX(m, 0, m.DimensionX, 0);

                for (var k = 2; k <= m.DimensionY; k++)
                {
                    var y = ConvertX(m, 0, m.DimensionX, k - 1);
                    AddValues(x, y, new double[0], new double[0]);
                }
            }
        }

        public void AddPoints(MatrixValue y, MatrixValue err)
        {
            var x = new MatrixValue(Math.Max(y.DimensionY, y.DimensionX), 1);

            for (var i = 1; i <= y.Length; i++)
                x[i, 1] = new ScalarValue(i);

            AddPoints(x, y, err);
        }

        public void AddPoints(MatrixValue x, MatrixValue y, MatrixValue err)
        {
            if (x.IsVector)
            {
                var vx = Convert(x, 0, x.Length);

                if (y.DimensionY > y.DimensionX || y.DimensionY >= x.Length)
                {
                    var dim = Math.Min(x.Length, y.DimensionY);
                    var yerr = err.DimensionX > 0 ? ConvertY(err, 0, Math.Min(dim, err.DimensionY), 0) : new double[0];
                    var xerr = err.DimensionX > 1 ? ConvertY(err, 0, Math.Min(dim, err.DimensionY), 1) : new double[0];

                    for (var i = 0; i < y.DimensionX; i++)
                    {
                        var vy = ConvertY(y, 0, dim, i);
                        AddValues(vx, vy, xerr, yerr);
                    }
                }
                else
                {
                    var dim = Math.Min(x.Length, y.DimensionX);
                    var yerr = err.DimensionY > 0 ? ConvertX(err, 0, Math.Min(dim, err.DimensionX), 0) : new double[0];
                    var xerr = err.DimensionY > 1 ? ConvertX(err, 0, Math.Min(dim, err.DimensionX), 1) : new double[0];

                    for (var i = 0; i < y.DimensionY; i++)
                    {
                        var vy = ConvertX(y, 0, dim, i);
                        AddValues(vx, vy, xerr, yerr);
                    }
                }
            }
            else
            {
                AddPoints(x, err);
                AddPoints(y, err);
            }
        }

        void AddValues(double[] _x, double[] _y, double[] _xerr, double[] _yerr)
        {
            var p = new Points<ErrorPointPair>();
            var xmin = double.MaxValue;
            var xmax = double.MinValue;
            var ymin = double.MaxValue;
            var ymax = double.MinValue;

            for (var i = 0; i < _y.Length; i++)
            {
                var x = _x[i];
                var y = _y[i];

                p.Add(new ErrorPointPair
                {
                    X = x,
                    Y = y,
                    Xerr = _xerr.Length > i ? _xerr[i] : 0.0,
                    Yerr = _yerr.Length > i ? _yerr[i] : 0.0
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

            AddSeries(p);
        }

        #endregion

        #region Nested types

        public struct ErrorPointPair
        {
            public double X;
            public double Y;
            public double Xerr;
            public double Yerr;
        }

        #endregion

        #region Serialization

        public override byte[] Serialize()
        {
            using (var s = Serializer.Create())
            {
                Serialize(s);
                s.Serialize(IsLogX);
                s.Serialize(IsLogY);
                s.Serialize(Count);

                for (var i = 0; i < Count; i++)
                {
                    var points = this[i];
                    points.Serialize(s);
                    s.Serialize(points.Count);

                    for (int j = 0; j < points.Count; j++)
                    {
                        s.Serialize(points[j].X);
                        s.Serialize(points[j].Y);
                        s.Serialize(points[j].Xerr);
                        s.Serialize(points[j].Yerr);
                    }
                }

                return s.Value;
            }
        }

        public override Value Deserialize(byte[] content)
        {
            using (var ds = Deserializer.Create(content))
            {
                Deserialize(ds);
                IsLogX = ds.GetBoolean();
                IsLogY = ds.GetBoolean();
                var length = ds.GetInt();

                for (var i = 0; i < length; i++)
                {
                    var points = new Points<ErrorPointPair>();
                    points.Deserialize(ds);
                    var count = ds.GetInt();

                    for (int j = 0; j < count; j++)
                    {
                        var x = ds.GetDouble();
                        var y = ds.GetDouble();
                        var xerr = ds.GetDouble();
                        var yerr = ds.GetDouble();

                        points.Add(new ErrorPointPair
                        {
                            X = x,
                            Y = y,
                            Xerr = xerr,
                            Yerr = yerr
                        });
                    }

                    AddSeries(points);
                }
            }

            return this;
        }

        #endregion

        #region Index

        public Points<ErrorPointPair> this[int index]
        {
            get
            {
                return base.GetSeries(index) as Points<ErrorPointPair>;
            }
        }

        public ErrorPointPair this[int index, int point]
        {
            get
            {
                return this[index][point];
            }
        }

        #endregion
    }
}
