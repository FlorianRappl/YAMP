using System;

namespace YAMP
{
	public class TypeNotEqualException : YAMPException
	{
		public TypeNotEqualException (Value left, Value right) : base("The types ({0}, {1}) must be equal.", left.Header, right.Header)
		{
		}
	}
}

