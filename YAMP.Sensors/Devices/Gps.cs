namespace YAMP.Sensors.Devices
{
    using System;
    using Windows.Devices.Geolocation;
    using Sensor = Windows.Devices.Geolocation.Geolocator;

    /// <summary>
    /// The GPS device.
    /// </summary>
    public class Gps : BaseDevice
    {
        static readonly Sensor sensor = GetSensor();
        static Geocoordinate position;

        private static Sensor GetSensor()
        {
            try
            {
                var sensor = new Geolocator();
                InitialPosition(sensor);
                return sensor;
            }
            catch
            {
                return null;
            }
        }

        async static void InitialPosition(Sensor sensor)
        {
            try
            {
                var geopos = await sensor.GetGeopositionAsync();
                position = geopos.Coordinate;
            }
            catch { }
        }

        private event EventHandler<GpsEventArgs> changed;

        /// <summary>
        /// Listens to the changed event.
        /// </summary>
        public event EventHandler<GpsEventArgs> Changed
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
            sensor.PositionChanged += OnReadingChanged;
        }

        /// <summary>
        /// Uninstalls the reading handler.
        /// </summary>
        protected override void UninstallReadingChangedHandler()
        {
            sensor.PositionChanged -= OnReadingChanged;
        }

        void OnReadingChanged(Sensor sender, PositionChangedEventArgs args)
        {
            var handler = changed;
            var coordinate = args.Position != null ? args.Position.Coordinate : null;

            if (coordinate != null)
            {
                if (handler != null)
                {
                    var value = ConvertToPosition(coordinate);
                    var e = new GpsEventArgs(value);
                    handler.Invoke(this, e);
                }

                position = coordinate;
            }
        }

        /// <summary>
        /// Gets the current location.
        /// </summary>
        public Position CurrentLocation
        {
            get
            {
                if (position != null)
                {
                    return ConvertToPosition(position);
                }

                return new Position();
            }
        }

        static Position ConvertToPosition(Geocoordinate position)
        {
            var point = position.Point;
            var geo = point != null ? point.Position : new BasicGeoposition();

            return new Position
            {
                Altitude = geo.Altitude,
                Latitude = geo.Latitude,
                Longitude = geo.Longitude,
                Speed = position.Speed ?? 0.0
            };
        }
    }
}
