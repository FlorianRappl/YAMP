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
using YAMP.Converter;

namespace YAMP
{
    /// <summary>
    /// Contains the data for barplots.
    /// </summary>
    public sealed class BarPlotValue : XYPlotValue
	{
		#region ctor

        /// <summary>
        /// Creates a new instance.
        /// </summary>
		public BarPlotValue()
        {
            MinX = 1.0;
            MaxX = 1.0;
            MinY = 0.0;
            MaxY = 0.0;
		}

		#endregion

		#region Methods

        /// <summary>
        /// Adds (multiple) series in form of a matrix.
        /// </summary>
        /// <param name="m">The matrix which contains the values.</param>
		public override void AddPoints(MatrixValue m)
		{
            if (m.IsVector)
                AddSingleSeries(m);
            else
            {
                if (m.DimensionX < m.DimensionY)
                    m = m.Transpose();

                //From here on m.DimensionX >= m.DimensionY !
                for (var j = 1; j <= m.DimensionY; j++)
                {
                    var vec = m.GetRowVector(j);
                    AddSingleSeries(vec);
                }
            }
		}

        /// <summary>
        /// Adds a single series explicitly.
        /// </summary>
        /// <param name="vec">The matrix seen as a vector.</param>
        public void AddSingleSeries(MatrixValue vec)
        {
            var values = new BarPoints();

            for (var i = 1; i <= vec.Length; i++)
            {
                var value = 0.0;

                if (vec[i].IsComplex)
                    value = vec[i].Abs();
                else
                    value = vec[i].Re;

                if (value < MinY)
                    MinY = value;
                else if (value > MaxY)
                    MaxY = value;

                if (i > MaxX)
                    MaxX = i;

                values.Add(value);
            }

            AddSeries(values);
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
				s.Serialize(Count);

				for (var i = 0; i < Count; i++)
				{
					var points = this[i];
                    points.Serialize(s);
                    s.Serialize(points.Count);

                    for(var j = 0; j < points.Count; j++)
					    s.Serialize(points[j]);

					s.Serialize(points.BarWidth);
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
            var bp = new BarPlotValue();

			using (var ds = Deserializer.Create(content))
			{
				bp.Deserialize(ds);
				var length = ds.GetInt();

				for (var i = 0; i < length; i++)
				{
					var points = new BarPoints();
					points.Deserialize(ds);
                    var count = ds.GetInt();

                    for(var j = 0; j < count; j++)
					    points.Add(ds.GetDouble());

					points.BarWidth = ds.GetDouble();
					bp.AddSeries(points);
				}
			}

			return bp;
		}

		#endregion

		#region Nested Type

        /// <summary>
        /// The representation of one series.
        /// </summary>
		public class BarPoints : Points<double>
		{
            /// <summary>
            /// Creates a new bar series.
            /// </summary>
			public BarPoints()
			{
				BarWidth = 1.0;
			}

            /// <summary>
            /// Gets or sets the relative width of the bars.
            /// </summary>
			[ScalarToDoubleConverter]
			public double BarWidth
			{
				get;
				set;
			}
		}

		#endregion

		#region Index

        /// <summary>
        /// Gets the series at the specified index.
        /// </summary>
        /// <param name="index">The 0-based index of the series.</param>
        /// <returns>The series (list of points and properties).</returns>
		public BarPoints this[int index]
		{
			get
			{
				return base.GetSeries(index) as BarPoints;
			}
		}

        /// <summary>
        /// Gets a certain point of the specified series.
        /// </summary>
        /// <param name="index">The 0-based index of the series.</param>
        /// <param name="point">The 0-based index of the point.</param>
        /// <returns>The point.</returns>
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
