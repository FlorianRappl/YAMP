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
