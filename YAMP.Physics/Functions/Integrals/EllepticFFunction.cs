namespace YAMP.Physics
{
    using System;
    using YAMP.Exceptions;
    using YAMP.Numerics;

    [Description("In integral calculus, elliptic integrals originally arose in connection with the problem of giving the arc length of an ellipse. They were first studied by Giulio Fagnano and Leonhard Euler. The incomplete elliptic integral of the first kind F is evaluated by this function.")]
    [Kind(PopularKinds.Function)]
    class EllipticFFunction : ArgumentFunction
    {
        [Description("Computes the incomplete elliptic integral of the first kind at the arguments phi and k.")]
        [Example("ellipticf(pi / 2, 1)", "Evaluates the incomplete elliptic integral at phi = pi / 2 and k = 1.")]
        public ScalarValue Function(ScalarValue phi, ScalarValue k)
        {
            return new ScalarValue(EllipticF(phi.Re, k.Re));
        }

        #region Algorithm

        public static Double EllipticF(Double phi, Double k)
        {
            if (Math.Abs(phi) > Helpers.HalfPI) 
                throw new YAMPArgumentRangeException("phi", -Helpers.HalfPI, Helpers.HalfPI);

            if ((k < 0) || (k > 1.0))
                throw new YAMPArgumentRangeException("k", 0, 1);

            var s = Math.Sin(phi);
            var c = Math.Cos(phi);
            var z = s * k;
            return s * CarlsonFFunction.CarlsonF(c * c, 1.0 - z * z, 1.0);
        }

        #endregion
    }
}
