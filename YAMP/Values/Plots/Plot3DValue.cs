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
    /// Container for 3D plots.
    /// </summary>
	public sealed class Plot3DValue : XYZPlotValue
	{
		#region ctor

        /// <summary>
        /// Creates a new instance.
        /// </summary>
		public Plot3DValue()
		{
			InitializeBoundaries();
		}

		#endregion

		#region Properties

        /// <summary>
        /// Gets the status of the x-axis - is it logarithmic?
        /// </summary>
		[ScalarToBooleanConverter]
		[StringToBooleanConverter]
		public bool IsLogX
		{
			get;
			internal set;
		}

        /// <summary>
        /// Gets the status of the y-axis - is it logarithmic?
        /// </summary>
		[ScalarToBooleanConverter]
		[StringToBooleanConverter]
		public bool IsLogY
		{
			get;
			internal set;
		}

        /// <summary>
        /// Gets the status of the z-axis - is it logarithmic?
        /// </summary>
		[ScalarToBooleanConverter]
		[StringToBooleanConverter]
		public bool IsLogZ
		{
			get;
			internal set;
		}

		#endregion

		#region Methods

        /// <summary>
        /// Adds a series with points given in a matrix m.
        /// </summary>
        /// <param name="m">The matrix with the points (requires at least an Nx3 or 3xN matrix).</param>
		public override void AddPoints(MatrixValue m)
		{
			if (m.DimensionY == 0 || m.DimensionX == 0)
				return;

			if (m.IsVector)
				return;
			else if (m.DimensionX <= m.DimensionY && m.DimensionX == 3)
			{
				var x = ConvertY(m, 0, m.DimensionY, 0);
				var y = ConvertY(m, 0, m.DimensionY, 1);
				var z = ConvertY(m, 0, m.DimensionY, 2);
				AddValues(x, y, z);
			}
			else if(m.DimensionX > m.DimensionY && m.DimensionY == 3)
			{
				var x = ConvertX(m, 0, m.DimensionX, 0);
				var y = ConvertX(m, 0, m.DimensionX, 1);
				var z = ConvertX(m, 0, m.DimensionX, 2);
				AddValues(x, y, z);
			}
		}

        /// <summary>
        /// Adds a series with points given by 3 matrices.
        /// </summary>
        /// <param name="x">The vector with the x values.</param>
        /// <param name="y">The vector with the y values.</param>
        /// <param name="z">The vector with the z values.</param>
		public void AddPoints(MatrixValue x, MatrixValue y, MatrixValue z)
		{
			var _x = Convert(x, 0, x.Length);
			var _y = Convert(y, 0, y.Length);
			var _z = Convert(z, 0, z.Length);
			AddValues(_x, _y, _z);
		}

        /// <summary>
        /// Adds points to the plot. Either by falling back to x, y, z (requires 3 vectors),
        /// or by considering each one to be a matrix (of all the given values).
        /// </summary>
        /// <param name="x">Either the x values given in a vector or one series.</param>
        /// <param name="zs">Either 2 vector or multiple atomic series.</param>
		public void AddPoints(MatrixValue x, params MatrixValue[] zs)
		{
			double[] vx = null;
			double[] vy = null;
			double[] vz = null;

			if (x.IsVector)
			{
				if (zs.Length < 2)
					return;

				vx = Convert(x, 0, x.Length);
			}
			else
			{
				AddPoints(x);

				foreach(MatrixValue z in zs)
					AddPoints(z);

				return;
			}

			for(int i = 0; i < zs.Length; i++)
			{
				if (zs[i].IsVector)
				{
					if (vy == null)
						vy = Convert(zs[i], 0, zs[i].Length);
					else
					{
						vz = Convert(zs[i], 0, zs[i].Length);
						AddValues(vx, vy, vz);
						vx = null;
						vz = null;
					}
				}
				else if (zs[i].DimensionX <= zs[i].DimensionY && zs[i].DimensionX == 2)
				{
					vy = ConvertY(zs[i], 0, zs[i].DimensionY, 0);
					vz = ConvertY(zs[i], 0, zs[i].DimensionY, 1);
					AddValues(vx, vy, vz);
				}
				else if (zs[i].DimensionX > zs[i].DimensionY && zs[i].DimensionY == 2)
				{
					vy = ConvertX(zs[i], 0, zs[i].DimensionX, 0);
					vz = ConvertX(zs[i], 0, zs[i].DimensionX, 1);
					AddValues(vx, vy, vz);
				}
			}
		}

		void AddValues(double[] _x, double[] _y, double[] _z)
		{
			var p = new Points<PointTriple>();
			var length = Math.Min(_x.Length, Math.Min(_y.Length, _z.Length));
			var xmin = MinX;
			var xmax = MaxX;
			var ymin = MinY;
			var ymax = MaxY;
			var zmin = MinZ;
			var zmax = MaxZ;

			for (var i = 0; i < length; i++)
			{
				var x = _x[i];
				var y = _y[i];
				var z = _z[i];

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

			AddSeries(p);
		}

		#endregion

		#region Nested Type

        /// <summary>
        /// Represents a 3D point.
        /// </summary>
		public struct PointTriple
		{
            /// <summary>
            /// The x value.
            /// </summary>
			public double X;

            /// <summary>
            /// The y value.
            /// </summary>
			public double Y;

            /// <summary>
            /// The z value.
            /// </summary>
			public double Z;
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
				s.Serialize(IsLogZ);
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
						s.Serialize(points[j].Z);
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
            var p3 = new Plot3DValue();

			using (var ds = Deserializer.Create(content))
			{
				p3.Deserialize(ds);
                p3.IsLogX = ds.GetBoolean();
                p3.IsLogY = ds.GetBoolean();
                p3.IsLogZ = ds.GetBoolean();
				var length = ds.GetInt();

				for (var i = 0; i < length; i++)
				{
					var points = new Points<PointTriple>();
					points.Deserialize(ds);
					var count = ds.GetInt();

					for (int j = 0; j < count; j++)
					{
						var x = ds.GetDouble();
						var y = ds.GetDouble();
						var z = ds.GetDouble();

						points.Add(new PointTriple
						{
							X = x,
							Y = y,
							Z = z
						});
					}

                    p3.AddSeries(points);
				}
			}

			return p3;
		}

		#endregion

		#region Index

        /// <summary>
        /// Gets the series at the specified index.
        /// </summary>
        /// <param name="index">The 0-based index of the series.</param>
        /// <returns>The series (list of points and properties).</returns>
		public Points<PointTriple> this[int index]
		{
			get
			{
				return base.GetSeries(index) as Points<PointTriple>;
			}
		}

        /// <summary>
        /// Gets a certain point of the specified series.
        /// </summary>
        /// <param name="index">The 0-based index of the series.</param>
        /// <param name="point">The 0-based index of the point.</param>
        /// <returns>The point.</returns>
		public PointTriple this[int index, int point]
		{
			get
			{
				return this[index][point];
			}
		}

		#endregion
	}
}
