namespace YAMP.Exceptions
{
    using System;

    /// <summary>
    /// The assignment exception.
    /// </summary>
	public class YAMPAssignmentException : YAMPRuntimeException
	{
        internal YAMPAssignmentException(String operation)
            : base("The left side of an assignment must be symbol.", operation)
		{
		}

        internal YAMPAssignmentException(String operation, String error)
            : base("Error in an assignment: {1}.", operation, error)
        {
        }
	}
}

