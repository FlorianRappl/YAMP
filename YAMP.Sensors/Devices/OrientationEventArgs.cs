namespace YAMP.Sensors.Devices
{
    using System;

    /// <summary>
    /// Arguments for the orientation sensor.
    /// </summary>
    public class OrientationEventArgs : EventArgs
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public OrientationEventArgs(Matrix value)
        {
            Value = value;
        }

        /// <summary>
        /// Gets the current value.
        /// </summary>
        public Matrix Value
        {
            get;
            private set;
        }
    }
}
