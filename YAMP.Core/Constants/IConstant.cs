namespace YAMP
{
	/// <summary>
	/// Gets the value of the imaginary constant.
	/// </summary>
	[Description("IConstantDescription")]
    [Kind(PopularKinds.Constant)]
    [Link("IConstantLink")]
	sealed class IConstant : BaseConstant
	{
		static readonly ScalarValue i = ScalarValue.I;

		public override Value Value
		{
			get { return i; }
		}
	}
}
