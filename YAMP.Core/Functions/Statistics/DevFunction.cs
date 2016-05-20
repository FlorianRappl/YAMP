namespace YAMP
{
    using System;

    [Description("DevFunctionDescription")]
    [Kind(PopularKinds.Statistic)]
    [Link("DevFunctionLink")]
    sealed class DevFunction : ArgumentFunction
	{
		[Description("DevFunctionDescriptionForMatrix")]
		[Example("dev([2, 4, 4, 4, 5, 5, 7, 9])", "DevFunctionExampleForMatrix1")]
		public ScalarValue Function(MatrixValue M)
		{
			var deviation = ScalarValue.Zero;
			var mean = M.Sum() / M.Length;

            for (var i = 1; i <= M.Length; i++)
            {
                deviation += (M[i] - mean).Square();
            }

            return new ScalarValue(Math.Sqrt(deviation.Abs() / M.Length));
		}
	}
}