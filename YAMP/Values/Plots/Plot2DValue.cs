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
using System.IO;
using YAMP.Converter;

namespace YAMP
{
    /// <summary>
    /// Is the type for basic 2D plotting.
    /// </summary>
    public sealed class Plot2DValue : XYPlotValue
	{
		#region ctor

        /// <summary>
        /// Creates a new instance.
        /// </summary>
		public Plot2DValue()
		{
			IsLogX = false;
			IsLogY = false;
			InitializeBoundaries();
		}

		#endregion

		#region Properties

        /// <summary>
        /// Gets or sets the status of the x-axis - is it logarithmic?
        /// </summary>
		[ScalarToBooleanConverter]
		[StringToBooleanConverter]
		public bool IsLogX
		{
			get;
			set;
		}

        /// <summary>
        /// Gets or sets the status of the y-axis - is it logarithmic?
        /// </summary>
		[ScalarToBooleanConverter]
		[StringToBooleanConverter]
		public bool IsLogY
		{
			get;
			set;
		}

		#endregion

		#region Methods

        /// <summary>
        /// Adds points given by a matrix.
        /// Vector ? x Values will be generated
        /// Matrix ? Investigates which dimension is bigger and takes the larger one as values, the lighter one as series.
        /// In a matrix the first series is always excluded and represents the x values.
        /// </summary>
        /// <param name="m">The matrix with the (multiple) series.</param>
		public override void AddPoints(MatrixValue m)
		{
			if (m.DimensionY == 0 || m.DimensionX == 0)
				return;

			if (m.IsVector)
			{
				var x = Generate(1.0, 1.0, m.Length);
				var y = Convert(m, 0, m.Length);
				AddValues(x, y);
			}
			else if(m.DimensionX <= m.DimensionY)
			{
				var x = ConvertY(m, 0, m.DimensionY, 0);

				for (var k = 2; k <= m.DimensionX; k++)
				{
					var y = ConvertY(m, 0, m.DimensionY, k - 1);
					AddValues(x, y);
				}
			}
			else
			{
				var x = ConvertX(m, 0, m.DimensionX, 0);

				for (var k = 2; k <= m.DimensionY; k++)
				{
					var y = ConvertX(m, 0, m.DimensionX, k - 1);
					AddValues(x, y);
				}
			}
		}

        /// <summary>
        /// Adds points with 2 different matrices. If x is a vector then it will
        /// represent the x values for different series that are present in y.
        /// Otherwise both matrices are interpreted as "stand-alone" and will
        /// be just added to the AddPoints that take 1 matrix.
        /// </summary>
        /// <param name="x">The first matrix, usually a vector (set of x values).</param>
        /// <param name="y">The second matrix, usually a set of y series (each series is a set of y values).</param>
		public void AddPoints(MatrixValue x, MatrixValue y)
		{
			if (x.IsVector)
			{
				var vx = Convert(x, 0, x.Length);

				if (y.DimensionY > y.DimensionX || y.DimensionY >= x.Length)
				{
					var dim = Math.Min(x.Length, y.DimensionY);

					for (var i = 0; i < y.DimensionX; i++)
					{
						var vy = ConvertY(y, 0, dim, i);
						AddValues(vx, vy);
					}
				}
				else
				{
					var dim = Math.Min(x.Length, y.DimensionX);

					for (var i = 0; i < y.DimensionY; i++)
					{
						var vy = ConvertX(y, 0, dim, i);
						AddValues(vx, vy);
					}
				}
			}
			else
			{
				AddPoints(x);
				AddPoints(y);
			}
		}

        /// <summary>
        /// Adds points with the same rules as before.
        /// </summary>
        /// <param name="x">Usually the x values (if a vector is given).</param>
        /// <param name="y">Usually a set of y-values.</param>
        /// <param name="zs">Another set of y-values.</param>
		public void AddPoints(MatrixValue x, MatrixValue y, params MatrixValue[] zs)
		{
			if (x.IsVector)
			{
				AddPoints(x, y);

				foreach (var z in zs)
					AddPoints(x, z);
			}
			else
			{
				AddPoints(x);
				AddPoints(y);

				foreach (var z in zs)
					AddPoints(z);
			}
		}

		void AddValues(double[] _x, double[] _y)
		{
			var p = new Points<PointPair>();
			var xmin = MinX;
			var xmax = MaxX;
			var ymin = MinY;
			var ymax = MaxY;

			for (var i = 0; i < _y.Length; i++)
			{
				var x = _x[i];
				var y = _y[i];

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

			AddSeries(p);
		}

		#endregion

		#region Nested types

        /// <summary>
        /// One point of a series.
        /// </summary>
		public struct PointPair
		{
            /// <summary>
            /// The x value of the point.
            /// </summary>
			public double X;

            /// <summary>
            /// The y value of the point.
            /// </summary>
			public double Y;
		}

		#endregion

		#region Serialization

        /// <summary>
        /// Converts the given instance to an array of bytes.
        /// </summary>
        /// <returns>The binary representation of this instance.</returns>
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
					}
				}

				return s.Value;
			}
		}

        /// <summary>
        /// Converts a set of bytes to a new instance.
        /// </summary>
        /// <param name="content">The binary representation.</param>
        /// <returns>The new instance.</returns>
		public override Value Deserialize(byte[] content)
		{
            var p2 = new Plot2DValue();

			using (var ds = Deserializer.Create(content))
			{
				p2.Deserialize(ds);
                p2.IsLogX = ds.GetBoolean();
                p2.IsLogY = ds.GetBoolean();
				var length = ds.GetInt();

				for (var i = 0; i < length; i++)
				{
					var points = new Points<PointPair>();
					points.Deserialize(ds);
					var count = ds.GetInt();

					for (int j = 0; j < count; j++)
					{
						var x = ds.GetDouble();
						var y = ds.GetDouble();

						points.Add(new PointPair
						{
							X = x,
							Y = y
						});
					}

                    p2.AddSeries(points);
				}
			}

			return p2;
		}

		#endregion

		#region Index

        /// <summary>
        /// Gets the series at the specified index.
        /// </summary>
        /// <param name="index">The 0-based index of the series.</param>
        /// <returns>The series (list of points and properties).</returns>
		public Points<PointPair> this[int index]
		{
			get
			{
				return base.GetSeries(index) as Points<PointPair>;
			}
		}

        /// <summary>
        /// Gets a certain point of the specified series.
        /// </summary>
        /// <param name="index">The 0-based index of the series.</param>
        /// <param name="point">The 0-based index of the point.</param>
        /// <returns>The point.</returns>
		public PointPair this[int index, int point]
		{
			get
			{
				return this[index][point];
			}
		}

		#endregion
	}
}
