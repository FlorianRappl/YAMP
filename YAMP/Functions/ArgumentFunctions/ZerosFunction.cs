using System;

namespace YAMP
{
    class ZerosFunction : ArgumentFunction
    {
        public Value Function()
        {
            return new MatrixValue(1,1);
        }

        public Value Function(ScalarValue dim)
        {
            var k = (int)dim.Value;
            return new MatrixValue(k, k);
        }

        public Value Function(ScalarValue rows, ScalarValue cols)
        {
            var k = (int)rows.Value;
            var l = (int)cols.Value;
            return new MatrixValue(k, l);
        }
    }
}
