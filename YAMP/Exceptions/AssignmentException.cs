using System;

namespace YAMP
{
	public class AssignmentException : Exception
	{
		public AssignmentException (string exception) : base("Error in assignment: " + exception + ".")
		{
		}
	}
}

