namespace YAMP
{
	/// <summary>
	/// Gets the value of the golden ratio.
	/// </summary>
	[Description("PhiConstantDescription")]
    [Kind(PopularKinds.Constant)]
    [Link("PhiConstantLink")]
	class PhiConstant : BaseConstant
	{
        static readonly ScalarValue phi = new ScalarValue(1.61803398874989484820458683436563811);

		public override Value Value
		{
			get { return phi; }
		}
	}
}
