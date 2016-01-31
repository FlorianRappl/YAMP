namespace YAMP
{
    using System;

    public class YAMPPixelFormatNotSupportedException : YAMPRuntimeException
    {
        public YAMPPixelFormatNotSupportedException(String fileName)
            : base("The pixel format of the file {0} is not supported.", fileName)
        {
        }
    }
}
