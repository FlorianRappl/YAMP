﻿using System;

namespace YAMP
{
    /// <summary>
    /// Returns the given scalar(s) with a switched sign. This operator is a unary operator,
    /// but is used as a binary one with a pseudo expression on the left side.
    /// </summary>
    class NegateOperator : LeftUnaryOperator
    {
        public NegateOperator()
            : base("-", 7)
        {
        }

        public override Value Perform(Value value)
        {
            if (value is ScalarValue)
            {
                var scalar = (ScalarValue)value;
                return -scalar;
            }
            else if (value is MatrixValue)
            {
                var matrix = (MatrixValue)value;
                return -matrix;
            }

            throw new YAMPOperationInvalidException(Op, value);
        }

        public override Operator Create()
        {
            return new NegateOperator();
        }
    }
}