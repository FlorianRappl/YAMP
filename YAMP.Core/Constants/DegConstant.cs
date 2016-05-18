namespace YAMP
{
    using System;

    /// <summary>
    /// Gets the value of one degree.
    /// </summary>
    [Description("DegConstantDescription")]
    [Kind(PopularKinds.Constant)]
    [Link("DegConstantLink")]
    sealed class DegConstant : BaseConstant
    {
        static readonly ScalarValue deg = new ScalarValue(Math.PI / 180.0);

        public override Value Value
        {
            get { return deg; }
        }
    }
}
