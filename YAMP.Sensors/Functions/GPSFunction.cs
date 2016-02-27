namespace YAMP.Sensors
{
    using System;
    using System.Collections.Generic;
    using YAMP.Sensors.Devices;

    [Description("Provides access to the geolocation sensor of an Intel UltraBook™.")]
	[Kind("Sensor")]
    sealed class GpsFunction : SensorFunction
    {
        static readonly Dictionary<String, Func<Gps, ScalarValue>> NamedProperties = new Dictionary<String, Func<Gps, ScalarValue>>(StringComparer.InvariantCultureIgnoreCase)
        {
            { "longitude", gps => new ScalarValue(gps.CurrentLocation.Longitude) },
            { "latitude", gps => new ScalarValue(gps.CurrentLocation.Latitude) },
            { "altitude", gps => new ScalarValue(gps.CurrentLocation.Altitude) },
            { "speed", gps => new ScalarValue(gps.CurrentLocation.Speed) },
        };

        readonly Gps _sensor = new Gps();

        /// <summary>
        /// retrieves the global position as (longitude, latitude, height) in units of (degrees, degrees, meters)
        /// </summary>
        /// <returns></returns>
        [Description("Retrieves the global position as (longitude, latitude, height) in units of (degrees, degrees, meters).")]
        [ExampleAttribute("gps()", "Returns a the global position as 3x1 matrix.")]
        public MatrixValue Function()
        {
            var position = _sensor.CurrentLocation;
            var vector = new[] { position.Longitude, position.Latitude, position.Altitude };
            return new MatrixValue(vector);
        }

        /// <summary>
        /// retrieves either the longitude in degrees (option "Longitude"),
        /// the latitude in degrees (option "Latitude"),
        /// the altitude in meters (option "Altitude"), or
        /// the speed in meters per second (option "Speed")
        /// </summary>
        /// <returns></returns>
        [Description("Retrieves one of the values for \"Longitude\" (in degrees), \"Latitude\" (in degrees), \"Altitude\" (in meter), or \"Speed\" (in meter per second).")]
        [ExampleAttribute("gps(\"Speed\")", "Returns the current speed as scalar.")]
        public ScalarValue Function(StringValue option)
        {
            var callback = default(Func<Gps, ScalarValue>);

            if (NamedProperties.TryGetValue(option.Value, out callback))
            {
                return callback(_sensor);
            }

            return new ScalarValue();
        }
    }
}
