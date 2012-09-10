using System;

namespace YAMP
{
	public class RangeException : Exception
	{
		public RangeException (string error) : base("Error in range expression: " + error + ".")
		{
		}
	}
}

