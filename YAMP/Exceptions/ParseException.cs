using System;
namespace YAMP
{
	public class ParseException : YAMPException
	{
		public ParseException () : base ("Error while parsing the expression tree.")
		{
		}

        public ParseException(int index, string near)
            : base("Error while parsing the expression tree at index {0} (near {1}).", index, near)
		{
            AtIndex = index;
            Near = near;
		}
		
		public ParseException (string near) : base ("Error while parsing the expression tree near {0}.", near)
		{
            Near = near;
		}
	}
}

