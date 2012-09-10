using System;

namespace YAMP
{
	class LinspaceFunction : ArgumentFunction
	{
		public Value Function (ScalarValue _from, ScalarValue _to, ScalarValue _count)
		{
			var c = _count.IntValue;

			if(c < 2)
				throw new ArgumentException("linspace");

			var step = (_to.Value - _from.Value) / (c - 1);
			return new RangeValue(_from.Value, _to.Value, step);
		}
	}
}

