using System;

namespace YAMP
{
	public class YAMPArgumentValueException : YAMPRuntimeException
	{
        public YAMPArgumentValueException(string given, string[] possibilities)
			: base("The value {0} is not in the list of possible values. Possible values are {1}.", 
					given, string.Join(", ", possibilities))
		{
		}
	}
}
