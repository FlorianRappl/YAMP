namespace YAMP.Sensors
{
    using System;
    using Windows.Devices.Sensors;

    [Description("Provides access to the acceleration sensor of an Intel UltraBook™.")]
	[Kind("Sensor")]
    public class AccFunction : SensorFunction
    {
        static readonly Accelerometer sensor = GetSensor();

        private static Accelerometer GetSensor()
        {
            try
            {
                return Accelerometer.GetDefault();
            }
            catch 
            {
                return null;
            }
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

        public static Double[] Acceleration
        {
            get
            {
                var values = new Double[3];

                if (sensor != null)
                {
                    var acc = sensor.GetCurrentReading();
                    values[0] = acc.AccelerationX;
                    values[1] = acc.AccelerationY;
                    values[2] = acc.AccelerationZ;
                }

                return values;
            }
        }
    }
}
