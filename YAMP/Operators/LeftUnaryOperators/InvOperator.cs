using System;

namespace YAMP
{
    /// <summary>
    /// Inverts the given scalar. This operator is a unary operator, but is
    /// used as a binary one with a pseudo expression on the left side.
    /// </summary>
    class InvOperator : LeftUnaryOperator
    {
        public InvOperator()
            : base("~", 995)
		{
		}

        public override Value Perform(Value value)
        {
            if (value is ScalarValue)
            {
                var scalar = (ScalarValue)value;
                return new ScalarValue(scalar.IsFalse);
            }
            else if (value is MatrixValue)
            {
                var A = (MatrixValue)value;
                var M = new MatrixValue(A.DimensionY, A.DimensionX);

                for (var j = 1; j <= A.DimensionY; j++)
                    for (var i = 1; i <= A.DimensionX; i++)
                        M[j, i] = new ScalarValue(A[j, i].IsFalse);

                return M;
            }

            return new ScalarValue(false);
        }

        public override Operator Create()
        {
            return new InvOperator();
        }
    }
}
