using System;

namespace YAMP
{
	public class ArgumentsException : YAMPException
	{
		public ArgumentsException (string function, int number) : base("No overload for {0} takes {1} arguments.", function, number)
		{
            Symbol = function;
            AtArgument = number;
		}
	}
}

