using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace YAMP
{
	[Serializable]
	public class Points<T> : List<T>, IPointSeries
	{
		public Points()
		{
			Color = "black";
			Label = "Data";
			ShowLabel = false;
			LineWidth = 1.0;
			Lines = false;
			Symbol = PointSymbol.Circle;
		}

		public Points(SerializationInfo info, StreamingContext ctxt)
		{
			Lines = (bool)info.GetValue("Lines", typeof(bool));
			ShowLabel = (bool)info.GetValue("ShowLabel", typeof(bool));
			Color = (string)info.GetValue("Color", typeof(string));
			Label = (string)info.GetValue("Label", typeof(string));
			LineWidth = (double)info.GetValue("LineWidth", typeof(double));
			Symbol = (PointSymbol)info.GetValue("Symbol", typeof(PointSymbol));
			var data = (T[])info.GetValue("Data", typeof(T[]));
			AddRange(data);
		}

		[ScalarToBooleanConverter]
		public bool Lines
		{
			get;
			set;
		}

		[ScalarToDoubleConverter]
		public double LineWidth
		{
			get;
			set;
		}

		[StringToEnumConverter(typeof(PointSymbol))]
		public PointSymbol Symbol
		{
			get;
			set;
		}

		[ScalarToBooleanConverter]
		public bool ShowLabel
		{
			get;
			set;
		}

		[StringToStringConverter]
		public string Color
		{
			get;
			set;
		}

		[StringToStringConverter]
		public string Label
		{
			get;
			set;
		}

		public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
		{
			var data = ToArray();
			info.AddValue("Label", Label);
			info.AddValue("Color", Color);
			info.AddValue("ShowLabel", ShowLabel);
			info.AddValue("Symbol", Symbol);
			info.AddValue("LineWidth", LineWidth);
			info.AddValue("Lines", Lines);
			info.AddValue("Data", data);
		}
	}
}
