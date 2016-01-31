namespace YAMP.Exceptions
{
    using System;

    /// <summary>
    /// The file format not supported exception.
    /// </summary>
    public class YAMPFileFormatNotSupportedException : YAMPRuntimeException
    {
        public YAMPFileFormatNotSupportedException(String fileName)
            : base("The format of the file {0} is not supported.", fileName)
        {
        }
    }
}
