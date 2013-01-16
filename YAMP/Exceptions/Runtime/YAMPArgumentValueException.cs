using System;

namespace YAMP
{
    /// <summary>
    /// Class to use when the value of an argument was not expected (i.e. some specific string or numeric value).
    /// </summary>
	public class YAMPArgumentValueException : YAMPRuntimeException
	{
        public YAMPArgumentValueException(string given, string[] possibilities)
			: base("The value {0} is not in the list of possible values. Possible values are {1}.", 
					given, string.Join(", ", possibilities))
		{
		}
	}
}
