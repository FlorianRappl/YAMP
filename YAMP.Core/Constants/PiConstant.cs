namespace YAMP
{
    using System;

	/// <summary>
	/// Gets the value of Pi.
	/// </summary>
	[Description("PiConstantDescription")]
    [Kind(PopularKinds.Constant)]
    [Link("PiConstantLink")]
	class PiConstant : BaseConstant
	{
        static readonly ScalarValue pi = new ScalarValue(Math.PI);

		public override Value Value
		{
			get { return pi; }
		}
	}
}
