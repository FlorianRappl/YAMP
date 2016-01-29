namespace YAMP.Sensors
{
    using Windows.Devices.Sensors;

    [Description("Provides access to the ambient light sensor of an Intel UltraBook™.")]
	[Kind("Sensor")]
    public class LightFunction : SensorFunction
    {
        static LightSensor sensor;

        static LightFunction()
        {
            try
            {
                sensor = LightSensor.GetDefault();
            }
            catch { }
        }

        protected override void InstallReadingChangedHandler()
        {
            if (sensor != null)
                sensor.ReadingChanged += OnReadingChanged;
        }

        protected override void UninstallReadingChangedHandler()
        {
            if (sensor != null)
                sensor.ReadingChanged -= OnReadingChanged;
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

        public static double Light
        {
            get
            {
                if (sensor == null)
                    return 0.0;

                var light = sensor.GetCurrentReading();
                return light.IlluminanceInLux;
            }
        }
    }
}
