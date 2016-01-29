using System;

namespace YAMP
{
	class YAMPSymbolMissingException : YAMPRuntimeException
	{
        public YAMPSymbolMissingException(string symbol)
			: base("The symbol {0} could not be found.", symbol)
		{
		}
	}
}

