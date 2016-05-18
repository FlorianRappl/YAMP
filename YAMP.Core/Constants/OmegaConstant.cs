namespace YAMP
{
    /// <summary>
    /// Gets the omega constant.
    /// </summary>
    [Description("OmegaConstantDescription")]
    [Kind(PopularKinds.Constant)]
    [Link("OmegaConstantLink")]
    sealed class OmegaConstant : BaseConstant
    {
        static readonly ScalarValue omega = new ScalarValue(0.5671432904097838729999686622);

        public override Value Value
        {
            get { return omega; }
        }
    }
}
