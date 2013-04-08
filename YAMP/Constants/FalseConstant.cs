using System;

namespace YAMP
{
    /// <summary>
    /// Gets the value for false.
    /// </summary>
    [Description("False can be used in all logical expressions or for calculations. False is numerically represented by 0, however, that does not necessarily mean that everything else is true.")]
    [Kind(PopularKinds.Constant)]
    class FalseConstant : BaseConstant
    {
        public override Value Value
        {
            get { return ScalarValue.False; }
        }
    }
}
