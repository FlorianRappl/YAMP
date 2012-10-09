using System;

namespace YAMP
{
	public class DimensionException : YAMPException
	{
		public DimensionException () : base("Cannot perform this operation on two objects with different dimensions.")
		{
		}

        public DimensionException(int dimA, int dimB)
            : base("The operation cannot be performed on two objects with dimensions {0} and {1}.", dimA, dimB)
		{
		}
	}
}

