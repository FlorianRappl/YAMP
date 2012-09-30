using System;

namespace YAMP
{
    [Description("Returns a logarithmically increased vector.")]
	class LogspaceFunction : ArgumentFunction
    {
        [Description("Creates a vector with count elements ranging from a certain value to a certain value for the basis 10.")]
        [Example("logspace(2, 3, 5)", "Creates the vector (100, 177, 316, 562, 1000), i.e. start at 10^2 and end at 10^3 with number of elements 5.")]
        public MatrixValue Function(ScalarValue start, ScalarValue end, ScalarValue count)
		{
			return Function(start, end, count, new ScalarValue(10.0));
		}

        [Description("Creates a vector with count elements ranging from a certain value to a certain value for an arbitrary basis.")]
        [Example("logspace(2, 6, 5, 2)", "Creates the vector (4, 8, 16, 32, 64), i.e. start at 2^2 and end at 2^6 with number of elements 5.")]
        public MatrixValue Function(ScalarValue start, ScalarValue end, ScalarValue count, ScalarValue basis)
		{
			var c = count.IntValue;
			
			if(c < 2)
				throw new ArgumentException("logspace");
			
			var s = (end.Value - start.Value) / (c - 1);
			var r = new RangeValue(start.Value, end.Value, s);	
			return basis.Power(r) as MatrixValue;
		}
	}
}

