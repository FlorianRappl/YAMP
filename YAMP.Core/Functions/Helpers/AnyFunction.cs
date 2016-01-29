using System;

namespace YAMP
{
    [Description("Determine whether any matrix element is nonzero.")]
    [Kind(PopularKinds.Logic)]
    sealed class AnyFunction : ArgumentFunction
    {
        [Description("Tests whether any of the elements along various dimensions of an array is nonzero.")]
        [Example("any([0, 0; 0, 0])", "Tests the 2x2 matrix with only zeros. Returns false.")]
        [Example("any(eye(3))", "Tests the 3x3 identity matrix. Returns true.")]
        public ScalarValue Function(MatrixValue M)
        {
            return new ScalarValue(M.HasElements);
        }
    }
}
