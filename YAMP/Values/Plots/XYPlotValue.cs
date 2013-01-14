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
using System.Collections.Generic;
using YAMP.Converter;

namespace YAMP
{
    /// <summary>
    /// Abstract base class for plot values that are based on an XY
    /// (or similar) coordinate system.
    /// </summary>
    public abstract class XYPlotValue : PlotValue
    {
        #region Members

        protected List<IPointSeries> points;
        protected List<Annotation> annotations;

        #endregion

        #region ctor

        public XYPlotValue()
        {
            points = new List<IPointSeries>();
            annotations = new List<Annotation>();
            ShowLegend = true;
            LegendBackground = "white";
            LegendLineColor = "black";
            LegendLineWidth = 1.0;
            LegendPosition = YAMP.LegendPosition.RightTop;
            XLabel = "x";
            YLabel = "y";
            Gridlines = false;
            MinorGridlines = false;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the number of contained series.
        /// </summary>
        public override int Count { get { return points.Count; } }

        /// <summary>
        /// Gets or sets if (major) gridlines should be shown.
        /// </summary>
        [ScalarToBooleanConverter]
        [StringToBooleanConverter]
        public bool Gridlines
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets if minor gridlines should be shown.
        /// </summary>
        [ScalarToBooleanConverter]
        [StringToBooleanConverter]
        public bool MinorGridlines
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the label of the x-axis.
        /// </summary>
        [StringToStringConverter]
        public string XLabel
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the label of the y-axis.
        /// </summary>
        [StringToStringConverter]
        public string YLabel
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the minimum x coordinate.
        /// </summary>
        [ScalarToDoubleConverter]
        public double MinX
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the maximum x coordinate.
        /// </summary>
        [ScalarToDoubleConverter]
        public double MaxX
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the pair of minimum and maximum x coordinates.
        /// </summary>
        [MatrixToDoubleArrayConverter]
        public double[] XRange
        {
            get { return new double[] { MinX, MaxX }; }
            set
            {
                MinX = value[0];
                MaxX = value[1];
            }
        }

        /// <summary>
        /// Gets or sets the minimum y coordinate.
        /// </summary>
        [ScalarToDoubleConverter]
        public double MinY
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the maximum y coordinate.
        /// </summary>
        [ScalarToDoubleConverter]
        public double MaxY
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the pair of minimum and maximum y coordinates.
        /// </summary>
        [MatrixToDoubleArrayConverter]
        public double[] YRange
        {
            get { return new double[] { MinY, MaxY }; }
            set
            {
                MinY = value[0];
                MaxY = value[1];
            }
        }

        /// <summary>
        /// Gets or sets if the legend should be shown.
        /// </summary>
        [ScalarToBooleanConverter]
        [StringToBooleanConverter]
        public bool ShowLegend
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the position of the legend.
        /// </summary>
        [StringToEnumConverter(typeof(LegendPosition))]
        public LegendPosition LegendPosition
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the color of the legend.
        /// </summary>
        [StringToStringConverter]
        public string LegendBackground
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the legend line color.
        /// </summary>
        [StringToStringConverter]
        public string LegendLineColor
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the line width of the legend box.
        /// </summary>
        [ScalarToDoubleConverter]
        public double LegendLineWidth
        {
            get;
            set;
        }

        /// <summary>
        /// Gets each of the contained annotations.
        /// </summary>
        public IEnumerable<Annotation> Annotations
        {
            get
            {
                foreach (var annotation in annotations)
                    yield return annotation;
            }
        }

        #endregion

        #region Methods

        public abstract void AddPoints(MatrixValue m);

        protected virtual void InitializeBoundaries()
        {
            MinX = double.MaxValue;
            MaxX = double.MinValue;
            MinY = double.MaxValue;
            MaxY = double.MinValue;
        }

        public void SetXRange(double min, double max)
        {
            MinX = min;
            MaxX = max;
        }

        public void SetYRange(double min, double max)
        {
            MinY = min;
            MaxY = max;
        }

        public void AddSeries(IPointSeries series)
        {
            series.Color = StandardColors[Count % StandardColors.Length];
            points.Add(series);
        }

        public IPointSeries GetSeries(int index)
        {
            return points[index];
        }

        #endregion

        #region Serialization

        internal virtual void Serialize(Serializer s)
        {
            s.Serialize(Title);
            s.Serialize(ShowLegend);
            s.Serialize(LegendBackground);
            s.Serialize(LegendLineColor);
            s.Serialize(LegendLineWidth);
            s.Serialize((int)LegendPosition);
            s.Serialize(XLabel);
            s.Serialize(YLabel);
            s.Serialize(Gridlines);
            s.Serialize(MinorGridlines);
            s.Serialize(MinX);
            s.Serialize(MaxX);
            s.Serialize(MinY);
            s.Serialize(MaxY);
        }

        internal virtual void Deserialize(Deserializer ds)
        {
            Title = ds.GetString();
            ShowLegend = ds.GetBoolean();
            LegendBackground = ds.GetString();
            LegendLineColor = ds.GetString();
            LegendLineWidth = ds.GetDouble();
            LegendPosition = (LegendPosition)ds.GetInt();
            XLabel = ds.GetString();
            YLabel = ds.GetString();
            Gridlines = ds.GetBoolean();
            MinorGridlines = ds.GetBoolean();
            MinX = ds.GetDouble();
            MaxX = ds.GetDouble();
            MinY = ds.GetDouble();
            MaxY = ds.GetDouble();
        }

        #endregion
    }
}
