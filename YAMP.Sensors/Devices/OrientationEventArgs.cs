namespace YAMP.Sensors.Devices
{
    using System;

    public class OrientationEventArgs : EventArgs
    {
        public OrientationEventArgs(Matrix value)
        {
            Value = value;
        }

        public Matrix Value
        {
            get;
            private set;
        }
    }
}
