using System;

namespace YAMP
{
	class LogspaceFunction : ArgumentFunction
	{
		public Value Function (ScalarValue start, ScalarValue end, ScalarValue count)
		{
			return Function(start, end, count, new ScalarValue(10.0));
		}

		public Value Function(ScalarValue start, ScalarValue end, ScalarValue count, ScalarValue basis)
		{
			var c = count.IntValue;
			
			if(c < 2)
				throw new ArgumentException("logspace");
			
			var s = (end.Value - start.Value) / (c - 1);
			var r = new RangeValue(start.Value, end.Value, s);	
			return basis.Power(r);
		}
	}
}

