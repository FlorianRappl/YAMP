using System;

namespace YAMP
{
    /// <summary>
    /// The abstract base class for any dot operator (.*, ./, ...), which is essentially a special
    /// binary operator.
    /// </summary>
	public abstract class DotOperator : BinaryOperator
    {
        #region Fields

        BinaryOperator _top;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new dot operator (e.g. .*, ./, ...).
        /// </summary>
        /// <param name="top">The binary operator on which this is based.</param>
        public DotOperator(BinaryOperator top) : base("." + top.Op, top.Level)
		{
			_top = top;
		}

        #endregion

        #region Methods

        /// <summary>
        /// Implementation of the operation.
        /// </summary>
        /// <param name="left">The left scalar.</param>
        /// <param name="right">The right scalar.</param>
        /// <returns>The result of the operation.</returns>
        public abstract ScalarValue Operation(ScalarValue left, ScalarValue right);

        /// <summary>
        /// Performs the evaluation of the dot operator.
        /// </summary>
        /// <param name="left">The left value.</param>
        /// <param name="right">The right value.</param>
        /// <returns>The result of the operation.</returns>
		public override Value Perform(Value left, Value right)
		{
			if (!(left is NumericValue))
				throw new YAMPOperationInvalidException(Op, left);
			else if (!(right is NumericValue))
				throw new YAMPOperationInvalidException(Op, right);

			if (left is MatrixValue && right is MatrixValue)
			{
				var l = (MatrixValue)left;
				var r = (MatrixValue)right;
				return Dot(l, r);
			}
			else if (left is MatrixValue && right is ScalarValue)
			{
				var l = (MatrixValue)left;
				var r = (ScalarValue)right;
				return Dot(l, r);
			}
			else if (left is ScalarValue && right is MatrixValue)
			{
				var l = (ScalarValue)left;
				var r = (MatrixValue)right;
				return Dot(l, r);
			}

			return _top.Perform(left, right);
		}

        #endregion

        #region Useful Helpers

        /// <summary>
        /// Performs an operation on each entry of the left and right matrix.
        /// </summary>
        /// <param name="left">The left matrix.</param>
        /// <param name="right">The right matrix.</param>
        /// <returns>The cross product of all possibilities.</returns>
        protected MatrixValue Dot(MatrixValue left, MatrixValue right)
		{
            if (left.DimensionX != right.DimensionX || left.DimensionY != right.DimensionY)
                throw new YAMPDifferentDimensionsException(left, right);

			var m = new MatrixValue(left.DimensionY, left.DimensionX);

			for (var i = 1; i <= left.DimensionX; i++)
				for (var j = 1; j <= left.DimensionY; j++)
					m[j, i] = Operation(left[j, i], right[j, i]);

			return m;
		}

        /// <summary>
        /// Performs an operation on each entry of the left matrix.
        /// </summary>
        /// <param name="left">The left matrix.</param>
        /// <param name="right">The right scalar.</param>
        /// <returns>The cross product of all possibilities.</returns>
        protected MatrixValue Dot(MatrixValue left, ScalarValue right)
		{
			var m = new MatrixValue(left.DimensionY, left.DimensionX);

			for (var i = 1; i <= left.DimensionX; i++)
				for (var j = 1; j <= left.DimensionY; j++)
					m[j, i] = Operation(left[j, i], right);

			return m;
		}

        /// <summary>
        /// Performs an operation on each entry of the right matrix.
        /// </summary>
        /// <param name="left">The left scalar.</param>
        /// <param name="right">The right matrix.</param>
        /// <returns>The cross product of all possibilities.</returns>
        protected MatrixValue Dot(ScalarValue left, MatrixValue right)
		{
			var m = new MatrixValue(right.DimensionY, right.DimensionX);

			for (var i = 1; i <= right.DimensionX; i++)
				for (var j = 1; j <= right.DimensionY; j++)
					m[j, i] = Operation(left, right[j, i]);

			return m;
        }

        #endregion
    }
}

