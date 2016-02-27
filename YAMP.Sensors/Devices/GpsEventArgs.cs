namespace YAMP.Sensors.Devices
{
    using System;

    /// <summary>
    /// The GPS event args.
    /// </summary>
    public class GpsEventArgs : EventArgs
    {
        /// <summary>
        /// Creates new event args.
        /// </summary>
        public GpsEventArgs(Position value)
        {
            Value = value;
        }

        /// <summary>
        /// Gets the current value.
        /// </summary>
        public Position Value
        {
            get;
            private set;
        }
    }
}
