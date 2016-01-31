namespace YAMP.Exceptions
{
    using System;

    /// <summary>
    /// Class to use for numeric non-convergence exceptions.
    /// </summary>
	public class YAMPNotConvergedException : YAMPRuntimeException
	{
        /// <summary>
        /// Creates a new not converged exception.
        /// </summary>
        /// <param name="function">The function where the problem occured.</param>
        public YAMPNotConvergedException(String function)
            : base("Unfortunately no convergence could be reached. The function {0} has been stopped.", function)
		{
		}
	}
}
