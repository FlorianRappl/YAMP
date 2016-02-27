namespace YAMP.Sensors
{
    using System;
    using System.Collections.Generic;
    using YAMP.Sensors.Devices;

    [Description("Provides access to the inclinometer sensor of an Intel UltraBook™.")]
	[Kind("Sensor")]
    sealed class IncFunction : SensorFunction
    {
        static readonly Dictionary<String, Func<Inclinometer, ScalarValue>> NamedProperties = new Dictionary<String, Func<Inclinometer, ScalarValue>>(StringComparer.InvariantCultureIgnoreCase)
        {
            { "pitch", inclinometer => new ScalarValue(inclinometer.CurrentGradient.Pitch) },
            { "roll", inclinometer => new ScalarValue(inclinometer.CurrentGradient.Roll) },
            { "yaw", inclinometer => new ScalarValue(inclinometer.CurrentGradient.Yaw) },
        };

        readonly Inclinometer _sensor = new Inclinometer();
        
        /// <summary>
        /// retrieves inclination around (X,Y,Z)-direction in degrees
        /// </summary>
        /// <returns></returns>
        [Description("Retrieves inclination around (X,Y,Z)-direction in degrees.")]
        [ExampleAttribute("inc()", "Returns the inclination as a 3x1 matrix.")]
        public MatrixValue Function()
        {
            var value = _sensor.CurrentGradient;
            var vector = new[] { value.Pitch, value.Roll, value.Yaw };
            return new MatrixValue(vector);
        }

        /// <summary>
        /// returns one named inclination angle of the three ("Pitch", "Roll", "Yaw")
        /// </summary>
        /// <returns></returns>
        [Description("Returns one named inclination angle of the three (\"Pitch\", \"Roll\", \"Yaw\").")]
        [ExampleAttribute("inc(\"Pitch\")", "Returns the pitch angle as a scalar.")]
        public ScalarValue Function(StringValue option)
        {
            var callback = default(Func<Inclinometer, ScalarValue>);

            if (NamedProperties.TryGetValue(option.Value, out callback))
            {
                return callback(_sensor);
            }

            return new ScalarValue();
        }
    }
}
