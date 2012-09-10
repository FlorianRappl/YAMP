using System;

namespace YAMP
{
	class RandiFunction : ArgumentFunction
	{
		static readonly Random ran = new Random();

		public Value Function (ScalarValue min, ScalarValue max)
		{
			return new ScalarValue(ran.Next(min.IntValue, max.IntValue + 1));
		}

		public Value Function(ScalarValue dim, ScalarValue min, ScalarValue max)
		{
			var k = (int)dim.Value;
			
			if(k <= 1)
				return new ScalarValue(ran.Next(min.IntValue, max.IntValue + 1));
			
			var m = new MatrixValue(k, k);
			
			for(var i = 1; i <= k; i++)
				for(var j = 1; j <= k; j++)
					m[j, i] = new ScalarValue(ran.Next(min.IntValue, max.IntValue + 1));
			
			return m;
		}
		
		public Value Function(ScalarValue rows, ScalarValue cols, ScalarValue min, ScalarValue max)
		{
			var k = (int)rows.Value;
			var l = (int)cols.Value;
			var m = new MatrixValue(k, l);
			
			for(var i = 1; i <= l; i++)
				for(var j = 1; j <= k; j++)
					m[j, i] = new ScalarValue(ran.Next(min.IntValue, max.IntValue + 1));
			
			return m;
		}
	}
}

