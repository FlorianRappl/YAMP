namespace YAMP
{
    /// <summary>
    /// Gets the value of alpha.
    /// </summary>
    [Description("AlphaConstantDescription")]
	[Kind(PopularKinds.Constant)]
    [Link("AlphaConstantLink")]
	sealed class AlphaConstant : BaseConstant
	{
        static readonly ScalarValue alpha = new ScalarValue(2.50290787509589282228390287321821578);

		public override Value Value
		{
			get { return alpha; }
		}
	}
}
