using System;

namespace YAMP
{
    /// <summary>
    /// Gets the value of one degree.
    /// </summary>
    [Description("A degree (in full, a degree of arc, arc degree, or arcdegree), usually denoted by ° (the degree symbol), is a measurement of plane angle, representing 1⁄360 of a full rotation; one degree is equivalent to π/180 radians.")]
    [Kind(PopularKinds.Constant)]
    [Link("http://en.wikipedia.org/wiki/Degree_(angle)")]
    class DegConstant : BaseConstant
    {
        static readonly ScalarValue deg = new ScalarValue(Math.PI / 180.0);

        public override Value Value
        {
            get { return deg; }
        }
    }
}
