namespace YAMP
{
    /// <summary>
    /// Gets the value of gauss's constant.
    /// </summary>
    [Description("GaussConstantDescription")]
    [Kind(PopularKinds.Constant)]
    sealed class GaussConstant : BaseConstant
    {
        static readonly ScalarValue gauss = new ScalarValue(0.8346268416740731862814297);

        public override Value Value
        {
            get { return gauss; }
        }
    }
}
