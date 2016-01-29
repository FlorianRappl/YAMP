namespace YAMP.Sensors
{
    using Windows.Devices.Sensors;

    [Description("Provides access to the gyrometer sensor of an Intel UltraBook™.")]
	[Kind("Sensor")]
    public class GyroFunction : SensorFunction
    {
        static Gyrometer sensor;

        static GyroFunction()
        {
            try
            {
                sensor = Gyrometer.GetDefault();
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

        void OnReadingChanged(Gyrometer sender, GyrometerReadingChangedEventArgs args)
        {
            RaiseReadingChanged(args.Reading);
        }

        /// <summary>
        /// retrieves angular velocity around the (X,Y,Z)-direction in units of degrees per second
        /// </summary>
        /// <returns></returns>
        [Description("Retrieves angular velocity around the (X,Y,Z)-direction in units of degrees per second.")]
        [ExampleAttribute("gyro()", "Returns a 3x1 matrix of angular velocities.")]
        public MatrixValue Function()
        {
            return new MatrixValue(AngularVelocity);
        }

        public static double[] AngularVelocity
        {
            get
            {
                if (sensor == null)
                    return new double[3];

                var gyro = sensor.GetCurrentReading();
                return new double[] { gyro.AngularVelocityX, gyro.AngularVelocityY, gyro.AngularVelocityZ };
            }
        }
    }
}
