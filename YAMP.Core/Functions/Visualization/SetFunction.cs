namespace YAMP
{
    using System;
    using System.Collections.Generic;
    using YAMP.Converter;
    using YAMP.Exceptions;

	[Kind(PopularKinds.Plot)]
    [Description("SetFunctionDescription")]
	sealed class SetFunction : SystemFunction
    {
        public SetFunction(ParseContext context)
            : base(context)
        {
        }

        #region Functions

        [Description("SetFunctionDescriptionForStringValue")]
        [Example("set(\"title\", \"My plot...\")", "SetFunctionExampleForStringValue1")]
		public void Function(StringValue property, Value newValue)
		{
            if (Context.LastPlot == null)
            {
                Context.RaiseNotification(new NotificationEventArgs(NotificationType.Failure, "No plot available... Nothing changed."));
            }
            else
            {
                Function(Context.LastPlot, property, newValue);
            }
		}

        [Description("SetFunctionDescriptionForPlotStringValue")]
        [Example("set(myplot, \"title\", \"My plot Title\")", "SetFunctionExampleForPlotStringValue1")]
		public void Function(PlotValue plot, StringValue property, Value newValue)
		{
			var propertyName = property.Value;
			AlterProperty(plot, propertyName, newValue);
            plot.RaisePlotChanged(propertyName);
            Context.RaiseNotification(new NotificationEventArgs(NotificationType.Success, "Property changed"));
		}

        [Description("SetFunctionDescriptionForScalarStringValue")]
        [Example("set(1, \"color\", \"#FF0000\")", "SetFunctionExampleForScalarStringValue1")]
        [Example("set(1, \"color\", \"rgb(255, 0, 0)\")", "SetFunctionExampleForScalarStringValue2")]
        [Example("set(1, \"color\", \"red\")", "SetFunctionExampleForScalarStringValue3")]
		public void Function(ScalarValue series, StringValue property, Value newValue)
		{
            if (Context.LastPlot == null)
            {
                Context.RaiseNotification(new NotificationEventArgs(NotificationType.Failure, "No plot available... Nothing changed."));
            }
            else
            {
                Function(Context.LastPlot, series, property, newValue);
            }
		}

        [Description("SetFunctionDescriptionForPlotScalarStringValue")]
        [Example("set(myplot, 1, \"color\", \"#FF0000\")", "SetFunctionExampleForPlotScalarStringValue1")]
        [Example("set(myplot, 1, \"color\", \"rgb(255, 0, 0)\")", "SetFunctionExampleForPlotScalarStringValue2")]
        [Example("set(myplot, 1, \"color\", \"red\")", "SetFunctionExampleForPlotScalarStringValue3")]
		public void Function(PlotValue plot, ScalarValue series, StringValue property, Value newValue)
		{
			if (plot.Count == 0)
				throw new YAMPNoSeriesAvailableException("The given plot contains no series.");

            var n = series.GetIntegerOrThrowException("series", Name);

			if (n < 1 || n > plot.Count)
				throw new YAMPArgumentRangeException("series", 1, plot.Count);

            AlterSeriesProperty(plot, n - 1, property.Value, newValue);
			plot.UpdateProperties();
            Context.RaiseNotification(new NotificationEventArgs(NotificationType.Success, "Series " + n + " changed."));
		}

        [Description("SetFunctionDescriptionForMatrixStringValue")]
        [Example("set(1:3, \"color\", \"#FF0000\")", "SetFunctionExampleForMatrixStringValue1")]
        [Example("set(1:2:5, \"color\", \"rgb(255, 0, 0)\")", "SetFunctionExampleForMatrixStringValue2")]
        [Example("set([1,3,7], \"color\", \"red\")", "SetFunctionExampleForMatrixStringValue3")]
		public void Function(MatrixValue series, StringValue property, Value newValue)
		{
            if (Context.LastPlot == null)
            {
                Context.RaiseNotification(new NotificationEventArgs(NotificationType.Failure, "No plot available... Nothing changed."));
            }
            else
            {
                Function(Context.LastPlot, series, property, newValue);
            }
		}

        [Description("SetFunctionDescriptionForPlotMatrixStringValue")]
        [Example("set(myplot, 1:3, \"color\", \"#FF0000\")", "SetFunctionExampleForPlotMatrixStringValue1")]
        [Example("set(myplot, 1:2:5, \"color\", \"rgb(255, 0, 0)\")", "SetFunctionExampleForPlotMatrixStringValue2")]
        [Example("set(myplot, [1,3,7], \"color\", \"red\")", "SetFunctionExampleForPlotMatrixStringValue3")]
		public void Function(PlotValue plot, MatrixValue series, StringValue property, Value newValue)
		{
			var s = new List<String>();

			if (series is RangeValue)
			{
				var r = series as RangeValue;
				var step = (Int32)r.Step;
                var end = r.All ? plot.Count : (Int32)r.End;

                for (var j = (Int32)r.Start; j <= end; j += step)
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

            Context.RaiseNotification(new NotificationEventArgs(NotificationType.Failure, "Series " + String.Join(", ", s.ToArray()) + " changed."));
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
        public static void AlterProperty(Object parent, String name, Value value)
		{
			var type = parent.GetType();
			var props = type.GetProperties();
			var available = new List<String>();

			foreach (var prop in props)
			{
				var attrs = prop.GetCustomAttributes(typeof(ValueConverterAttribute), false);

                if (attrs.Length != 0)
                {
                    available.Add(prop.Name);

                    if (prop.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                    {
                        var content = default(Object);
                        var possible = new List<String>();

                        foreach (ValueConverterAttribute attr in attrs)
                        {
                            possible.Add(attr.Type);

                            if (attr.CanConvertFrom(value))
                            {
                                content = attr.Convert(value);
                                break;
                            }
                        }

                        if (content == null)
                        {
                            throw new YAMPArgumentWrongTypeException(value.Header, possible.ToArray(), "set");
                        }

                        prop.SetValue(parent, content, null);
                        return;
                    }
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
        public static void AlterSeriesProperty(Object parent, Int32 series, String property, Value value)
        {
            var type = parent.GetType();
            var props = type.GetProperties();

            foreach (var prop in props)
            {
                var idx = prop.GetIndexParameters();

                if (idx.Length == 1)
                {
                    var method = prop.GetGetMethod();
                    var s = method.Invoke(parent, new Object[] { series });
                    AlterProperty(s, property, value);
                    break;
                }
            }
        }

        #endregion
    }
}
