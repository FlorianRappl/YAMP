using System;

namespace YAMP
{
	class YAMPPropertyMissingException : YAMPRuntimeException
	{
        public YAMPPropertyMissingException(string givenProperty, string[] availableProperties)
			: base("The given property {0} does not exist. The available properties are {1}.", givenProperty, string.Join(", ", availableProperties))
		{
		}
	}
}
