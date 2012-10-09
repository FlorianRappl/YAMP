using System;

namespace YAMP
{
	public class AssignmentException : YAMPException
	{
        public AssignmentException(string op)
            : base("The left side in the assignment ({0}) must be symbol.", op)
		{
            Symbol = op;
		}

        public AssignmentException(string op, string exception)
            : base("Error in the assignment ({0}): {1}.", op, exception)
        {
            Symbol = op;
        }
	}
}

