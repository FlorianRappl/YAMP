using System;

namespace YAMP
{
    abstract class DotOperator : BinaryOperator
    {
        BinaryOperator _top;

        public DotOperator(BinaryOperator top) : base("." + top.Op, top.Level)
        {
            _top = top;
        }

        public abstract ScalarValue Operation(ScalarValue left, ScalarValue right);

        public override Value Perform(Value left, Value right)
        {
            if (left.GetType() != right.GetType())
                throw new OperationNotSupportedException(Op, right);

            if (left is MatrixValue)
            {
                var l = left as MatrixValue;
                var r = right as MatrixValue;

                if (l.DimensionX != r.DimensionX)
                    throw new DimensionException(l.DimensionX, r.DimensionX);

                if (l.DimensionY != r.DimensionY)
                    throw new DimensionException(l.DimensionY, r.DimensionY);

                var m = new MatrixValue(l.DimensionY, l.DimensionX);

                for (var i = 1; i <= l.DimensionX; i++)
                    for (var j = 1; j <= l.DimensionY; j++)
                        m[j, i] = Operation(l[j, i], r[j, i]);

                return m;
            }

            return _top.Perform(left, right);
        }
    }
}

