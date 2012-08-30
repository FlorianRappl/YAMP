using System;
namespace YAMP
{
	public class FunctionNotFoundException : Exception
	{
		public FunctionNotFoundException () : base("The requested function could not been found.")
		{
		}
		
		public FunctionNotFoundException (string name) : base("The function " + name + " could not been found.")
		{
		}
	}
}

