namespace YAMP
{
    /// <summary>
    /// The visualization function is a special kind of system function, which
    /// takes care of the given plot (setting it as "LastPlot").
    /// </summary>
    public abstract class VisualizationFunction : SystemFunction
    {
        /// <summary>
        /// Creates a new visualization with the given context.
        /// </summary>
        /// <param name="context">The given context.</param>
        public VisualizationFunction(ParseContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Performs the function and takes the result (PlotValue) as LastPlot.
        /// </summary>
        /// <param name="argument">The argument for invoking the function.</param>
        /// <returns>The result of the function</returns>
        public override Value Perform(Value argument)
        {
            var value = base.Perform(argument);

            if (value != null && value is PlotValue)
            {
                Context.LastPlot = (PlotValue)value;
            }

            return value;
        }
    }
}
