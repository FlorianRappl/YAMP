using System;

namespace YAMP
{
	public class FunctionNotFoundException : YAMPException
	{
		public FunctionNotFoundException () : base("The requested function could not been found.")
		{
		}

        public FunctionNotFoundException(string name)
            : base("The function {0} could not been found.", name)
		{
            Symbol = name;
		}
	}
}

