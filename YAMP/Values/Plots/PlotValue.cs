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
using System.IO;
using YAMP.Converter;

namespace YAMP
{
	public abstract class PlotValue : Value
    {
        #region Members

        protected List<IPointSeries> points;

        #endregion

        #region Events

        public event EventHandler<PlotEventArgs> OnPlotChanged;

		#endregion

		#region ctor

		public PlotValue()
		{
            points = new List<IPointSeries>();
			Title = string.Empty;
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

		public int Count { get { return points.Count; } }

		[ScalarToBooleanConverter]
		[StringToBooleanConverter]
		public bool Gridlines
		{
			get;
			set;
		}

		[ScalarToBooleanConverter]
		[StringToBooleanConverter]
		public bool MinorGridlines
		{
			get;
			set;
		}

		[StringToStringConverter]
		public string Title
		{
			get;
			set;
		}

		[StringToStringConverter]
		public string XLabel
		{
			get;
			set;
		}

		[StringToStringConverter]
		public string YLabel
		{
			get;
			set;
		}

		[ScalarToDoubleConverter]
		public double MinX
		{
			get;
			set;
		}

		[ScalarToDoubleConverter]
		public double MaxX
		{
			get;
			set;
		}

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

		[ScalarToDoubleConverter]
		public double MinY
		{
			get;
			set;
		}

		[ScalarToDoubleConverter]
		public double MaxY
		{
			get;
			set;
		}

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

		[ScalarToBooleanConverter]
		public bool ShowLegend
		{
			get;
			set;
		}

		[StringToEnumConverter(typeof(LegendPosition))]
		public LegendPosition LegendPosition
		{
			get;
			set;
		}

		[StringToStringConverter]
		public string LegendBackground
		{
			get;
			set;
		}

		[StringToStringConverter]
		public string LegendLineColor
		{
			get;
			set;
		}

		[ScalarToDoubleConverter]
		public double LegendLineWidth
		{
			get;
			set;
		}

		public static string[] StandardColors { get { return standardColors; } }

		#endregion

		#region Methods

		internal void RaisePlotChanged(string property)
		{
			if (OnPlotChanged != null)
			{
				var args = new PlotEventArgs(this, property);
				OnPlotChanged(this, args);
			}
		}

		public abstract void AddPoints(MatrixValue m);

		public void Update()
		{
			RaisePlotChanged("Data");
		}

        public void UpdateProperties()
        {
            RaisePlotChanged("Properties");
        }

        public void UpdateLayout()
        {
            RaisePlotChanged("Layout");
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

		protected double[] Generate(double minValue, double step, int count)
		{
			count = Math.Max(count, 0);
			var values = new double[count];

			if(count > 0)
			{
				values[0] = minValue;

				for (int i = 1; i < count; i++)
				{
					values[i] = values[i - 1] + step;
				}
			}

			return values;
		}

		protected double[] Convert(MatrixValue m, int offset, int length)
		{
			var values = new double[length];
			var complex = m.IsComplex;
			var j = offset + 1;

			for (int i = 0; i < length; i++)
			{
			    values[i] = complex ? m[j].Abs().Value : m[j].Value;
			    j++;
			}

			return values;
		}

		protected double[] ConvertX(MatrixValue m, int dx, int length, int dy)
		{
			var values = new double[length];
			var complex = m.IsComplex;
			var j = dy + 1;
			var k = dx + 1;

			for (int i = 0; i < length; i++)
			{
				values[i] = complex ? m[j, k].Abs().Value : m[j, k].Value;
				k++;
			}

			return values;
		}

		protected double[] ConvertY(MatrixValue m, int dy, int length, int dx)
		{
			var values = new double[length];
			var complex = m.IsComplex;
			var j = dy + 1;
			var k = dx + 1;

			for (int i = 0; i < length; i++)
			{
				values[i] = complex ? m[j, k].Abs().Value : m[j, k].Value;
				j++;
			}

			return values;
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

		#region Statics

		readonly static string[] standardColors = new string[]
		{
			"red", 
			"green",
			"blue",
			"pink",
			"teal",
			"orange",
			"brown",
			"lightblue",
			"violet",
			"yellow",
			"gray",
			"lightgreen",
			"cyan",
			"steelblue",
			"black",
			"gold",
			"silver",
			"forestgreen",
			"blueviolet",
			"darkorange",
			"gainsboro",
			"lightcoral",
			"olivedrab",
			"turquoise",
			"tan",
			"peachpuff"
		};

		#endregion

		#region Operators

		public override Value Add(Value right)
		{
			throw new OperationNotSupportedException("+", this);
		}

		public override Value Subtract(Value right)
		{
			throw new OperationNotSupportedException("-", this);
		}

		public override Value Multiply(Value right)
		{
			throw new OperationNotSupportedException("*", this);
		}

		public override Value Divide(Value denominator)
		{
			throw new OperationNotSupportedException("/", this);
		}

		public override Value Power(Value exponent)
		{
			throw new OperationNotSupportedException("^", this);
		}

		#endregion 

		#region Serialization

		internal void Serialize(Serializer s)
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

        internal void Deserialize(Deserializer ds)
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
