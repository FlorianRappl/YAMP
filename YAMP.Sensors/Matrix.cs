namespace YAMP.Sensors
{
    using System;

    /// <summary>
    /// The orientation matrix.
    /// </summary>
    public struct Matrix
    {
        /// <summary>
        /// Gets or sets the 1,1 entry.
        /// </summary>
        public Double Xx;

        /// <summary>
        /// Gets or sets the 1,2 entry.
        /// </summary>
        public Double Xy;

        /// <summary>
        /// Gets or sets the 1,3 entry.
        /// </summary>
        public Double Xz;

        /// <summary>
        /// Gets or sets the 2,1 entry.
        /// </summary>
        public Double Yx;

        /// <summary>
        /// Gets or sets the 2,2 entry.
        /// </summary>
        public Double Yy;

        /// <summary>
        /// Gets or sets the 2,3 entry.
        /// </summary>
        public Double Yz;

        /// <summary>
        /// Gets or sets the 3,1 entry.
        /// </summary>
        public Double Zx;

        /// <summary>
        /// Gets or sets the 3,2 entry.
        /// </summary>
        public Double Zy;

        /// <summary>
        /// Gets or sets the 3,3 entry.
        /// </summary>
        public Double Zz;
    }
}
