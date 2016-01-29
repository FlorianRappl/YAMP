using System;

namespace YAMP.Numerics
{
    /// <summary>
    /// Possible normalization values for the fourier transformation.
    /// </summary>
    public enum FourierNormalization
    {
        /// <summary>
        /// The series is not normalized.
        /// </summary>
        None,
        /// <summary>
        /// The series is multiplied by 1/N<sup>1/2</sup>.
        /// </summary>
        Unitary,
        /// <summary>
        /// The series is multiplied by 1/N.
        /// </summary>
        Inverse
    }
}
