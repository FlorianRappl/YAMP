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
    /// Abstract base class for plot values that are based on an XYZ
    /// (or similar) coordinate system.
    /// </summary>
    public abstract class XYZPlotValue : XYPlotValue
    {
        #region ctor

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public XYZPlotValue()
        {
            ZLabel = "z";
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the pairs of minimum and maximum x, y coordinates.
        /// </summary>
        [MatrixToDoubleArrayConverter]
        public override double[] View
        {
            get { return new double[] { MinX, MaxX, MinY, MaxY, MinZ, MaxZ }; }
            set
            {
                var elements = MakeArrayPeriodic(value, 6);

                MinX = elements[0];
                MaxX = elements[1];
                MinY = elements[2];
                MaxY = elements[3];
                MinZ = elements[4];
                MaxZ = elements[5];
            }
        }

        /// <summary>
        /// Gets or sets the label of the z axis.
        /// </summary>
        [StringToStringConverter]
        public string ZLabel
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the minimum z coordinate.
        /// </summary>
        [ScalarToDoubleConverter]
        public double MinZ
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the maximum z coordinate.
        /// </summary>
        [ScalarToDoubleConverter]
        public double MaxZ
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the pair of minimum and maximum z coordinates.
        /// </summary>
        [MatrixToDoubleArrayConverter]
        public double[] ZRange
        {
            get { return new double[] { MinZ, MaxZ }; }
            set
            {
                var elements = MakeArrayPeriodic(value, 2);
                MinZ = elements[0];
                MaxZ = elements[1];
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the values MinX, MaxX, MinY, MaxY, MinZ and MaxZ.
        /// </summary>
        protected override void InitializeBoundaries()
        {
            base.InitializeBoundaries();
            MinZ = double.MaxValue;
            MaxZ = double.MinValue;
        }

        /// <summary>
        /// Sets the z-range (min and max) in one statement.
        /// </summary>
        /// <param name="min">The minimum for the z-axis.</param>
        /// <param name="max">The maximum for the z-axis.</param>
        public void SetZRange(double min, double max)
        {
            MinZ = min;
            MaxZ = max;
        }

        #endregion

        #region Serialization

        internal override void Serialize(Serializer s)
        {
            base.Serialize(s);
            s.Serialize(MinZ);
            s.Serialize(MaxZ);
            s.Serialize(ZLabel);
        }

        internal override void Deserialize(Deserializer ds)
        {
            base.Deserialize(ds);
            MinZ = ds.GetDouble();
            MaxZ = ds.GetDouble();
            ZLabel = ds.GetString();
        }

        #endregion
    }
}
