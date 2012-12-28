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
using YAMP.Converter;

namespace YAMP
{
	public sealed class BarPlotValue : PlotValue
	{
		#region Members

		#endregion

		#region ctor

		public BarPlotValue()
		{

		}

		#endregion

		#region Methods

		public override void AddPoints(MatrixValue m)
		{
			for (var i = 1; i <= m.Length; i++)
			{
				var value = 0.0;

				if (m[i].IsComplex)
					value = m[i].Abs().Value;
				else
					value = m[i].Value;

				AddSeries(new BarPoint(value));
			}
		}

		#endregion

		#region Serialization

		public override byte[] Serialize()
		{
			using (var s = Serializer.Create())
			{
				Serialize(s);
				s.Serialize(Count);

				for (var i = 0; i < Count; i++)
				{
					var points = this[i];
					points.Serialize(s);
					s.Serialize(points.Value);
					s.Serialize(points.BarWidth);
				}

				return s.Value;
			}
		}

		public override Value Deserialize(byte[] content)
		{
			using (var ds = Deserializer.Create(content))
			{
				Deserialize(ds);
				var length = ds.GetInt();

				for (var i = 0; i < length; i++)
				{
					var points = new BarPoint();
					points.Deserialize(ds);
					points.Value = ds.GetDouble();
					points.BarWidth = ds.GetDouble();
					AddSeries(points);
				}
			}

			return this;
		}

		#endregion

		#region Nested Type

		public class BarPoint : Points<double>
		{
			public BarPoint()
			{
			}

			public BarPoint(double y)
			{
				Add(y);
			}

			[ScalarToDoubleConverter]
			public double Value
			{
				get { return Count > 0 ? this[0] : 0.0; }
				set
				{
					if (Count > 0)
						this[0] = value;
					else
						Add(value);
				}
			}

			[ScalarToDoubleConverter]
			public double BarWidth
			{
				get;
				set;
			}
		}

		#endregion

		#region Index

		public BarPoint this[int index]
		{
			get
			{
				return base.GetSeries(index) as BarPoint;
			}
		}

		public double this[int index, int point]
		{
			get
			{
				return this[index][point];
			}
		}

		#endregion
	}
}
