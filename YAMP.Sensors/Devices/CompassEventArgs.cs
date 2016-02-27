namespace YAMP.Sensors.Devices
{
    using System;

    /// <summary>
    /// Arguments for the compass.
    /// </summary>
    public class CompassEventArgs : EventArgs
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public CompassEventArgs(HeadingNorth value)
        {
            Value = value;
        }

        /// <summary>
        /// Gets the current value.
        /// </summary>
        public HeadingNorth Value
        {
            get;
            private set;
        }
    }
}
