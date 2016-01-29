namespace YAMP.Sensors
{
    using System;
    using Windows.Devices.Geolocation;

    [Description("Provides access to the geolocation sensor of an Intel UltraBook™.")]
	[Kind("Sensor")]
    public class GpsFunction : SensorFunction
    {
        static Geolocator sensor;
        static Geocoordinate geoCoordinate;

        static GpsFunction()
        {
            try
            {
                sensor = new Geolocator();
                InitialPosition();
            }
            catch { }
        }

        protected override void InstallReadingChangedHandler()
        {
            if(sensor != null)
                sensor.PositionChanged += OnPositionChanged;
        }

        protected override void UninstallReadingChangedHandler()
        {
            if(sensor != null)
                sensor.PositionChanged -= OnPositionChanged;
        }

        void OnPositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            geoCoordinate = args.Position.Coordinate;
            RaiseReadingChanged(args.Position);
        }

        async static void InitialPosition()
        {
            if (sensor != null)
            {
                try
                {
                    var geopos = await sensor.GetGeopositionAsync();
                    geoCoordinate = geopos.Coordinate;
                }
                catch { }
            }
        }

        /// <summary>
        /// retrieves the global position as (longitude, latitude, height) in units of (degrees, degrees, meters)
        /// </summary>
        /// <returns></returns>
        [Description("Retrieves the global position as (longitude, latitude, height) in units of (degrees, degrees, meters).")]
        [ExampleAttribute("gps()", "Returns a the global position as 3x1 matrix.")]
        public MatrixValue Function()
        {
            return new MatrixValue(Position);
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
			var opt = option.Value.ToLower();

            switch (opt)
            {
                case "longitude":
                    return new ScalarValue(Longitude);

                case "latitude":
                    return new ScalarValue(Latitude);

                case "altitude":
                    return new ScalarValue(Altitude);

                case "speed":
                    return new ScalarValue(Speed);

                default:
                    return new ScalarValue();
            }
        }

        //LAT, LNG, ALT
        public static double[] Position
        {
            get
            {
                if (sensor == null || geoCoordinate == null)
                    return new double[3];

                return new double[] { Latitude, Longitude, Altitude };
            }
        }

        public static double Longitude
        {
            get
            {
                if (sensor == null || geoCoordinate == null)
                    return 0.0;

                return geoCoordinate.Longitude;
            }
        }

        public static double Latitude
        {
            get
            {
                if (sensor == null || geoCoordinate == null)
                    return 0.0;

                return geoCoordinate.Latitude;
            }
        }

        public static double Altitude
        {
            get 
            {
                if (sensor == null || geoCoordinate == null)
                    return 0.0;

                if (!geoCoordinate.Altitude.HasValue)
                    return 0.0;

                return geoCoordinate.Altitude.Value;
            }
        }

        public static double Speed
        {
            get
            {
                if (sensor == null || geoCoordinate == null)
                    return 0.0;
                
                if(!geoCoordinate.Speed.HasValue)
                    return 0.0;

                return geoCoordinate.Speed.Value;
            }
        }
    }
}
