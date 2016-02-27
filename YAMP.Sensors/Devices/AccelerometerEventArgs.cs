namespace YAMP.Sensors.Devices
{
    using System;

    /// <summary>
    /// Arguments for the accelerometer.
    /// </summary>
    public class AccelerometerEventArgs : EventArgs
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public AccelerometerEventArgs(Vector value)
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
