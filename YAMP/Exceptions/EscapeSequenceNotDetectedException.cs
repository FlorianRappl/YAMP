using System;

namespace YAMP
{
	public class EscapeSequenceNotDetectedException : YAMPException
	{
		public EscapeSequenceNotDetectedException(char sequence) : base("The escape sequence \\{0} is not recognized.", sequence)
		{
		}
	}
}
