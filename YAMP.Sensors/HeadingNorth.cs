namespace YAMP.Sensors
{
    using System;

    /// <summary>
    /// The compass data.
    /// </summary>
    public struct HeadingNorth
    {
        /// <summary>
        /// Gets or sets the magnetic north-pole angle.
        /// </summary>
        public Double Magnetic;

        /// <summary>
        /// Gets or sets the true north-pole angle.
        /// </summary>
        public Double True;
    }
}
