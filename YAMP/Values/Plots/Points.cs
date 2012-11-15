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
			Nodes = true;
			Symbol = PointSymbol.Circle;
		}

		public bool Nodes
		{
			get;
			set;
		}

		public bool Lines
		{
			get;
			set;
		}

		public double LineWidth
		{
			get;
			set;
		}

		public PointSymbol Symbol
		{
			get;
			set;
		}

		public bool ShowLabel
		{
			get;
			set;
		}

		public string Color
		{
			get;
			set;
		}

		public string Label
		{
			get;
			set;
		}
	}
}
