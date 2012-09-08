using System;

namespace YAMP
{
    class EyeFunction : ArgumentFunction
    {
        public Value Function()
        {
            return MatrixValue.One(1);
        }

        public Value Function(ScalarValue dim)
        {
            var k = (int)dim.Value;
            return MatrixValue.One(k);
        }
    }
}
