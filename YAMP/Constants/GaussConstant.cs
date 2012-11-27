using System;

namespace YAMP
{
    /// <summary>
    /// Gets the value of gauss's constant.
    /// </summary>
    [Description("In mathematics, Gauss's constant, denoted by G, is defined as the reciprocal of the arithmetic-geometric mean of 1 and the square root of 2.")]
    [Kind(PopularKinds.Constant)]
    class GaussConstant : BaseConstant
    {
        static readonly ScalarValue gauss = new ScalarValue(0.8346268416740731862814297);

        public override Value Value
        {
            get { return gauss; }
        }
    }
}
