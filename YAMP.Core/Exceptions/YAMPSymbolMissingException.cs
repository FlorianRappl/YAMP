namespace YAMP.Exceptions
{
    using System;

    /// <summary>
    /// The symbol missing exception.
    /// </summary>
	public class YAMPSymbolMissingException : YAMPRuntimeException
	{
        public YAMPSymbolMissingException(String symbol)
			: base("The symbol {0} could not be found.", symbol)
		{
		}
	}
}

