namespace YAMP
{
    using System;

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
        public PlotEventArgs(PlotValue value)
        {
            Value = value;
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="value">The PlotValue for the arguments.</param>
        /// <pparam name="propertyName">The name of the property.</pparam>
        public PlotEventArgs(PlotValue value, String propertyName)
        {
            Value = value;
            PropertyName = propertyName;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the new value of the variable.
        /// </summary>
        public PlotValue Value { get; private set; }

        /// <summary>
        /// Gets the property that has been changed.
        /// </summary>
        public String PropertyName { get; private set; }

        #endregion
    }
}
