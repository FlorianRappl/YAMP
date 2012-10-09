using System;

namespace YAMP
{
	public class ExpressionNotFoundException : YAMPException
	{
        public ExpressionNotFoundException(string input)
            : base("No valid expression has been found in the beginning of {0}.", input)
		{
            Near = input;
		}
	}
}

