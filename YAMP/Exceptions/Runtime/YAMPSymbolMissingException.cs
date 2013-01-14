using System;
namespace YAMP
{
	public class YAMPSymbolMissingException : YAMPRuntimeException
	{
        public YAMPSymbolMissingException(string symbol)
			: base("The symbol {0} could not be found.", symbol)
		{
		}
	}
}

