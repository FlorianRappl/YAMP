namespace YAMP
{
	/// <summary>
	/// Gets the value of gamma(1).
	/// </summary>
	[Description("Gamma1ConstantDescription")]
    [Kind(PopularKinds.Constant)]
    [Link("Gamma1ConstantLink")]
	sealed class Gamma1Constant : BaseConstant
	{
        static readonly ScalarValue gamma = new ScalarValue(0.57721566490153286060651209008240243);

		public override Value Value
		{
			get { return gamma; }
		}
	}
}
