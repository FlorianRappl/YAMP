using System;
namespace YAMP
{
	public class SymbolException : Exception
	{
		public SymbolException () : base("The symbol could not been resolved.")
		{
		}
		
		public SymbolException (string symbol) : base("The symbol " + symbol + " could not been resolved.")
		{
		}
	}
}

