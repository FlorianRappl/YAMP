using System;

namespace YAMP
{
    public struct MatrixIndex : IIsIndex
    {
        public int Row { get; set; }
		public int Column { get; set; }

		public int[] Get()
		{
			return new int[] { Row, Column };
		}
    }
}
