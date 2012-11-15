using System;

namespace YAMP
{
	public class WrongArgumentsException : YAMPException
	{
		public WrongArgumentsException(string argumentName, string type) : base("Cannot start YAMP with the argument {0}. Expected type to be a Value, but received {1} as type.", argumentName, type)
		{
		}
	}
}
