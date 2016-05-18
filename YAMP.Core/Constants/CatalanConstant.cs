namespace YAMP
{
	/// <summary>
	/// Gets Catalan's constant.
	/// </summary>
	[Description("CatalanConstantDescription")]
    [Kind(PopularKinds.Constant)]
    [Link("CatalanConstantLink")]
	sealed class CatalanConstant : BaseConstant
	{
        static readonly ScalarValue beta = new ScalarValue(0.915965594177219015054604);

		public override Value Value
		{
			get { return beta; }
		}
	}
}
