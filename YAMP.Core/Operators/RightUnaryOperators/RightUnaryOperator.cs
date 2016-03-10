namespace YAMP
{
    using System;

    abstract class RightUnaryOperator : UnaryOperator
    {
        public RightUnaryOperator(String op, Int32 level) : 
            base(op, level)
        {
            IsRightToLeft = true;
        }
    }
}
