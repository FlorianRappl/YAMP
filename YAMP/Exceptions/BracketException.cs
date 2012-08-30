using System;
namespace YAMP
{
	public class BracketException : Exception
	{
		public BracketException () : base("Error parsing the end of the bracket.")
		{
		}
		
		public BracketException(int start) : base("Error parsing the end of the bracket starting at index " + start + ".")
		{
		}
		
		public BracketException(string bracket, string near) : base("Error parsing the end of the bracket " + bracket + " near " + near + ".")
		{
		}
	}
}

