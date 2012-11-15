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
	public abstract class PlotValue<T> : PlotValue
	{
		#region Members

		List<Points<T>> points;

		#endregion

		#region ctor

		public PlotValue ()
		{
			points = new List<Points<T>>();
		}

		#endregion

		#region Properties

		public int Count { get { return points.Count; } }

		public Points<T> this[int index]
		{
			get
			{
				return points[index];
			}
		}

		public T this[int index, int point]
		{
			get
			{
				return points[index][point];
			}
		}

		#endregion

		#region Methods

		public void AddValues(Points<T> values)
		{
			values.Color = ColorPalette[Count % ColorPalette.Length];
			points.Add(values);
		}

		#endregion
	}

	public abstract class PlotValue : Value
	{
		#region Members

		string title;
		string xlabel;
		string ylabel;
		double minx;
		double maxx;
		double miny;
		double maxy;

		#endregion

		#region ctor

		public PlotValue()
		{
			Title = string.Empty;
			XLabel = "x";
			YLabel = "y";
		}

		#endregion

		#region Properties

		public bool IsLogX
		{
			get;
			internal set;
		}

		public bool IsLogY
		{
			get;
			internal set;
		}

		public bool IsPolar
		{
			get;
			internal set;
		}

		public string Title 
		{
			get { return title; }
			set
			{
				title = value;
			}
		}

		public string XLabel 
		{
			get { return xlabel; }
			set
			{
				xlabel = value;
			}
		}

		public string YLabel 
		{
			get { return ylabel; }
			set
			{
				ylabel = value;
			}
		}

		public double MinX 
		{
			get { return minx; }
			set
			{
				minx = value;
			}
		}

		public double MaxX 
		{
			get { return maxx; }
			set
			{
				maxx = value;
			}
		}

		public double[] XRange
		{
			get { return new double[] { MinX, MaxX }; }
			set
			{
				MinX = value[0];
				MaxX = value[1];
			}
		}

		public double MinY 
		{
			get { return miny; }
			set
			{
				miny = value;
			}
		}

		public double MaxY 
		{
			get { return maxy; }
			set
			{
				maxy = value;
			}
		}

		public double[] YRange
		{
			get { return new double[] { MinY, MaxY }; }
			set
			{
				MinY = value[0];
				MaxY = value[1];
			}
		}

		public static string[] ColorPalette { get { return colorPalette; } }

		#endregion

		#region Methods

		public abstract void AddPoints(MatrixValue m);

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

		#endregion

		#region Statics

		readonly static string[] colorPalette = new string[]
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

		public override byte[] Serialize()
		{
			return new byte[0];
		}

		public override Value Deserialize(byte[] content)
		{
			return Value.Empty;
		}

		#endregion
	}
}
