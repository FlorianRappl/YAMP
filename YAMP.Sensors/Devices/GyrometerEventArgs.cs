namespace YAMP.Sensors.Devices
{
    using System;

    /// <summary>
    /// Arguments for the gyrometer.
    /// </summary>
    public class GyrometerEventArgs : EventArgs
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public GyrometerEventArgs(Vector value)
        {
            Value = value;
        }

        /// <summary>
        /// Gets the current value.
        /// </summary>
        public Vector Value
        {
            get;
            private set;
        }
    }
}
