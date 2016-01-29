namespace YAMP.Sensors
{
    using Windows.Devices.Sensors;

    [Description("Provides access to the inclinometer sensor of an Intel UltraBook™.")]
	[Kind("Sensor")]
    public class IncFunction : SensorFunction
    {
        static Inclinometer sensor;

        static IncFunction()
        {
            try
            {
                sensor = Inclinometer.GetDefault();
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

        public static double[] Inclination
        {
            get
            {
                if (sensor == null)
                    return new double[3];

                var inc = sensor.GetCurrentReading();
                return new double[] { inc.PitchDegrees, inc.RollDegrees, inc.YawDegrees };
            }
        }

        public static double Pitch
        {
            get
            {
                if (sensor == null)
                    return 0.0;

                var inc = sensor.GetCurrentReading();
                return inc.PitchDegrees;
            }
        }

        public static double Roll
        {
            get {
            if (sensor == null)
                return 0.0;

            var inc = sensor.GetCurrentReading();
            return inc.RollDegrees;
            }
        }

        public static double Yaw
        {
            get {
            if (sensor == null)
                return 0.0;

                var inc = sensor.GetCurrentReading();
                return inc.YawDegrees;
            }
        }
    }
}
