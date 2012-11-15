using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace YAMP
{
	[Kind(PopularKinds.Plot)]
	[Description("Sets data of a plot series given by the plot and / or the series.")]
	class SetDataFunction : SystemFunction
	{
		public SetDataFunction()
		{
			offset = 3;
		}

		int offset;

		[Description("Sets data of the given plot by the specified 2n arguments.")]
		[Example("setData(myplot, 1, \"color\", \"#FF0000\")", "Sets the color of series #1 of the plot in the variable myplot to red (hex-color).")]
		[Example("setData(myplot, 1, \"color\", \"rgb(255, 0, 0)\")", "Sets the color of series #1 of the plot in the variable myplot to red (rgb-color).")]
		[Example("setData(myplot, 1, \"color\", \"red\")", "Sets the color of series #1 of the plot in the variable myplot to red.")]
		[Example("setData(myplot, 2, \"symbol\", \"none\", \"width\", 4, \"lines\", 1)", "Hides the symbol of series #2 of the plot in the variable myplot and sets the width to 4 while using lines.")]
		[Arguments(2, 2, int.MaxValue, 2)]
		public PlotValue Function(PlotValue plot, ScalarValue series, ArgumentsValue arguments)
		{
			var index = series.IntValue;

			if (plot is Plot2DValue)
			{
				var p = plot as Plot2DValue;

				if (index > 0 && index <= p.Count)
				{
					var j = index - 1;
					var s = p[j];

					for (var i = 1; i <= arguments.Length; i += 2)
					{
						var label = arguments[i];

						if(!(label is StringValue))
							throw new ArgumentException(Name, i + offset, "String", label.Header);

						var property = (label as StringValue).Value.ToLower();
						ChangeProperty(s, property, arguments[i + 1], i + 1);
						Context.RaisePlotChanged(plot, j + "." + property);
					}
				}
			}

			return plot;
		}

		[Description("Sets the data of the last plot by the specified 2n arguments.")]
		[Example("setData(1, \"color\", \"#FF0000\")", "Sets the color of series #1 of the latest plot to red (hex-color).")]
		[Example("setData(1, \"color\", \"rgb(255, 0, 0)\")", "Sets the color of series #1 of the latest plot to red (rgb-color).")]
		[Example("setData(1, \"color\", \"red\")", "Sets the color of series #1 of the latest plot to red.")]
		[Example("setData(2, \"symbol\", \"none\", \"width\", 4, \"lines\", 1)", "Hides the symbol of series #2 of the latest plot and sets the width to 4 while using lines.")]
		[Arguments(1, 2, int.MaxValue, 2)]
		public PlotValue Function(ScalarValue series, ArgumentsValue arguments)
		{
			offset = 2;
			return Function(Context.LastPlot, series, arguments);
		}

		#region Modifier functions

		void ChangeProperty(IPointSeries series, string property, Value value, int valueIndex)
		{
			if (modifiers.ContainsKey(property))
			{
				var f = modifiers[property];
				var exp = f(series, value);

				if (exp != null)
					throw new ArgumentException(Name, valueIndex, exp, value.Header);

				return;
			}

			throw new ArgumentException(Name, valueIndex, property, allModifiers);
		}

		static IDictionary<string, Func<IPointSeries, Value, string>> modifiers;
		static string allModifiers;

		static SetDataFunction()
		{
			modifiers = new Dictionary<string, Func<IPointSeries, Value, string>>();

			modifiers.Add("color", (s, v) =>
			{
				if (v is StringValue)
				{
					s.Color = (v as StringValue).Value;
					return null;
				}

				return "String";
			});

			modifiers.Add("symbol", (s, v) =>
			{
				if (v is StringValue)
				{
					s.Symbol = (PointSymbol)TypeDescriptor.GetConverter(typeof(PointSymbol)).ConvertFromString((v as StringValue).Value);
					return null;
				}

				return "String";
			});

			modifiers.Add("label", (s, v) =>
			{
				if (v is StringValue)
				{
					s.ShowLabel = true;
					s.Label = (v as StringValue).Value;
					return null;
				}

				return "String";
			});

			modifiers.Add("width", (s, v) =>
			{
				if (v is ScalarValue)
				{
					s.LineWidth = (v as ScalarValue).Value;
					return null;
				}

				return "Scalar";
			});

			modifiers.Add("lines", (s, v) =>
			{
				if (v is ScalarValue)
				{
					s.Lines = (v as ScalarValue).Value == 1.0;
					return null;
				}

				return "Scalar";
			});

			var i = 0;
			var total = modifiers.Keys.Count;
			var sb = new StringBuilder();
			sb.Append("{ ");

			foreach(var key in modifiers.Keys)
			{
				sb.Append(key);

				if (++i < total)
					sb.Append(", ");
			}

			sb.Append(" }");
			allModifiers = sb.ToString();
		}

		#endregion
	}
}
