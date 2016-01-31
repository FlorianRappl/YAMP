namespace YAMP.Exceptions
{
    using System;

    /// <summary>
    /// The different lengths exception.
    /// </summary>
    public class YAMPDifferentLengthsException : YAMPRuntimeException
    {
        public YAMPDifferentLengthsException(Int32 lengthA, Int32 lengthB)
            : base("Cannot compute the value for two matrices with different lengths, {0} and {1}.", lengthA, lengthB)
        {
        }

        public YAMPDifferentLengthsException(Int32 lengthA, String lengthB)
            : base("Cannot compute the value for the matrix length {0}. The length must be {1}.", lengthA, lengthB)
        {
        }
    }
}
