using System;
namespace YAMP
{
	public class ArgumentException : YAMPException
	{
		public ArgumentException (string function) : base("The argument provided for " + function + " is not valid.")
		{
            Symbol = function;
		}
	}
}

