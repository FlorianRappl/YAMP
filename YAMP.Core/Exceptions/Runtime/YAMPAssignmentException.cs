using System;

namespace YAMP
{
	class YAMPAssignmentException : YAMPRuntimeException
	{
        public YAMPAssignmentException(string operation)
            : base("The left side of an assignment must be symbol.", operation)
		{
		}

        public YAMPAssignmentException(string operation, string error)
            : base("Error in an assignment: {1}.", operation, error)
        {
        }
	}
}

