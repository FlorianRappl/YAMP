namespace YAMP.Sensors.Devices
{
    using System;

    public class AccelerometerEventArgs : EventArgs
    {
        public AccelerometerEventArgs(Vector value)
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
