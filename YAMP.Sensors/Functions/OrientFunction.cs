namespace YAMP.Sensors
{
    using YAMP.Sensors.Devices;

    [Description("Provides access to the orientation sensor of an Intel UltraBook™.")]
	[Kind("Sensor")]
    sealed class OrientFunction : SensorFunction
    {
        readonly Orientation _sensor = new Orientation();

        /// <summary>
        /// returns the 3D rotation matrix
        /// </summary>
        /// <returns></returns>
        [Description("Returns the 3D rotation matrix.")]
        [ExampleAttribute("orient()", "Returns the 3x3 orientation matrix.")]
        public MatrixValue Function()
        {
            var value = _sensor.CurrentRotation;
            var matrix = new [,]
            {
                { value.Xx, value.Xy, value.Xz },
                { value.Yx, value.Yy, value.Yz },
                { value.Zx, value.Zy, value.Zz },
            };
            return new MatrixValue(matrix);
        }
    }
}
