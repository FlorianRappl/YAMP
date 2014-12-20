/*
	Copyright (c) 2012-2014, Florian Rappl.
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

        /// <summary>
        /// The various included series.
        /// </summary>
        protected List<IPointSeries> points;

        /// <summary>
        /// The contained annotations.
        /// </summary>
        protected List<Annotation> annotations;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public XYPlotValue()
        {
            points = new List<IPointSeries>();
            annotations = new List<Annotation>();
            ShowLegend = true;
            LegendBackground = "white";
            LegendLineColor = "black";
            LegendLineWidth = 1.0;
            LegendPosition = LegendPosition.TopRight;
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
        public virtual bool Gridlines
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets if minor gridlines should be shown.
        /// </summary>
        [ScalarToBooleanConverter]
        [StringToBooleanConverter]
        public virtual bool MinorGridlines
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the label of the x-axis.
        /// </summary>
        [StringToStringConverter]
        public virtual string XLabel
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the label of the y-axis.
        /// </summary>
        [StringToStringConverter]
        public virtual string YLabel
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the minimum x coordinate.
        /// </summary>
        [ScalarToDoubleConverter]
        public virtual double MinX
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the maximum x coordinate.
        /// </summary>
        [ScalarToDoubleConverter]
        public virtual double MaxX
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the pair of minimum and maximum x coordinates.
        /// </summary>
        [MatrixToDoubleArrayConverter]
        public virtual double[] XRange
        {
            get { return new double[] { MinX, MaxX }; }
            set
            {
                var elements = MakeArrayPeriodic(value, 2);
                MinX = elements[0];
                MaxX = elements[1];
            }
        }

        /// <summary>
        /// Gets or sets the minimum y coordinate.
        /// </summary>
        [ScalarToDoubleConverter]
        public virtual double MinY
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the maximum y coordinate.
        /// </summary>
        [ScalarToDoubleConverter]
        public virtual double MaxY
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the pair of minimum and maximum y coordinates.
        /// </summary>
        [MatrixToDoubleArrayConverter]
        public virtual double[] YRange
        {
            get { return new double[] { MinY, MaxY }; }
            set
            {
                var elements = MakeArrayPeriodic(value, 2);
                MinY = elements[0];
                MaxY = elements[1];
            }
        }

        /// <summary>
        /// Gets or sets if the legend should be shown.
        /// </summary>
        [ScalarToBooleanConverter]
        [StringToBooleanConverter]
        public virtual bool ShowLegend
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the position of the legend.
        /// </summary>
        [StringToEnumConverter(typeof(LegendPosition))]
        public virtual LegendPosition LegendPosition
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the color of the legend.
        /// </summary>
        [StringToStringConverter]
        public virtual string LegendBackground
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the legend line color.
        /// </summary>
        [StringToStringConverter]
        public virtual string LegendLineColor
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the line width of the legend box.
        /// </summary>
        [ScalarToDoubleConverter]
        public virtual double LegendLineWidth
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the pairs of minimum and maximum x, y coordinates.
        /// </summary>
        [MatrixToDoubleArrayConverter]
        public virtual double[] View
        {
            get { return new double[] { MinX, MaxX, MinY, MaxY }; }
            set
            {
                var elements = MakeArrayPeriodic(value, 4);

                MinX = elements[0];
                MaxX = elements[1];
                MinY = elements[2];
                MaxY = elements[3];
            }
        }

        /// <summary>
        /// Gets each of the contained annotations.
        /// </summary>
        public virtual IEnumerable<Annotation> Annotations
        {
            get
            {
                foreach (var annotation in annotations)
                    yield return annotation;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds points to the plot.
        /// </summary>
        /// <param name="m">The given matrix.</param>
        public abstract void AddPoints(MatrixValue m);

        /// <summary>
        /// Initializes the values MinX, MaxX, MinY and MaxY.
        /// </summary>
        protected virtual void InitializeBoundaries()
        {
            MinX = double.MaxValue;
            MaxX = double.MinValue;
            MinY = double.MaxValue;
            MaxY = double.MinValue;
        }

        /// <summary>
        /// Sets the x-range (min and max) in one statement.
        /// </summary>
        /// <param name="min">The minimum for the x-axis.</param>
        /// <param name="max">The maximum for the x-axis.</param>
        public void SetXRange(double min, double max)
        {
            MinX = min;
            MaxX = max;
        }

        /// <summary>
        /// Sets the y-range (min and max) in one statement.
        /// </summary>
        /// <param name="min">The minimum for the y-axis.</param>
        /// <param name="max">The maximum for the y-axis.</param>
        public void SetYRange(double min, double max)
        {
            MinY = min;
            MaxY = max;
        }

        /// <summary>
        /// Adds a new series to the plot. This function automatically
        /// selects a new color for the series, such that no color
        /// should be taken twice.
        /// </summary>
        /// <param name="series">The series to add.</param>
        /// <param name="nameSeries">Should the series be named?</param>
        /// <param name="colorSeries">Should the series be colored automatically?</param>
        public void AddSeries(IPointSeries series, bool nameSeries = true, bool colorSeries = true)
        {
            if(nameSeries)
                series.Label = "Series " + (points.Count + 1);

            if(colorSeries)
                series.Color = StandardColors[Count % StandardColors.Length];

            points.Add(series);
        }

        /// <summary>
        /// Gets the specified series as an IPointSeries.
        /// </summary>
        /// <param name="index">The 0-based index of the series.</param>
        /// <returns>The series at the given index.</returns>
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
