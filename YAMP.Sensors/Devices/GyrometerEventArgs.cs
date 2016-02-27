namespace YAMP.Sensors.Devices
{
    using System;

    public class GyrometerEventArgs : EventArgs
    {
        public GyrometerEventArgs(Vector value)
        {
            Value = value;
        }

        public Vector Value
        {
            get;
            private set;
        }
    }
}
