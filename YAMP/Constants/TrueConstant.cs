using System;

namespace YAMP
{
    /// <summary>
    /// Gets the value for true.
    /// </summary>
    [Description("True can be used in all logical expressions or for calculations. True is numerically represented by 1, however, that does not necessarily mean that everything else is false.")]
    [Kind(PopularKinds.Constant)]
    class TrueConstant : BaseConstant
    {
        static readonly ScalarValue _true = new ScalarValue(true);

        public override Value Value
        {
            get { return _true; }
        }
    }
}
