using System;

namespace YAMP
{
    /// <summary>
    /// Gets the omega constant.
    /// </summary>
    [Description("The omega constant is the value of W(1) where W is Lambert's W function. The name is derived from the alternate name for Lambert's W function, the omega function.")]
    [Kind(PopularKinds.Constant)]
    [Link("http://en.wikipedia.org/wiki/Omega_constant")]
    class OmegaConstant : BaseConstant
    {
        static readonly ScalarValue omega = new ScalarValue(0.5671432904097838729999686622);

        public override Value Value
        {
            get { return omega; }
        }
    }
}
