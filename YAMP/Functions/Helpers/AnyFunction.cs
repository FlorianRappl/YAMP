using System;

namespace YAMP
{
    class AnyFunction : ArgumentFunction
    {
        [Description("")]
        [Example("any([0, 0; 0, 0])", "Tests the 2x2 matrix with only zeros. Returns false.")]
        [Example("any(eye(3))", "Tests the 3x3 identity matrix. Returns true.")]
        public ScalarValue Function(MatrixValue M)
        {
            return new ScalarValue(M.HasElements);
        }
    }
}
