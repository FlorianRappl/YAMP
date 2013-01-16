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
    /// Contains the data for heatmap plots.
    /// </summary>
    public sealed class HeatmapPlotValue : XYPlotValue
    {
        #region ctor

        public HeatmapPlotValue()
        {
            ColorPalette = ColorPalettes.Hot;
        }

        #endregion

        #region Methods

        public override void AddPoints(MatrixValue M)
        {
            var p = new Points<HeatPoint>();
            var min = double.MaxValue;
            var max = double.MinValue;

            for (var i = 1; i <= M.DimensionX; i++)
            {
                for (var j = 1; j <= M.DimensionY; j++)
                {
                    var value = M[j, i].Abs();

                    p.Add(new HeatPoint
                    {
                        Row = j,
                        Column = i,
                        Magnitude = value
                    });

                    if (value > max)
                        max = value;
                    else if (value < min)
                        min = value;
                }
            }

            var gap = max - min;

            for (var i = 0; i != p.Count; i++)
                p[i] = new HeatPoint { Column = p[i].Column, Row = p[i].Row, Magnitude = (p[i].Magnitude - min) / gap };

            Minimum = min;
            Maximum = max;
            MinX = 1;
            MaxX = M.DimensionX;
            MinY = 1;
            MaxY = M.DimensionY;
            AddSeries(p);
        }

        #endregion

        #region Properties
        
        /// <summary>
        /// Gets or sets the color palette to use.
        /// </summary>
        [StringToEnumConverter(typeof(ColorPalettes))]
        public ColorPalettes ColorPalette
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the minimum (absolute) value of the matrix.
        /// </summary>
        public double Minimum
        { get; private set; }

        /// <summary>
        /// Gets the maximum (absolute) value of the matrix.
        /// </summary>
        public double Maximum
        { get; private set; }

        #endregion

        #region Nested types

        public struct HeatPoint
        {
            public int Column;

            public int Row;

            public double Magnitude;
        }

        #endregion

        #region Serialization

        public override byte[] Serialize()
        {
            using (var s = Serializer.Create())
            {
                Serialize(s);
                s.Serialize((int)ColorPalette);
                s.Serialize(Minimum);
                s.Serialize(Maximum);
                s.Serialize(Count);

                for (var i = 0; i < Count; i++)
                {
                    var points = this[i];
                    points.Serialize(s);
                    s.Serialize(points.Count);

                    for (int j = 0; j < points.Count; j++)
                    {
                        s.Serialize(points[j].Column);
                        s.Serialize(points[j].Row);
                        s.Serialize(points[j].Magnitude);
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
                ColorPalette = (ColorPalettes)ds.GetInt();
                Minimum = ds.GetDouble();
                Maximum = ds.GetDouble();

                var length = ds.GetInt();

                for (var i = 0; i < length; i++)
                {
                    var points = new Points<HeatPoint>();
                    points.Deserialize(ds);
                    var count = ds.GetInt();

                    for (int j = 0; j < count; j++)
                    {
                        var x = ds.GetInt();
                        var y = ds.GetInt();
                        var z = ds.GetDouble();

                        points.Add(new HeatPoint
                        {
                            Column = x,
                            Row = y,
                            Magnitude = z
                        });
                    }

                    AddSeries(points);
                }
            }

            return this;
        }

        #endregion

        #region Index

        public Points<HeatPoint> this[int index]
        {
            get
            {
                return base.GetSeries(index) as Points<HeatPoint>;
            }
        }

        public HeatPoint this[int index, int point]
        {
            get
            {
                return this[index][point];
            }
        }

        #endregion
    }
}
