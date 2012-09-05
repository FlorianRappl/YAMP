using System;
namespace YAMP
{
	public class ArgumentException : Exception
	{
		public ArgumentException (string function) : base("The argument provided for " + function + " is not valid.")
		{
		}
	}
}

