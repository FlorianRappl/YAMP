using System;

namespace YAMP
{
	public class TypeNotEqualException : Exception
	{
		public TypeNotEqualException (Value left, Value right) : base("The types must be equal - received: " + left.GetType().Name + ", " + right.GetType().Name + ".")
		{
		}
	}
}

