namespace YAMP.Sensors
{
    using YAMP.Sensors.Devices;

    [Description("Provides access to the acceleration sensor of an Intel UltraBook™.")]
	[Kind("Sensor")]
    sealed class AccFunction : SensorFunction
    {
        readonly Accelerometer _sensor = new Accelerometer();

        /// <summary>
        /// Retrieves acceleration in (X,Y,Z)-direction in units of g.
        /// </summary>
        [Description("Retrieves acceleration in (X, Y, Z)-direction in units of g. Hence usually (no movement) the returned vector will be (0, 0, 1).")]
        [ExampleAttribute("acc()", "Returns a 3x1 matrix of accelerations in the x, y and z directions.")]
        public MatrixValue Function()
        {
            var value = _sensor.CurrentAcceleration;
            var vector = new[] { value.X, value.Y, value.Z };
            return new MatrixValue(vector);
        }
    }
}
