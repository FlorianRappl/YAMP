using System;

namespace YAMP
{
	public struct VectorIndex : IIsIndex
	{
		public int Entry { get; set; }

		public int[] Get()
		{
			return new int[] { Entry };
		}
	}
}
