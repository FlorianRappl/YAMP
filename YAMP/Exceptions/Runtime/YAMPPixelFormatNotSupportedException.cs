using System;

namespace YAMP
{
    class YAMPPixelFormatNotSupportedException : YAMPRuntimeException
    {
        public YAMPPixelFormatNotSupportedException(string fileName)
            : base("The pixel format of the file {0} is not supported.", fileName)
        {
        }
    }
}
