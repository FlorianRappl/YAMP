namespace YAMP.Sensors.Devices
{
    using System;
    using Windows.Devices.Sensors;
    using Sensor = Windows.Devices.Sensors.Compass;

    public class Compass : BaseDevice
    {
        static readonly Sensor sensor = GetSensor();

        private static Sensor GetSensor()
        {
            try
            {
                return Sensor.GetDefault();
            }
            catch
            {
                return null;
            }
        }

        private event EventHandler<CompassEventArgs> changed;

        public event EventHandler<CompassEventArgs> Changed
        {
            add
            {
                InstallHandler(sensor);
                changed += value;
            }
            remove
            {
                changed -= value;
                UninstallHandler(sensor);
            }
        }

        protected override void InstallReadingChangedHandler()
        {
            sensor.ReadingChanged += OnReadingChanged;
        }

        protected override void UninstallReadingChangedHandler()
        {
            sensor.ReadingChanged -= OnReadingChanged;
        }

        void OnReadingChanged(Sensor sender, CompassReadingChangedEventArgs args)
        {
            var handler = changed;

            if (handler != null)
            {
                var value = ConvertToHeadingNorth(args.Reading);
                var e = new CompassEventArgs(value);
                handler.Invoke(this, e);
            }
        }

        public HeadingNorth CurrentHeading
        {
            get
            {
                if (sensor != null)
                {
                    var cmp = sensor.GetCurrentReading();
                    return ConvertToHeadingNorth(cmp);
                }

                return new HeadingNorth();
            }
        }

        static HeadingNorth ConvertToHeadingNorth(CompassReading reading)
        {
            return new HeadingNorth
            {
                Magnetic = reading.HeadingMagneticNorth,
                True = reading.HeadingTrueNorth ?? 0.0
            };
        }
    }
}
