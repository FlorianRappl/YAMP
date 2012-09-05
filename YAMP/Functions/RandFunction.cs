using System;

namespace YAMP
{
	class RandFunction : ArgumentFunction
	{
		static Random ran = new Random();
		
		public Value Function(ScalarValue dim)
		{
			var k = (int)dim.Value;
			
			if(k <= 1)
				return new ScalarValue(ran.NextDouble());
			
			var m = new MatrixValue(k, k);
			
			for(var i = 1; i <= k; i++)
				for(var j = 1; j <= k; j++)
					m[j, i] = new ScalarValue(ran.NextDouble());
			
			return m;
		}
		
		public Value Function(ScalarValue rows, ScalarValue cols)
		{
			var k = (int)rows.Value;
			var l = (int)cols.Value;
			var m = new MatrixValue(k, l);
			
			for(var i = 1; i <= l; i++)
				for(var j = 1; j <= k; j++)
					m[j, i] = new ScalarValue(ran.NextDouble());
			
			return m;
		}
	}
}