using System;
namespace YAMP
{
	public class ParseException : Exception
	{
		public ParseException () : base ("Error while parsing the expression tree.")
		{
		}
		
		public ParseException (int index, string near) : base ("Error while parsing the expression tree at index " + index + " (near " + near + ").")
		{
		}
		
		public ParseException (string near) : base ("Error while parsing the expression tree near " + near + ".")
		{
		}
	}
}

