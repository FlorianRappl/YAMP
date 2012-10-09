using System;

namespace YAMP
{
	public class BracketException : YAMPException
	{
		public BracketException () : base("Error parsing the end of the bracket.")
		{
		}

        public BracketException(int start)
            : base("Error parsing the end of the bracket starting at index {0}.", start)
		{
            AtIndex = start;
		}

        public BracketException(string bracket, string near)
            : base("Error parsing the end of the bracket {0} near {1}.", bracket, near)
		{
            Symbol = bracket;
            Near = near;
		}
		
		public BracketException(int start, string bracket, string near) :
            base("Error parsing the end of the bracket {0} near {1} starting at index {2}.", bracket, near, start)
		{
            AtIndex = start;
            Symbol = bracket;
            Near = near;
		}
	}
}

