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
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace YAMP
{
	[Serializable]
    public class Plot2DValue : PlotValue<Plot2DValue.PointPair>
	{
		#region ctor

		public Plot2DValue()
		{
			IsLogX = false;
			IsLogY = false;
		}

		public Plot2DValue(SerializationInfo info, StreamingContext ctxt) : base(info, ctxt)
		{
			IsLogX = (bool)info.GetValue("IsLogX", typeof(bool));
			IsLogY = (bool)info.GetValue("IsLogY", typeof(bool));

		}

		#endregion

		#region Properties

		[ScalarToBooleanConverter]
		public bool IsLogX
		{
			get;
			set;
		}

		[ScalarToBooleanConverter]
		public bool IsLogY
		{
			get;
			set;
		}

		#endregion

        #region Methods

        public override void AddPoints(MatrixValue m)
        {
            if (m.DimensionY == 0 || m.DimensionX == 0)
                return;

            if (m.DimensionX == 1)
            {
                var x = Generate(1.0, 1.0, m.DimensionY);
                var y = ConvertY(m, 0, m.DimensionY, 0);
                AddValues(x, y);
            }
            else
            {
                var x = ConvertY(m, 0, m.DimensionY, 0);

                for (var k = 2; k <= m.DimensionX; k++)
                {
                    var y = ConvertY(m, 0, m.DimensionY, k - 1);
                    AddValues(x, y);
                }
            }
        }

        public void AddPoints(MatrixValue x, MatrixValue y)
        {
            if (x.DimensionX > 1 && x.DimensionY > 1)
            {
                AddPoints(x);
                AddPoints(y);
            }
            else
            {
                var transpose = y.DimensionY == 1 && y.DimensionX > 1;
                var dim = Math.Min(x.Length, transpose ? y.DimensionX : y.DimensionY);
                var _x = Convert(x, 0, dim);

                for (var k = 1; k <= y.DimensionX; k++)
                {
                    var _y = transpose ? ConvertX(y, 0, dim, k - 1) : ConvertY(y, 0, dim, k - 1);
                    AddValues(_x, _y);
                }
            }
        }

        void AddValues(double[] _x, double[] _y)
        {
            var p = new Points<PointPair>();
            var xmin = double.MaxValue;
            var xmax = double.MinValue;
            var ymin = double.MaxValue;
            var ymax = double.MinValue;

            for (var i = 0; i < _x.Length; i++)
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

            AddValues(p);
        }

		#endregion

        #region Nested types

		[Serializable]
        public struct PointPair
        {
            public double X;
            public double Y;
        }

		#endregion

		#region Serialization

		public override Value Deserialize(byte[] content)
		{
			var o = BinaryDeserialize(content) as Plot2DValue;

			if (o == null)
				return Value.Empty;

			return o;
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext ctxt)
		{
			base.GetObjectData(info, ctxt);
			info.AddValue("IsLogX", IsLogX);
			info.AddValue("IsLogY", IsLogY);
		}

		#endregion
    }
}
