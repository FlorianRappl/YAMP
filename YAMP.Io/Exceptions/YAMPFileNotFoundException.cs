namespace YAMP.Exceptions
{
    using System;

    /// <summary>
    /// Class to use when a file could not be found.
    /// </summary>
    public class YAMPFileNotFoundException : YAMPRuntimeException
    {
        /// <summary>
        /// Creates a new file not found exception.
        /// </summary>
        /// <param name="fileName">The path to the file that has not been found.</param>
        public YAMPFileNotFoundException(String fileName)
            : base("The file {0} was not found.", fileName)
        {
        }
    }
}
