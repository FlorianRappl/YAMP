namespace YAMP
{
    [Description("DiffFunctionDescription")]
    [Kind(PopularKinds.Function)]
    [Link("DiffFunctionLink")]
    sealed class DiffFunction : ArgumentFunction
	{
		[Description("DiffFunctionDescriptionForMatrix")]
        [Example("diff([0,1,3,6,8;0,2,3,3,2])", "DiffFunctionExampleForMatrix1")]
        public MatrixValue Function(MatrixValue func)
        {
            var adm = new MatrixValue(func.DimensionY, func.DimensionX - 1);

            for (var i = 1; i <= func.DimensionY; i++)
            {
                for (var t = 1; t <= func.DimensionX - 1; t++)
                {
                    adm[i, t] = new ScalarValue(func[i, t + 1].Re - func[i, t].Re);
                }
            }

            return adm;
        }
    }
}
