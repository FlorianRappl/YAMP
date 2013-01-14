using System;

namespace YAMP
{
	public class YAMPNotConvergedException : YAMPRuntimeException
	{
        public YAMPNotConvergedException(string function)
            : base("Unfortunately no convergence could be reached. The function {0} has been stopped.", function)
		{
		}
	}
}
