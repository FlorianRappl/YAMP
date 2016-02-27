namespace YAMP.Sensors.Devices
{
    using System;
    using Windows.Devices.Sensors;
    using Sensor = Windows.Devices.Sensors.LightSensor;

    public class AmbientLight : BaseDevice
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

        private event EventHandler<AmbientLightEventArgs> changed;

        public event EventHandler<AmbientLightEventArgs> Changed
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

        void OnReadingChanged(Sensor sender, LightSensorReadingChangedEventArgs args)
        {
            var handler = changed;

            if (handler != null)
            {
                var value = args.Reading.IlluminanceInLux;
                var e = new AmbientLightEventArgs(value);
                handler.Invoke(this, e);
            }
        }

        public Double CurrentLight
        {
            get
            {
                if (sensor != null)
                {
                    var light = sensor.GetCurrentReading();
                    return light.IlluminanceInLux;
                }

                return 0.0;
            }
        }
    }
}
