namespace YAMP
{
    using System;

    public class YAMPFileFormatNotSupportedException : YAMPRuntimeException
    {
        public YAMPFileFormatNotSupportedException(String fileName)
            : base("The format of the file {0} is not supported.", fileName)
        {
        }
    }
}
