namespace YAMP.Sensors.Devices
{
    using System;

    public class InclinometerEventArgs : EventArgs
    {
        public InclinometerEventArgs(Inclination value)
        {
            Value = value;
        }

        public Inclination Value
        {
            get;
            private set;
        }
    }
}
