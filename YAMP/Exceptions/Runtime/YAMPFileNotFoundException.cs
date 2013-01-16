using System;

namespace YAMP
{
    /// <summary>
    /// Class to use when a file could not be found.
    /// </summary>
    public class YAMPFileNotFoundException : YAMPRuntimeException
    {
        public YAMPFileNotFoundException(string fileName)
            : base("The file {0} was not found.", fileName)
        {
        }
    }
}
