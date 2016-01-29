namespace YAMP.Sensors
{
    using Windows.Devices.Sensors;

    [Description("Provides access to the compass sensor of an Intel UltraBook™.")]
	[Kind("Sensor")]
    public class CompFunction : SensorFunction
    {
        static Compass sensor;

        static CompFunction()
        {
            try
            {
                sensor = Compass.GetDefault();
            }
            catch { }
        }

        protected override void InstallReadingChangedHandler()
        {
            if(sensor != null)
                sensor.ReadingChanged += OnReadingChanged;
        }

        protected override void UninstallReadingChangedHandler()
        {
            if (sensor != null)
                sensor.ReadingChanged -= OnReadingChanged;
        }

        void OnReadingChanged(Compass sender, CompassReadingChangedEventArgs args)
        {
            RaiseReadingChanged(args.Reading);
        }

        /// <summary>
        /// returns heading information relative to true north in degrees
        /// </summary>
        /// <returns></returns>
        [Description("Returns heading information relative to magnetic north in degrees.")]
        [ExampleAttribute("comp()", "Returns heading information relative to magnetic north as scalar.")]
        public ScalarValue Function()
        {
            return new ScalarValue(HeadingMagneticNorth);
        }

        /// <summary>
        /// returns heading information relative to magnetic or true north in degrees
        /// </summary>
        /// <returns></returns>
        [Description("Returns heading information relative to magnetic or true north in degrees.")]
        [ExampleAttribute("comp(\"MagneticNorth\")", "Returns heading information relative to magnetic north as scalar.")]
        [ExampleAttribute("comp(\"TrueNorth\")", "Returns heading information relative to true north as scalar.")]
        public ScalarValue Function(StringValue option)
		{
			var opt = option.Value.ToLower();

            switch (opt)
            {
                case "magneticnorth":
                    return new ScalarValue(HeadingMagneticNorth);

                case "truenorth":
                    return new ScalarValue(HeadingTrueNorth);

                default:
                    return new ScalarValue();
            }
        }

        public static double HeadingMagneticNorth
        {
            get
            {
                if (sensor == null)
                    return 0.0;

                var cmp = sensor.GetCurrentReading();
                return cmp.HeadingMagneticNorth;
            }
        }

        public static double HeadingTrueNorth
        {
            get
            {
                if (sensor == null)
                    return 0.0;

                var cmp = sensor.GetCurrentReading();

                if (!cmp.HeadingTrueNorth.HasValue)
                    return 0.0;

                return cmp.HeadingTrueNorth.Value;
            }
        }
    }
}
