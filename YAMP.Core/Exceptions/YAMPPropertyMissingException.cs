namespace YAMP.Exceptions
{
    using System;

    /// <summary>
    /// The property missing exception.
    /// </summary>
	public class YAMPPropertyMissingException : YAMPRuntimeException
	{
        internal YAMPPropertyMissingException(String givenProperty, String[] availableProperties)
			: base("The given property {0} does not exist. The available properties are {1}.", givenProperty, String.Join(", ", availableProperties))
		{
		}
	}
}
