using System;
using System.Collections.Generic;
using YAMP.Converter;

namespace YAMP
{
	[Kind(PopularKinds.Plot)]
	[Description("Sets properties of a plot.")]
	sealed class SetFunction : SystemFunction
    {
        #region Functions

        [Description("Sets the specified (as string) field's value to a new value.")]
		[Example("set(\"title\", \"My plot...\")", "Sets the title of the last plot to My Plot....")]
		public void Function(StringValue property, Value newValue)
		{
            if (Context.LastPlot == null)
                Parser.RaiseNotification(Context, new NotificationEventArgs(NotificationType.Failure, "No plot available... Nothing changed."));
            else
			    Function(Context.LastPlot, property, newValue);
		}

		[Description("Sets the specified (as string) field's value to a new value.")]
		[Example("set(myplot, \"title\", \"My plot Title\")", "Sets the title of the plot in the variable myplot to My Plot Title.")]
		public void Function(PlotValue plot, StringValue property, Value newValue)
		{
			var propertyName = property.Value;
			AlterProperty(plot, propertyName, newValue);
            plot.RaisePlotChanged(propertyName);
            Parser.RaiseNotification(Context, new NotificationEventArgs(NotificationType.Success, "Property changed"));
		}

		[Description("Sets the specified (as string) field's value of the given series to a new value.")]
		[Example("set(1, \"color\", \"#FF0000\")", "Sets the color of series #1 of the last plot to red (hex-color).")]
		[Example("set(1, \"color\", \"rgb(255, 0, 0)\")", "Sets the color of series #1 of the last plot to red (rgb-color).")]
		[Example("set(1, \"color\", \"red\")", "Sets the color of series #1 of the last plot to red.")]
		public void Function(ScalarValue series, StringValue property, Value newValue)
		{
            if (Context.LastPlot == null)
                Parser.RaiseNotification(Context, new NotificationEventArgs(NotificationType.Failure, "No plot available... Nothing changed."));
            else
			    Function(Context.LastPlot, series, property, newValue);
		}

		[Description("Sets the specified (as string) field's values of the specified series to a new value.")]
		[Example("set(myplot, 1, \"color\", \"#FF0000\")", "Sets the color of series #1 of the plot in the variable myplot to red (hex-color).")]
		[Example("set(myplot, 1, \"color\", \"rgb(255, 0, 0)\")", "Sets the color of series #1 of the plot in the variable myplot to red (rgb-color).")]
		[Example("set(myplot, 1, \"color\", \"red\")", "Sets the color of series #1 of the plot in the variable myplot to red.")]
		public void Function(PlotValue plot, ScalarValue series, StringValue property, Value newValue)
		{
			if(plot.Count == 0)
				throw new YAMPNoSeriesAvailableException("The given plot contains no series.");

            var n = series.GetIntegerOrThrowException("series", Name);

			if(n < 1 || n > plot.Count)
				throw new YAMPArgumentRangeException("series", 1, plot.Count);

            AlterSeriesProperty(plot, n - 1, property.Value, newValue);
			plot.UpdateProperties();
            Parser.RaiseNotification(Context, new NotificationEventArgs(NotificationType.Success, "Series " + n + " changed."));
		}

		[Description("Sets the specified (as string) field's value of the given series to a new value.")]
		[Example("set(1:3, \"color\", \"#FF0000\")", "Sets the color of series #1 to #3 of the last plot to red (hex-color).")]
		[Example("set(1:2:5, \"color\", \"rgb(255, 0, 0)\")", "Sets the color of series #1, #3, #5 of the last plot to red (rgb-color).")]
		[Example("set([1,3,7], \"color\", \"red\")", "Sets the color of series #1, #3, #7 of the last plot to red.")]
		public void Function(MatrixValue series, StringValue property, Value newValue)
		{
            if (Context.LastPlot == null)
                Parser.RaiseNotification(Context, new NotificationEventArgs(NotificationType.Failure, "No plot available... Nothing changed."));
            else
			    Function(Context.LastPlot, series, property, newValue);
		}

		[Description("Sets the specified (as string) field's value to of the given series to a new value.")]
		[Example("set(myplot, 1:3, \"color\", \"#FF0000\")", "Sets the color of series #1 to #3 of the plot in the variable myplot to red (hex-color).")]
		[Example("set(myplot, 1:2:5, \"color\", \"rgb(255, 0, 0)\")", "Sets the color of series #1, #3, #5 of the plot in the variable myplot to red (rgb-color).")]
		[Example("set(myplot, [1,3,7], \"color\", \"red\")", "Sets the color of series #1, #3, #7 of the plot in the variable myplot to red.")]
		public void Function(PlotValue plot, MatrixValue series, StringValue property, Value newValue)
		{
			var s = new List<string>();

			if (series is RangeValue)
			{
				var r = series as RangeValue;
				var step = (int)r.Step;
				var end = r.All ? plot.Count : (int)r.End;

				for (var j = (int)r.Start; j <= end; j += step)
				{
					s.Add(j.ToString());
                    AlterSeriesProperty(plot, j - 1, property.Value, newValue);
				}
			}
			else
			{
				var end = series.Length;

				for (var i = 1; i <= end; i++)
                {
                    var n = series[i].GetIntegerOrThrowException("series", Name);
                    s.Add(n.ToString());
                    AlterSeriesProperty(plot, n - 1, property.Value, newValue);
				}
			}

            Parser.RaiseNotification(Context, new NotificationEventArgs(NotificationType.Failure, "Series " + string.Join(", ", s.ToArray()) + " changed."));
            plot.UpdateProperties();
		}

        #endregion

        #region Helper

        /// <summary>
        /// Changes a given property to a certain value using the available value converter.
        /// </summary>
        /// <param name="parent">The object that should contain the property.</param>
        /// <param name="name">The name of the property (property needs to have a converter specified).</param>
        /// <param name="value">The new value of the property.</param>
        public static void AlterProperty(object parent, string name, Value value)
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
                        throw new YAMPArgumentWrongTypeException(value.Header, possible.ToArray(), "set");

					prop.SetValue(parent, content, null);
					return;
				}
			}

            throw new YAMPPropertyMissingException(name, available.ToArray());
        }

        /// <summary>
        /// Changes a given property to a certain value using the available value converter.
        /// </summary>
        /// <param name="series">The series (0..(n-1)) that should be changed.</param>
        /// <param name="parent">The object that should contain the property.</param>
        /// <param name="property">The name of the property (property needs to have a converter specified).</param>
        /// <param name="value">The new value of the property.</param>
        public static void AlterSeriesProperty(object parent, int series, string property, Value value)
        {
            var type = parent.GetType();
            var props = type.GetProperties();

            foreach (var prop in props)
            {
                var idx = prop.GetIndexParameters();

                if (idx.Length == 1)
                {
                    var method = prop.GetGetMethod();
                    var s = method.Invoke(parent, new object[] { series });
                    AlterProperty(s, property, value);
                    break;
                }
            }
        }

        #endregion
    }
}
