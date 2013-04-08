using System;

namespace YAMP
{
    [Description("Determine whether all matrix elements are nonzero.")]
    [Kind(PopularKinds.Logic)]
    sealed class AllFunction : ArgumentFunction
    {
        [Description("Tests whether all the elements along various dimensions of an array are nonzero.")]
        [Example("all([0, 0; 0, 0])", "Tests the 2x2 matrix with only zeros. Returns false.")]
        [Example("all(eye(3))", "Tests the 3x3 identity matrix. Returns false.")]
        [Example("all([2,1;5,2])", "Tests the given 2x2 matrix. Returns true.")]
        public ScalarValue Function(MatrixValue M)
        {
            return new ScalarValue(M.HasNoZeros);
        }
    }
}
