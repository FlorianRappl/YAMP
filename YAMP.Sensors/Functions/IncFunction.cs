namespace YAMP.Sensors
{
    using System;
    using Windows.Devices.Sensors;

    [Description("Provides access to the inclinometer sensor of an Intel UltraBook™.")]
	[Kind("Sensor")]
    sealed class IncFunction : SensorFunction
    {
        static readonly Inclinometer sensor = GetSensor();

        private static Inclinometer GetSensor()
        {
            try
            {
                return Inclinometer.GetDefault();
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

        void OnReadingChanged(Inclinometer sender, InclinometerReadingChangedEventArgs args)
        {
            RaiseReadingChanged(args.Reading);
        }

        /// <summary>
        /// retrieves inclination around (X,Y,Z)-direction in degrees
        /// </summary>
        /// <returns></returns>
        [Description("Retrieves inclination around (X,Y,Z)-direction in degrees.")]
        [ExampleAttribute("inc()", "Returns the inclination as a 3x1 matrix.")]
        public MatrixValue Function()
        {
            return new MatrixValue(Inclination);
        }

        /// <summary>
        /// returns one named inclination angle of the three ("Pitch", "Roll", "Yaw")
        /// </summary>
        /// <returns></returns>
        [Description("Returns one named inclination angle of the three (\"Pitch\", \"Roll\", \"Yaw\").")]
        [ExampleAttribute("inc(\"Pitch\")", "Returns the pitch angle as a scalar.")]
        public ScalarValue Function(StringValue option)
        {
            switch (option.ToString())
            {
                case "Pitch":
                    return new ScalarValue(Pitch);

                case "Roll":
                    return new ScalarValue(Roll);

                case "Yaw":
                    return new ScalarValue(Yaw);

                default:
                    return new ScalarValue();
            }
        }

        public static Double[] Inclination
        {
            get
            {
                var values = new Double[3];

                if (sensor != null)
                {
                    var inc = sensor.GetCurrentReading();
                    values[0] = inc.PitchDegrees;
                    values[1] = inc.RollDegrees;
                    values[2] = inc.YawDegrees;
                }

                return values;
            }
        }

        public static Double Pitch
        {
            get
            {
                if (sensor != null)
                {
                    var inc = sensor.GetCurrentReading();
                    return inc.PitchDegrees;
                }

                return 0.0;
            }
        }

        public static Double Roll
        {
            get 
            {
                if (sensor != null)
                {
                    var inc = sensor.GetCurrentReading();
                    return inc.RollDegrees;
                }

                return 0.0;
            }
        }

        public static Double Yaw
        {
            get 
            {
                if (sensor != null)
                {
                    var inc = sensor.GetCurrentReading();
                    return inc.YawDegrees;
                }

                return 0.0;
            }
        }
    }
}
