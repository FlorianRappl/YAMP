using System;
namespace YAMP
{
	public class ExpressionNotFoundException : Exception
	{
		public ExpressionNotFoundException (string input) : base("No valid expression has been found in the beginning of " + input + ".")
		{
		}
	}
}

