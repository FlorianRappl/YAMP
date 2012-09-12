using System;

namespace YAMP
{
    class OnesFunction : ArgumentFunction
    {
        public Value Function()
        {
            return MatrixValue.Ones(1, 1);
        }

        public Value Function(ScalarValue dim)
        {
            var k = (int)dim.Value;
            return MatrixValue.Ones(k, k);
        }

        public Value Function(ScalarValue rows, ScalarValue cols)
        {
            var k = (int)rows.Value;
            var l = (int)cols.Value;
            return MatrixValue.Ones(k, l);
        }
    }
}
