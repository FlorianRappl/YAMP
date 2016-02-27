namespace YAMP.Sensors.Devices
{
    using System;

    public class GpsEventArgs : EventArgs
    {
        public GpsEventArgs(Position value)
        {
            Value = value;
        }

        public Position Value
        {
            get;
            private set;
        }
    }
}
