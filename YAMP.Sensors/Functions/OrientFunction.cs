namespace YAMP.Sensors
{
    using Windows.Devices.Sensors;

    [Description("Provides access to the orientation sensor of an Intel UltraBook™.")]
	[Kind("Sensor")]
    public class OrientFunction : SensorFunction
    {
        static OrientationSensor sensor;

        static OrientFunction()
        {
            try
            {
                sensor = OrientationSensor.GetDefault();
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

        void OnReadingChanged(OrientationSensor sender, OrientationSensorReadingChangedEventArgs args)
        {
            RaiseReadingChanged(args.Reading);
        }

        /// <summary>
        /// returns the 3D rotation matrix
        /// </summary>
        /// <returns></returns>
        [Description("Returns the 3D rotation matrix.")]
        [ExampleAttribute("orient()", "Returns the 3x3 orientation matrix.")]
        public MatrixValue Function()
        {
            return new MatrixValue(RotationMatrix);
        }

        public static double[,] RotationMatrix
        {
            get 
            {
                if (sensor == null)
                    return new double[3, 3];

                var orient = sensor.GetCurrentReading();
                return new double[3,3] 
                {
                    {orient.RotationMatrix.M11, orient.RotationMatrix.M12, orient.RotationMatrix.M13},
                    {orient.RotationMatrix.M21, orient.RotationMatrix.M22, orient.RotationMatrix.M23},
                    {orient.RotationMatrix.M31, orient.RotationMatrix.M32, orient.RotationMatrix.M33}
                };
            }
        }
    }
}
