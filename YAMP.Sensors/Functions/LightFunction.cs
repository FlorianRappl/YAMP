namespace YAMP.Sensors
{
    using YAMP.Sensors.Devices;

    [Description("Provides access to the ambient light sensor of an Intel UltraBook™.")]
	[Kind("Sensor")]
    sealed class LightFunction : SensorFunction
    {
        readonly AmbientLight _sensor = new AmbientLight();

        /// <summary>
        /// retrieves the ambient light flux in lux
        /// </summary>
        /// <returns></returns>
        [Description("Retrieves the ambient light flux in lux.")]
        [ExampleAttribute("light()", "Returns the light flux as a scalar.")]
        public ScalarValue Function()
        {
            var value = _sensor.CurrentLight;
            return new ScalarValue(value);
        }
    }
}
