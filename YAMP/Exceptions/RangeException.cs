using System;

namespace YAMP
{
	public class RangeException : YAMPException
	{
        public RangeException(string error)
            : base("Error in range expression: {0}.", error)
		{
            Symbol = ":";
		}
	}
}

