namespace YAMP
{
    using System;

    /// <summary>
	/// Gets the value of euler's number.
	/// </summary>
	[Description("EConstantDescription")]
    [Kind(PopularKinds.Constant)]
    [Link("EConstantLink")]
	class EConstant : BaseConstant
	{
		static readonly ScalarValue e = new ScalarValue(Math.E);

		public override Value Value
		{
			get { return e; }
		}
	}
}
