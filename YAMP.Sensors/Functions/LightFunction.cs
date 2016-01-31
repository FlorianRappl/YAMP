namespace YAMP.Sensors
{
    using System;
    using Windows.Devices.Sensors;

    [Description("Provides access to the ambient light sensor of an Intel UltraBook™.")]
	[Kind("Sensor")]
    sealed class LightFunction : SensorFunction
    {
        static readonly LightSensor sensor = GetSensor();

        private static LightSensor GetSensor()
        {
            try
            {
                return LightSensor.GetDefault();
            }
            catch
            {
                return null;
            }
        }

        protected override void InstallReadingChangedHandler()
        {
            if (sensor != null)
            {
                sensor.ReadingChanged += OnReadingChanged;
            }
        }

        protected override void UninstallReadingChangedHandler()
        {
            if (sensor != null)
            {
                sensor.ReadingChanged -= OnReadingChanged;
            }
        }

        void OnReadingChanged(LightSensor sender, LightSensorReadingChangedEventArgs args)
        {
            RaiseReadingChanged(args.Reading);
        }

        /// <summary>
        /// retrieves the ambient light flux in lux
        /// </summary>
        /// <returns></returns>
        [Description("Retrieves the ambient light flux in lux.")]
        [ExampleAttribute("light()", "Returns the light flux as a scalar.")]
        public ScalarValue Function()
        {
            return new ScalarValue(Light);
        }

        public static Double Light
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
