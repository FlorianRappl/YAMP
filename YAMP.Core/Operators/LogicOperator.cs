namespace YAMP
{
    using YAMP.Exceptions;

    /// <summary>
    /// The abstract base class for any logic operator (==, ~=, >, >=, ...),
    /// which is essentially a binary operator.
    /// </summary>
    public abstract class LogicOperator : BinaryOperator
    {
        #region ctor

        /// <summary>
        /// Creates a new logic operator (like ==, ~=, ...).
        /// </summary>
        /// <param name="op">The operator string.</param>
        public LogicOperator (string op) : base(op, 4)
		{
		}

        /// <summary>
        /// Creates a new logic operator.
        /// </summary>
        /// <param name="op">The operator string.</param>
        /// <param name="level">The operator level.</param>
        public LogicOperator(string op, int level)
            : base(op, level)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Method to implement, which compares two scalars.
        /// </summary>
        /// <param name="left">The left one.</param>
        /// <param name="right">The right one.</param>
        /// <returns>The result of the comparison.</returns>
        public abstract ScalarValue Compare(ScalarValue left, ScalarValue right);
		
        /// <summary>
        /// Performs the logic operation with two values.
        /// </summary>
        /// <param name="left">The left value.</param>
        /// <param name="right">The right value.</param>
        /// <returns>The result of the operation.</returns>
		public override Value Perform (Value left, Value right)
		{
			if (!(left is ScalarValue || left is MatrixValue))
				throw new YAMPOperationInvalidException(Op, left);

			if (!(right is ScalarValue || right is MatrixValue))
				throw new YAMPOperationInvalidException(Op, right);
			
			if (left is ScalarValue && right is ScalarValue)
			{
				return Compare (left as ScalarValue, right as ScalarValue);
			}
			else if (left is MatrixValue && right is ScalarValue)
			{
				var l = left as MatrixValue;
				var r = right as ScalarValue;
				var m = new MatrixValue(l.DimensionY, l.DimensionX);

                for (var i = 1; i <= m.DimensionX; i++)
                {
                    for (var j = 1; j <= m.DimensionY; j++)
                    {
                        m[j, i] = Compare(l[j, i], r);
                    }
                }
				
				return m;
			}
			else if (left is ScalarValue && right is MatrixValue)
			{
				var l = left as ScalarValue;
				var r = right as MatrixValue;
				var m = new MatrixValue(r.DimensionY, r.DimensionX);

                for (var i = 1; i <= m.DimensionX; i++)
                {
                    for (var j = 1; j <= m.DimensionY; j++)
                    {
                        m[j, i] = Compare(l, r[j, i]);
                    }
                }
				
				return m;
			}
			else
			{
				var l = left as MatrixValue;
				var r = right as MatrixValue;

				if (l.DimensionX != r.DimensionX || l.DimensionY != r.DimensionY)
					throw new YAMPDifferentDimensionsException(l, r);

				var m = new MatrixValue(l.DimensionY, l.DimensionX);

                for (var i = 1; i <= m.DimensionX; i++)
                {
                    for (var j = 1; j <= m.DimensionY; j++)
                    {
                        m[j, i] = Compare(l[j, i], r[j, i]);
                    }
                }
				
				return m;
			}
        }

        #endregion
    }
}

