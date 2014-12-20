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
    /// Generic points container for the various plot types.
    /// </summary>
    /// <typeparam name="T"></typeparam>
	public class Points<T> : List<T>, IPointSeries
    {
        #region ctor

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public Points()
		{
			Color = "black";
			Label = "Data";
			ShowLabel = false;
			LineWidth = 1.0;
			Lines = false;
			Symbol = PointSymbol.Circle;
		}

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets if the points are connected.
        /// </summary>
        [ScalarToBooleanConverter]
		[StringToBooleanConverter]
		public bool Lines
		{
			get;
			set;
		}

        /// <summary>
        /// Gets or sets the width of the connected line.
        /// </summary>
		[ScalarToDoubleConverter]
		public double LineWidth
		{
			get;
			set;
		}

        /// <summary>
        /// Gets or sets the symbol for the points.
        /// </summary>
		[StringToEnumConverter(typeof(PointSymbol))]
		public PointSymbol Symbol
		{
			get;
			set;
		}

        /// <summary>
        /// Gets or sets the status for visibility of the label.
        /// </summary>
		[ScalarToBooleanConverter]
		[StringToBooleanConverter]
		public bool ShowLabel
		{
			get;
			set;
		}

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
		[StringToStringConverter]
		public string Color
		{
			get;
			set;
		}
        
        /// <summary>
        /// Gets or sets the label for the series.
        /// </summary>
		[StringToStringConverter]
		public string Label
		{
			get;
			set;
		}

        #endregion

        #region Serialization

        internal void Serialize(Serializer s)
        {
            s.Serialize(Label);
            s.Serialize(Color);
            s.Serialize(ShowLabel);
            s.Serialize((int)Symbol);
            s.Serialize(LineWidth);
            s.Serialize(Lines);
        }

        internal void Deserialize(Deserializer ds)
        {
            Label = ds.GetString();
            Color = ds.GetString();
            ShowLabel = ds.GetBoolean();
            Symbol = (PointSymbol)ds.GetInt();
            LineWidth = ds.GetDouble();
            Lines = ds.GetBoolean();
        }

        #endregion
    }
}
