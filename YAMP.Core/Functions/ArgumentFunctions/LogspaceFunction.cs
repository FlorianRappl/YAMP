namespace YAMP
{
    using System;

    [Description("LogspaceFunctionDescription")]
	[Kind(PopularKinds.Function)]
    sealed class LogspaceFunction : ArgumentFunction
	{
		[Description("LogspaceFunctionDescriptionForScalarScalarScalar")]
		[Example("logspace(2, 3, 5)")]
		public MatrixValue Function(ScalarValue start, ScalarValue end, ScalarValue count)
		{
            return Function(start, end, count, new ScalarValue(10));
		}

		[Description("LogspaceFunctionDescriptionForScalarScalarScalarScalar")]
		[Example("logspace(2, 6, 5, 2)", "LogspaceFunctionExampleForScalarScalarScalar1")]
		public MatrixValue Function(ScalarValue start, ScalarValue end, ScalarValue count, ScalarValue basis)
        {
            var c = count.GetIntegerOrThrowException("count", Name);

            if (c < 2)
            {
                throw new ArgumentException("logspace");
            }
			
			var s = (end.Re - start.Re) / (c - 1);
			var r = new RangeValue(start.Re, end.Re, s);	
			return MatrixValue.PowSM(basis, r);
		}
	}
}

