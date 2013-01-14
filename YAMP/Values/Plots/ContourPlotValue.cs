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
    /// Contains the data for contourplot data.
    /// </summary>
    public sealed class ContourPlotValue : XYPlotValue
	{
		#region ctor

		public ContourPlotValue()
		{
			ShowLevel = true;
			ColorPalette = ColorPalettes.Jet;
			ShowLegend = false;
		}

		#endregion

		#region Methods

		public void SetLevels(int n)
		{
			if (Levels == null || Levels.Length == 0 || n < 2)
				return;

			SetLevels(Levels[0], Levels[Levels.Length - 1], n);
		}

		public void SetLevels(MatrixValue v)
		{
			if (v.Length < 1)
				return;

			Levels = new double[v.Length];

			for (var i = 1; i <= v.Length; i++)
				Levels[i - 1] = v[i].Value;
		}

		public void SetLevels(double zmin, double zmax, int n)
		{
			Levels = Generate(zmin, (zmax - zmin) / (n - 1), n);
		}

		public void AddPoints(MatrixValue X, MatrixValue Y, MatrixValue Z)
		{
			var x = X.GetRealVector();
			var y = Y.GetRealVector();
			var z = new double[Z.DimensionX, Z.DimensionY];

			for (var i = 1; i <= Z.DimensionX; i++)
				for (var j = 1; j <= Z.DimensionY; j++)
					z[i - 1, j - 1] = Z[j, i].Value;

			AddValues(x, y, z);
		}

		public override void AddPoints(MatrixValue M)
		{
			var x = Generate(1, 1, M.DimensionX);
			var y = Generate(1, 1, M.DimensionY);
			var z = new double[M.DimensionX, M.DimensionY];

			for (var i = 1; i <= M.DimensionX; i++)
				for (var j = 1; j <= M.DimensionY; j++)
					z[i - 1, j - 1] = M[j, i].Value;

			AddValues(x, y, z);
		}

		void AddValues(double[] _x, double[] _y, double[,] _z)
		{
			var p = new Points<ContourPoint>();

			var xmin = double.MaxValue;
			var xmax = double.MinValue;
			var ymin = double.MaxValue;
			var ymax = double.MinValue;
			var zmin = double.MaxValue;
			var zmax = double.MinValue;

			var dx = Math.Min(_x.Length, _z.GetLength(0));
			var dy = Math.Min(_y.Length, _z.GetLength(1));

			for (var i = 0; i < dx; i++)
			{
				for (var j = 0; j < dy; j++)
				{
					var x = _x[i];
					var y = _y[j];
					var z = _z[i, j];

					p.Add(new ContourPoint
					{
						X = x,
						Y = y,
						Magnitude = z
					});

					if (y < ymin)
						ymin = y;

					if (ymax < y)
						ymax = y;

					if (z< zmin)
						zmin = z;

					if (zmax < z)
						zmax = z;

					if (x < xmin)
						xmin = x;

					if (xmax < x)
						xmax = x;
				}
			}

			if (Count == 0 || xmin < MinX)
				MinX = xmin;

			if (Count == 0 || xmax > MaxX)
				MaxX = xmax;

			if (Count == 0 || ymin < MinY)
				MinY = ymin;

			if (Count == 0 || ymax > MaxY)
				MaxY = ymax;

			SetLevels(zmin, zmax, Math.Min(Math.Max(dx, dy), 10));
			AddSeries(p);
		}

		#endregion

		#region Properties

		[ScalarToBooleanConverter]
		[StringToBooleanConverter]
		public bool ShowLevel
		{
			get;
			set;
		}

		[StringToEnumConverter(typeof(ColorPalettes))]
		public ColorPalettes ColorPalette
		{
			get;
			set;
		}

		public double[] Levels
		{
			get;
			set;
		}

		#endregion

		#region Nested types

		public struct ContourPoint
		{
			public double X;
			public double Y;
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
				s.Serialize(ShowLevel);
				s.Serialize(Levels.Length);

				for (var i = 0; i < Levels.Length; i++)
					s.Serialize(Levels[i]);

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
				ShowLevel = ds.GetBoolean();
				Levels = new double[ds.GetInt()];

				for (var i = 0; i < Levels.Length; i++)
					Levels[i] = ds.GetDouble();

				var length = ds.GetInt();

				for (var i = 0; i < length; i++)
				{
					var points = new Points<ContourPoint>();
					points.Deserialize(ds);
					var count = ds.GetInt();

					for (int j = 0; j < count; j++)
					{
						var x = ds.GetDouble();
						var y = ds.GetDouble();
						var z = ds.GetDouble();

						points.Add(new ContourPoint
						{
							X = x,
							Y = y,
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

		public Points<ContourPoint> this[int index]
		{
			get
			{
				return base.GetSeries(index) as Points<ContourPoint>;
			}
		}

		public ContourPoint this[int index, int point]
		{
			get
			{
				return this[index][point];
			}
		}

		#endregion
	}
}
