using System;

namespace YAMP
{
    /// <summary>
    /// This is the basic contract for any point series.
    /// </summary>
	public interface IPointSeries
	{
        /// <summary>
        /// Gets or sets if lines should be shown.
        /// </summary>
		bool Lines { get; set; }

        /// <summary>
        /// Gets or sets the width of the lines (if shown).
        /// </summary>
		double LineWidth { get; set; }

        /// <summary>
        /// Gets or sets the symbol to use.
        /// </summary>
		PointSymbol Symbol { get; set; }

        /// <summary>
        /// Gets or sets if the label should be shown.
        /// </summary>
		bool ShowLabel { get; set; }

        /// <summary>
        /// Gets or sets the color of the series.
        /// </summary>
		string Color { get; set; }

        /// <summary>
        /// Gets or sets the label for the series.
        /// </summary>
		string Label { get; set; }
	}
}
