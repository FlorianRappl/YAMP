namespace YAMP.Sensors.Devices
{
    using System;

    /// <summary>
    /// Arguments for the inclinometer.
    /// </summary>
    public class InclinometerEventArgs : EventArgs
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public InclinometerEventArgs(Inclination value)
        {
            Value = value;
        }

        /// <summary>
        /// Gets the current value.
        /// </summary>
        public Inclination Value
        {
            get;
            private set;
        }
    }
}
