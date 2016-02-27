namespace YAMP.Sensors
{
    using YAMP.Sensors.Devices;

    [Description("Provides access to the gyrometer sensor of an Intel UltraBook™.")]
	[Kind("Sensor")]
    sealed class GyroFunction : SensorFunction
    {
        readonly Gyrometer _sensor = new Gyrometer();

        /// <summary>
        /// retrieves angular velocity around the (X,Y,Z)-direction in units of degrees per second
        /// </summary>
        /// <returns></returns>
        [Description("Retrieves angular velocity around the (X,Y,Z)-direction in units of degrees per second.")]
        [ExampleAttribute("gyro()", "Returns a 3x1 matrix of angular velocities.")]
        public MatrixValue Function()
        {
            var value = _sensor.CurrentAngularVelocity;
            var vector = new[] { value.X, value.Y, value.Z };
            return new MatrixValue(vector);
        }
    }
}
