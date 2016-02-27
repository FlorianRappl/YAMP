namespace YAMP.Sensors.Devices
{
    using System;
    using Windows.Devices.Sensors;
    using Sensor = Windows.Devices.Sensors.Inclinometer;

    /// <summary>
    /// The inclinometer sensor.
    /// </summary>
    public class Inclinometer : BaseDevice
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

        private event EventHandler<InclinometerEventArgs> changed;

        /// <summary>
        /// Listens to the changed event.
        /// </summary>
        public event EventHandler<InclinometerEventArgs> Changed
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

        void OnReadingChanged(Sensor sender, InclinometerReadingChangedEventArgs args)
        {
            var handler = changed;

            if (handler != null)
            {
                var value = ConvertToInclination(args.Reading);
                var e = new InclinometerEventArgs(value);
                handler.Invoke(this, e);
            }
        }

        /// <summary>
        /// Gets the current inclination.
        /// </summary>
        public Inclination CurrentGradient
        {
            get
            {
                if (sensor != null)
                {
                    var inc = sensor.GetCurrentReading();
                    return ConvertToInclination(inc);
                }

                return new Inclination();
            }
        }

        static Inclination ConvertToInclination(InclinometerReading reading)
        {
            return new Inclination
            {
                Roll = reading.RollDegrees,
                Pitch = reading.PitchDegrees,
                Yaw = reading.YawDegrees
            };
        }

    }
}
