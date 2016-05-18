namespace YAMP
{
    using YAMP.Numerics;

	/// <summary>
	/// Gets the first values of the bernoulli series.
	/// </summary>
	[Description("BernoulliConstantDescription")]
    [Kind(PopularKinds.Constant)]
    [Link("BernoulliConstantLink")]
	sealed class BernoulliConstant : BaseConstant
	{
		public override Value Value
		{
            get { return new MatrixValue(Helpers.BernoulliNumbers); }
		}
	}
}
