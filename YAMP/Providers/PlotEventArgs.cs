using System;

namespace YAMP
{
    /// <summary>
    /// Contains information about which variable changed its value into what.
    /// </summary>
    public class PlotEventArgs : EventArgs
    {
        public PlotEventArgs(PlotValue value, string property)
        {
            Value = value;
            Property = property;
        }

        /// <summary>
        /// Gets the name of the variable that has been changed.
        /// </summary>
        public string Property { get; private set; }

        /// <summary>
        /// Gets the new value of the variable.
        /// </summary>
        public PlotValue Value { get; private set; }
    }
}
