namespace YAMP.Sensors
{
    using System;
    using System.Collections.Generic;
    using Windows.Devices.Sensors;

    [Description("Provides access to the compass sensor of an Intel UltraBook™.")]
	[Kind("Sensor")]
    sealed class CompFunction : SensorFunction
    {
        static readonly Dictionary<String, Func<ScalarValue>> NamedProperties = new Dictionary<String, Func<ScalarValue>>(StringComparer.InvariantCultureIgnoreCase)
        {
            { "magneticnorth", () => new ScalarValue(HeadingMagneticNorth) },
            { "truenorth", () => new ScalarValue(HeadingTrueNorth) },
        };

        static readonly Compass sensor = GetSensor();

        private static Compass GetSensor()
        {
            try
            {
                return Compass.GetDefault();
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
            var callback = default(Func<ScalarValue>);

            if (NamedProperties.TryGetValue(option.Value, out callback))
            {
                return callback();
            }

            return new ScalarValue();
        }

        public static Double HeadingMagneticNorth
        {
            get
            {
                if (sensor != null)
                {
                    var cmp = sensor.GetCurrentReading();
                    return cmp.HeadingMagneticNorth;
                }

                return 0.0;
            }
        }

        public static Double HeadingTrueNorth
        {
            get
            {
                if (sensor != null)
                {
                    var cmp = sensor.GetCurrentReading();

                    if (cmp.HeadingTrueNorth.HasValue)
                    {
                        return cmp.HeadingTrueNorth.Value;
                    }
                }
                    
                return 0.0;
            }
        }
    }
}
