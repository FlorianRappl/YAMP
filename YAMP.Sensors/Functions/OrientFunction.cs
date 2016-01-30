namespace YAMP.Sensors
{
    using System;
    using Windows.Devices.Sensors;

    [Description("Provides access to the orientation sensor of an Intel UltraBook™.")]
	[Kind("Sensor")]
    public class OrientFunction : SensorFunction
    {
        static readonly OrientationSensor sensor = GetSensor();

        private static OrientationSensor GetSensor()
        {
            try
            {
                return OrientationSensor.GetDefault();
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

        public static Double[,] RotationMatrix
        {
            get 
            {
                var values = new Double[3, 3];

                if (sensor != null)
                {
                    var orient = sensor.GetCurrentReading();
                    values[0, 0] = orient.RotationMatrix.M11;
                    values[0, 1] = orient.RotationMatrix.M12;
                    values[0, 2] = orient.RotationMatrix.M13;
                    
                    values[1, 0] = orient.RotationMatrix.M21;
                    values[1, 1] = orient.RotationMatrix.M22;
                    values[1, 2] = orient.RotationMatrix.M23;
                    
                    values[2, 0] = orient.RotationMatrix.M31;
                    values[2, 1] = orient.RotationMatrix.M32;
                    values[2, 2] = orient.RotationMatrix.M33;
                }
                
                return values;
            }
        }
    }
}
