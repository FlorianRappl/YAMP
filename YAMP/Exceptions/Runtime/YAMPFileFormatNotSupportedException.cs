using System;

namespace YAMP
{
    class YAMPFileFormatNotSupportedException : YAMPRuntimeException
    {
        public YAMPFileFormatNotSupportedException(string fileName)
            : base("The format of the file {0} is not supported.", fileName)
        {
        }
    }
}
