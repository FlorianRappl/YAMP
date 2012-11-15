using System;
namespace YAMP
{
	public class SymbolException : YAMPException
	{
		public SymbolException () : base("The symbol could not been resolved.")
		{
		}

		public SymbolException(string symbol)
			: base("The symbol {0} could not been resolved.", symbol)
		{
			Symbol = symbol;
		}
	}
}

