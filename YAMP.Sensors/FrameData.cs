namespace YAMP.Sensors
{
    using System;

    /// <summary>
    /// The video frame data.
    /// </summary>
    public struct FrameData
    {
        /// <summary>
        /// Gets or sets the red pixel values.
        /// </summary>
        public Double[,] Red;

        /// <summary>
        /// Gets or sets the green pixel values.
        /// </summary>
        public Double[,] Green;

        /// <summary>
        /// Gets or sets the blue pixel values.
        /// </summary>
        public Double[,] Blue;

        /// <summary>
        /// Gets or sets if the values have been fused.
        /// </summary>
        public Boolean IsFused;
    }
}
