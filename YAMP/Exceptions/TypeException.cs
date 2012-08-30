using System;
namespace YAMP
{
	public class TypeException : Exception
	{
		public TypeException () : base("Cannot perform this operation on two different types.")
		{
		}
	}
}

