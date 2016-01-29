using System;

namespace YAMP
{
	[Description("Returns a logarithmically increased vector.")]
	[Kind(PopularKinds.Function)]
    sealed class LogspaceFunction : ArgumentFunction
	{
		[Description("Creates a vector with count elements ranging from a certain value to a certain value for the basis 10.")]
		[Example("logspace(2, 3, 5)", "Creates the vector [100, 177, 316, 562, 1000], i.e. start at 10^2 and end at 10^3 with number of elements 5.")]
		public MatrixValue Function(ScalarValue start, ScalarValue end, ScalarValue count)
		{
            return Function(start, end, count, new ScalarValue(10));
		}

		[Description("Creates a vector with count elements ranging from a certain value to a certain value for an arbitrary basis.")]
		[Example("logspace(2, 6, 5, 2)", "Creates the vector [4, 8, 16, 32, 64], i.e. start at 2^2 and end at 2^6 with number of elements 5.")]
		public MatrixValue Function(ScalarValue start, ScalarValue end, ScalarValue count, ScalarValue basis)
        {
            var c = count.GetIntegerOrThrowException("count", Name);
			
			if(c < 2)
				throw new ArgumentException("logspace");
			
			var s = (end.Re - start.Re) / (c - 1);
			var r = new RangeValue(start.Re, end.Re, s);	
			return MatrixValue.PowSM(basis, r);
		}
	}
}

