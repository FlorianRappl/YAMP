using System;
using System.Collections.Generic;
using YAMP.Converter;

namespace YAMP
{
	[Kind(PopularKinds.Plot)]
	[Description("Sets properties of a plot.")]
	class SetFunction : SystemFunction
	{
		[Description("Sets the specified (as string) field's value to a new value.")]
		[Example("set(\"title\", \"My plot...\")", "Sets the title of the last plot to My Plot....")]
		public Value Function(StringValue property, Value newValue)
		{
			return Function(Context.LastPlot, property, newValue);
		}

		[Description("Sets the specified (as string) field's value to a new value.")]
		[Example("set(myplot, \"title\", \"My plot Title\")", "Sets the title of the plot in the variable myplot to My Plot Title.")]
		public Value Function(PlotValue plot, StringValue property, Value newValue)
		{
			var propertyName = property.Value;
			AlterProperty(plot, propertyName, newValue);
			plot.RaisePlotChanged(propertyName);
			return newValue;
		}

		[Description("Sets the specified (as string) field's value of the given series to a new value.")]
		[Example("set(1, \"color\", \"#FF0000\")", "Sets the color of series #1 of the last plot to red (hex-color).")]
		[Example("set(1, \"color\", \"rgb(255, 0, 0)\")", "Sets the color of series #1 of the last plot to red (rgb-color).")]
		[Example("set(1, \"color\", \"red\")", "Sets the color of series #1 of the last plot to red.")]
		public Value Function(ScalarValue series, StringValue property, Value newValue)
		{
			return Function(Context.LastPlot, series, property, newValue);
		}

		[Description("Sets the specified (as string) field's values of the specified series to a new value.")]
		[Example("set(myplot, 1, \"color\", \"#FF0000\")", "Sets the color of series #1 of the plot in the variable myplot to red (hex-color).")]
		[Example("set(myplot, 1, \"color\", \"rgb(255, 0, 0)\")", "Sets the color of series #1 of the plot in the variable myplot to red (rgb-color).")]
		[Example("set(myplot, 1, \"color\", \"red\")", "Sets the color of series #1 of the plot in the variable myplot to red.")]
		public Value Function(PlotValue plot, ScalarValue series, StringValue property, Value newValue)
		{
			if(plot.Count == 0)
				throw new ArgumentOutOfRangeException("The given plot contains no series.");

			if(series.IntValue < 1 || series.IntValue > plot.Count)
				throw new ArgumentOutOfRangeException(string.Format("The value for series must be between {0} and {1}.", 1, plot.Count));

			var type = plot.GetType();
			var props = type.GetProperties();

			foreach (var prop in props)
			{
				var idx = prop.GetIndexParameters();

				if (idx.Length == 1)
				{
					var method = prop.GetGetMethod();
					var s = method.Invoke(plot, new object[] { series.IntValue - 1 });
					AlterProperty(s, property.Value, newValue);
					break;
				}
			}

			plot.UpdateProperties();
			return newValue;
		}

		[Description("Sets the specified (as string) field's value of the given series to a new value.")]
		[Example("set(1:3, \"color\", \"#FF0000\")", "Sets the color of series #1 to #3 of the last plot to red (hex-color).")]
		[Example("set(1:2:5, \"color\", \"rgb(255, 0, 0)\")", "Sets the color of series #1, #3, #5 of the last plot to red (rgb-color).")]
		[Example("set([1,3,7], \"color\", \"red\")", "Sets the color of series #1, #3, #7 of the last plot to red.")]
		public Value Function(MatrixValue series, StringValue property, Value newValue)
		{
			return Function(Context.LastPlot, series, property, newValue);
		}

		[Description("Sets the specified (as string) field's value to of the given series to a new value.")]
		[Example("set(myplot, 1:3, \"color\", \"#FF0000\")", "Sets the color of series #1 to #3 of the plot in the variable myplot to red (hex-color).")]
		[Example("set(myplot, 1:2:5, \"color\", \"rgb(255, 0, 0)\")", "Sets the color of series #1, #3, #5 of the plot in the variable myplot to red (rgb-color).")]
		[Example("set(myplot, [1,3,7], \"color\", \"red\")", "Sets the color of series #1, #3, #7 of the plot in the variable myplot to red.")]
		public Value Function(PlotValue plot, MatrixValue series, StringValue property, Value newValue)
		{
			if (series is RangeValue)
			{
				var r = series as RangeValue;
				var step = (int)r.Step;
				var end = r.All ? plot.Count : (int)r.End;

				for (var j = (int)r.Start; j <= end; j += step)
					Function(plot, new ScalarValue(j), property, newValue);
			}
			else
			{
				var end = series.Length;

				for (var i = 1; i <= end; i++)
					Function(plot, series[i], property, newValue);
			}

			return newValue;
		}

		void AlterProperty(object parent, string name, Value value)
		{
			var type = parent.GetType();
			var props = type.GetProperties();
			var available = new List<string>();

			foreach (var prop in props)
			{
				var attrs = prop.GetCustomAttributes(typeof(ValueConverterAttribute), false);
					
				if(attrs.Length == 0)
					continue;

				available.Add(prop.Name);

				if (prop.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
				{
					object content = null;
					var possible = new List<string>();

					foreach(ValueConverterAttribute attr in attrs)
					{
						possible.Add(attr.Type);

						if(attr.CanConvertFrom(value))
						{
							content = attr.Convert(value);
							break;
						}
					}

					if (content == null)
						throw new ArgumentException(Name, Length, "[ " + string.Join(", ", possible.ToArray()) + " ]", value.Header);

					prop.SetValue(parent, content, null);
					return;
				}
			}

			throw new PropertyNotFoundException(name, available.ToArray());
		}
	}
}
