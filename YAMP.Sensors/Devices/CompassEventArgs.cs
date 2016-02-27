namespace YAMP.Sensors.Devices
{
    using System;

    public class CompassEventArgs : EventArgs
    {
        public CompassEventArgs(HeadingNorth value)
        {
            Value = value;
        }

        public HeadingNorth Value
        {
            get;
            private set;
        }
    }
}
