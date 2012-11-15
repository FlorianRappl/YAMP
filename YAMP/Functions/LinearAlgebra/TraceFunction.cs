using System;

namespace YAMP
{
	[Description("Performs the trace operation on the given matrix.")]
	[Kind(PopularKinds.Function)]
	class TraceFunction : ArgumentFunction
	{
		[Description("Sums all elements on the diagonal.")]
		[Example("trace([1,2;3,4])", "Results in the value 5.")]
		public ScalarValue Function(MatrixValue argument)
		{
			return argument.Trace();
		}

		[Description("The trace of a 1x1 matrix is the element itself.")]
		[Example("trace(10)", "Results in the value 10.")]
		public ScalarValue Function(ScalarValue argument)
		{
			return argument;
		}
	}
}
