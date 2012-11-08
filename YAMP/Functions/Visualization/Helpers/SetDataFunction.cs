using System;

namespace YAMP
{
    class SetDataFunction : SystemFunction
    {
        public SetDataFunction() : base(3)
        {
            offset = 3;
        }

        int offset;

        public PlotValue Function(PlotValue plot, ScalarValue series, ArgumentsValue arguments)
        {
            var index = series.IntValue;

            if (arguments.Length % 2 == 1)
                throw new ArgumentsException(Name, arguments.Length + offset);

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

        public PlotValue Function(ScalarValue series, ArgumentsValue arguments)
        {
            offset = 2;
            return Function(Context.LastPlot, series, arguments);
        }

        void ChangeProperty<T>(YAMP.Plot2DValue.Points<T> series, string property, Value value, int valueIndex)
        {
            switch (property)
            {
                case "color":
                    if (value is StringValue)
                        series.Color = (value as StringValue).Value;
                    else
                        throw new ArgumentException(Name, valueIndex, "String", value.Header);
                    break;
                case "label":
                    if (value is StringValue)
                    {
                        series.ShowLabel = true;
                        series.Label = (value as StringValue).Value;
                    }
                    else
                        throw new ArgumentException(Name, valueIndex, "String", value.Header);
                    break;
                case "symbol":
                    if (value is StringValue)
                        series.Symbol = (PlotValue.PointSymbol)System.ComponentModel.TypeDescriptor.GetConverter(typeof(PlotValue.PointSymbol)).ConvertFromString((value as StringValue).Value);
                    else
                        throw new ArgumentException(Name, valueIndex, "String", value.Header);
                    break;
                case "width":
                    if (value is ScalarValue)
                        series.LineWidth = (value as ScalarValue).Value;
                    else
                        throw new ArgumentException(Name, valueIndex, "Scalar", value.Header);
                    break;
                case "lines":
                    if (value is ScalarValue)
                        series.Lines = (value as ScalarValue).Value == 1.0;
                    else
                        throw new ArgumentException(Name, valueIndex, "Scalar", value.Header);
                    break;
                default:
                    throw new ArgumentException(Name, valueIndex, property, "{ color, label, symbol, width, lines }");
            }
        }
    }
}
