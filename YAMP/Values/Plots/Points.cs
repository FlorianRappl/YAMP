using System;
using System.Collections.Generic;

namespace YAMP
{
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
	}
}
