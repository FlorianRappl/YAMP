using System;

namespace YAMP
{
    abstract class VisualizationFunction : SystemFunction
    {
        public override Value Perform(Value argument)
        {
            Value value = base.Perform(argument);
            Context.LastPlot = value as PlotValue;
            return value;
        }
    }
}
