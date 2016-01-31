namespace YAMP.Exceptions
{
    using System;

    /// <summary>
    /// The pixel format not supported exception.
    /// </summary>
    public class YAMPPixelFormatNotSupportedException : YAMPRuntimeException
    {
        internal YAMPPixelFormatNotSupportedException(String fileName)
            : base("The pixel format of the file {0} is not supported.", fileName)
        {
        }
    }
}
