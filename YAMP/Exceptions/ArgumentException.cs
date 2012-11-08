using System;
namespace YAMP
{
	public class ArgumentException : YAMPException
	{
        public ArgumentException(string function)
            : base("The argument provided for {0} is not valid.", function)
		{
            Symbol = function;
		}

        public ArgumentException(string function, int argument)
            : base("The argument #{0} provided for {1} is not valid.", argument, function)
        {
            Symbol = function;
            AtArgument = argument;
        }

        public ArgumentException(string function, int argument, string expected, string received)
            : base("The argument #{0} provided for {1} is not valid. Excepted argument of type {2}, but received {3}.", argument, function, expected, received)
        {
            Symbol = function;
            AtArgument = argument;
        }
	}
}

