using System;

namespace YAMP
{
    /// <summary>
    /// Class to use for numeric non-convergence exceptions.
    /// </summary>
	public class YAMPNotConvergedException : YAMPRuntimeException
	{
        public YAMPNotConvergedException(string function)
            : base("Unfortunately no convergence could be reached. The function {0} has been stopped.", function)
		{
		}
	}
}
