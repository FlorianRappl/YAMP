namespace YAMP
{
	/// <summary>
	/// Gets the value of delta.
	/// </summary>
	[Description("DeltaConstantDescription")]
    [Kind(PopularKinds.Constant)]
    [Link("DeltaConstantLink")]
	class DeltaConstant : BaseConstant
	{
        static readonly ScalarValue delta = new ScalarValue(4.66920160910299067185320382046620161);

		public override Value Value
		{
			get { return delta; }
		}
	}
}
