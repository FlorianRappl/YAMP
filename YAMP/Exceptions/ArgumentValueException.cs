using System;

namespace YAMP
{
	public class ArgumentValueException : YAMPException
	{
		public ArgumentValueException(string given, string[] possibilities)
			: base("The value {0} is not in the list of possible values. The possible values are [ {1} ].", 
					given, string.Join(", ", possibilities))
		{

		}
	}
}
