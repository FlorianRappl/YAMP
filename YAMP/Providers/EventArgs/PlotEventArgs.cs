using System;

namespace YAMP
{
    /// <summary>
    /// Contains information about which variable changed its value into what.
    /// </summary>
    public class PlotEventArgs : EventArgs
    {
        #region ctor

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="value">The PlotValue for the arguments.</param>
        /// <param name="property">The name of the property.</param>
        public PlotEventArgs(PlotValue value, string property)
        {
            Value = value;
            Property = property;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the name of the variable that has been changed.
        /// </summary>
        public string Property { get; private set; }

        /// <summary>
        /// Gets the new value of the variable.
        /// </summary>
        public PlotValue Value { get; private set; }

        #endregion
    }
}
