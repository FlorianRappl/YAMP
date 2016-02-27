namespace YAMP.Sensors.Devices
{
    using System;
    using Windows.Devices.Sensors;
    using Sensor = Windows.Devices.Sensors.OrientationSensor;

    /// <summary>
    /// The orientation sensor.
    /// </summary>
    public class Orientation : BaseDevice
    {
        static readonly Sensor sensor = GetSensor();

        private static Sensor GetSensor()
        {
            try
            {
                return Sensor.GetDefault();
            }
            catch
            {
                return null;
            }
        }

        private event EventHandler<OrientationEventArgs> changed;

        /// <summary>
        /// Listens to the changed event.
        /// </summary>
        public event EventHandler<OrientationEventArgs> Changed
        {
            add
            {
                InstallHandler(sensor);
                changed += value;
            }
            remove
            {
                changed -= value;
                UninstallHandler(sensor);
            }
        }

        /// <summary>
        /// Installs the reading handler.
        /// </summary>
        protected override void InstallReadingChangedHandler()
        {
            sensor.ReadingChanged += OnReadingChanged;
        }

        /// <summary>
        /// Uninstalls the reading handler.
        /// </summary>
        protected override void UninstallReadingChangedHandler()
        {
            sensor.ReadingChanged -= OnReadingChanged;
        }

        void OnReadingChanged(Sensor sender, OrientationSensorReadingChangedEventArgs args)
        {
            var handler = changed;

            if (handler != null)
            {
                var value = ConvertToMatrix(args.Reading);
                var e = new OrientationEventArgs(value);
                handler.Invoke(this, e);
            }
        }

        /// <summary>
        /// Gets the value of the current rotation.
        /// </summary>
        public Matrix CurrentRotation
        {
            get
            {
                if (sensor != null)
                {
                    var reading = sensor.GetCurrentReading();
                    return ConvertToMatrix(reading);
                }

                return new Matrix();
            }
        }

        static Matrix ConvertToMatrix(OrientationSensorReading reading)
        {
            return new Matrix
            {
                Xx = reading.RotationMatrix.M11,
                Xy = reading.RotationMatrix.M12,
                Xz = reading.RotationMatrix.M13,
                Yx = reading.RotationMatrix.M21,
                Yy = reading.RotationMatrix.M22,
                Yz = reading.RotationMatrix.M23,
                Zx = reading.RotationMatrix.M31,
                Zy = reading.RotationMatrix.M32,
                Zz = reading.RotationMatrix.M33
            };
        }
    }
}
