namespace YAMP.Sensors
{
    using Windows.Devices.Sensors;

    [Description("Provides access to the acceleration sensor of an Intel UltraBook™.")]
	[Kind("Sensor")]
    public class AccFunction : SensorFunction
    {
        static Accelerometer sensor;

        static AccFunction()
        {
            try
            {
                sensor = Accelerometer.GetDefault();
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

        void OnReadingChanged(Accelerometer sender, AccelerometerReadingChangedEventArgs args)
        {
            RaiseReadingChanged(args.Reading);
        }

        /// <summary>
        /// retrieves acceleration in (X,Y,Z)-direction in units of g
        /// </summary>
        /// <returns></returns>
        [Description("Retrieves acceleration in (X, Y, Z)-direction in units of g. Hence usually (no movement) the returned vector will be (0, 0, 1).")]
        [ExampleAttribute("acc()", "Returns a 3x1 matrix of accelerations in the x, y and z directions.")]
        public MatrixValue Function()
        {
            return new MatrixValue(Acceleration);
        }

        public static double[] Acceleration
        {
            get
            {
                if (sensor == null)
                    return new double[3];

                var acc = sensor.GetCurrentReading();
                return new double[] { acc.AccelerationX, acc.AccelerationY, acc.AccelerationZ };
            }
        }
    }
}
