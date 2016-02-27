namespace YAMP.Sensors.Devices
{
    using System;

    /// <summary>
    /// Arguments for the ambient light sensor.
    /// </summary>
    public class AmbientLightEventArgs : EventArgs
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public AmbientLightEventArgs(Double value)
        {
            Value = value;
        }

        /// <summary>
        /// Gets the current value.
        /// </summary>
        public Double Value
        {
            get;
            private set;
        }
    }
}
