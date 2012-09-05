using System;

namespace YAMP
{
	public class ArgumentsException : Exception
	{
		public ArgumentsException (string function, int number) : base("No overload for " + function + " takes " + number + " arguments.")
		{
		}
	}
}

