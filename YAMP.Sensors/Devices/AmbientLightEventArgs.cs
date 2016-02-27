namespace YAMP.Sensors.Devices
{
    using System;

    public class AmbientLightEventArgs : EventArgs
    {
        public AmbientLightEventArgs(Double value)
        {
            Value = value;
        }

        public Double Value
        {
            get;
            private set;
        }
    }
}
