namespace YAMP.Sensors
{
    using System;
    using Windows.Devices.Sensors;

    [Description("Provides access to the gyrometer sensor of an Intel UltraBook™.")]
	[Kind("Sensor")]
    sealed class GyroFunction : SensorFunction
    {
        static readonly Gyrometer sensor = GetSensor();

        private static Gyrometer GetSensor()
        {
            try
            {
                return Gyrometer.GetDefault();
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

        public static Double[] AngularVelocity
        {
            get
            {
                var values = new Double[3];

                if (sensor != null)
                {
                    var gyro = sensor.GetCurrentReading();
                    values[0] = gyro.AngularVelocityX;
                    values[1] = gyro.AngularVelocityY;
                    values[2] = gyro.AngularVelocityZ;
                }

                return values;
            }
        }
    }
}
