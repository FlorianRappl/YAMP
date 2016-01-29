using System;

namespace YAMP
{
    abstract class LeftUnaryOperator : UnaryOperator
    {
        public LeftUnaryOperator(string op, int level) : base(op, level)
        {
            IsRightToLeft = false;
        }
    }
}
