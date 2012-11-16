using System;

namespace YAMP
{
	public interface IPointSeries
	{
		bool Lines { get; set; }

		double LineWidth { get; set; }

		PointSymbol Symbol { get; set; }

		bool ShowLabel { get; set; }

		string Color { get; set; }

		string Label { get; set; }
	}
}
