using System;

namespace YAMP
{
    abstract class PropertyFunction<T> : SystemFunction where T : Value
	{
		#region Members

		string _propertyName;

		#endregion

		#region ctor

		public PropertyFunction(string propertyName)
        {
            _propertyName = propertyName;
        }

		#endregion

		#region Methods

		protected abstract object GetValue(T parameter);
        protected abstract T GetValue(object original);

        [Description("Sets the property of a given plot value.")]
        [Example("ylabel(myplot, \"Y-Axis\")", "Sets the text of the y label to be Y-Axis of the plot contained in the variable myplot.")]
        public Value Function(PlotValue plot, T parameter)
        {
            plot.GetType().GetProperty(_propertyName).SetValue(plot, GetValue(parameter), null);
            Context.RaisePlotChanged(plot, _propertyName);
            return plot;
        }

        [Description("Sets the property of the last given plot.")]
        [Example("title(\"Example\")", "Sets the title of the last plot value generated to be Example.")]
        public virtual Value Function(T parameter)
        {
            return Function(Context.LastPlot, parameter);
        }

        [Description("Gets the property of the last plot generated.")]
        [Example("xlabel()", "Gets the x label of the last plot generated.")]
        public virtual Value Function()
        {
            return Function(Context.LastPlot);
        }

        [Description("Gets the property of a given plot value.")]
        [Example("title(myplot)", "Gets the title of the plot saved in the variable myplot.")]
        public virtual Value Function(PlotValue plot)
        {
            var value = plot.GetType().GetProperty(_propertyName).GetValue(plot, null);
            return GetValue(value);
		}

		#endregion
    }
}
