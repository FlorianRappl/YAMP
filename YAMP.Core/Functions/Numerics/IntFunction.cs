namespace YAMP
{
    [Description("IntFunctionDescription")]
    [Kind(PopularKinds.Function)]
    [Link("IntFunctionLink")]
    sealed class IntFunction : ArgumentFunction
	{
		[Description("IntFunctionDescriptionForMatrix")]
        [Example("int([1,2,3,2;2,1,0,-1])", "IntFunctionExampleForMatrix1")]
        public MatrixValue Function(MatrixValue func)
        {
            var adm = new MatrixValue(func.DimensionY, func.DimensionX + 1);

            for (var i = 1; i <= func.DimensionY; i++)
            {
                adm[i, 1] = new ScalarValue(0);

                for (var t = 1; t <= func.DimensionX; t++)
                {
                    adm[i, t + 1] = new ScalarValue(adm[i, t].Re + func[i, t].Re);
                }
            }

            return adm;
        }
    }
}
