using System;

namespace YAMP
{
    abstract class RightUnaryOperator : UnaryOperator
    {
        public RightUnaryOperator(string op, int level) : base(op, level)
        {
            IsRightToLeft = true;
        }
    }
}
