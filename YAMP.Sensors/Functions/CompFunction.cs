namespace YAMP.Sensors
{
    using System;
    using System.Collections.Generic;
    using YAMP.Sensors.Devices;

    [Description("Provides access to the compass sensor of an Intel UltraBook™.")]
	[Kind("Sensor")]
    sealed class CompFunction : SensorFunction
    {
        static readonly Dictionary<String, Func<Compass, ScalarValue>> NamedProperties = new Dictionary<String, Func<Compass, ScalarValue>>(StringComparer.InvariantCultureIgnoreCase)
        {
            { "magneticnorth", compass => new ScalarValue(compass.CurrentHeading.Magnetic) },
            { "truenorth", compass => new ScalarValue(compass.CurrentHeading.True) },
        };

        readonly Compass _sensor = new Compass();

        /// <summary>
        /// returns heading information relative to true north in degrees
        /// </summary>
        /// <returns></returns>
        [Description("Returns heading information relative to magnetic north in degrees.")]
        [ExampleAttribute("comp()", "Returns heading information relative to magnetic north as scalar.")]
        public ScalarValue Function()
        {
            var value = _sensor.CurrentHeading;
            return new ScalarValue(value.Magnetic);
        }

        /// <summary>
        /// returns heading information relative to magnetic or true north in degrees
        /// </summary>
        /// <returns></returns>
        [Description("Returns heading information relative to magnetic or true north in degrees.")]
        [ExampleAttribute("comp(\"MagneticNorth\")", "Returns heading information relative to magnetic north as scalar.")]
        [ExampleAttribute("comp(\"TrueNorth\")", "Returns heading information relative to true north as scalar.")]
        public ScalarValue Function(StringValue option)
		{
            var callback = default(Func<Compass, ScalarValue>);

            if (NamedProperties.TryGetValue(option.Value, out callback))
            {
                return callback(_sensor);
            }

            return new ScalarValue();
        }
    }
}
