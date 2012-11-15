using System;

namespace YAMP
{
	public struct MultipleIndex : IIsIndex
	{
		public int[] Indices { get; set; }

		public int[] Get()
		{
			return Indices;
		}
	}
}
