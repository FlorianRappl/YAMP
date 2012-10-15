using System;
using System.Collections.Generic;

namespace YAMP
{
	public class RangeValue : MatrixValue
	{
		public double Step { get; private set; }

		public double Start { get; private set; }

		public double End { get; private set; }

		public bool All { get; private set; }

		public RangeValue (double start, double end, double step)
		{
			Start = start;
			End = end;
			Step = step;
			var count = Math.Floor((end - start) / step);

			if(count < 0)
				throw new RangeException("Negative number of entries detected"); 
			else if(count >= int.MaxValue/ 100)
				throw new RangeException("Too many entries in the range");
			
			for(var j = 0; j <= count; j++)
				this[j + 1, 1] = new ScalarValue(start + j * step);
		}
		
		public RangeValue (double start, double step) : this(start, start, step)
		{
			All = true;
		}

		public RangeValue () : this(1, 1)
		{
		}
	}
}

