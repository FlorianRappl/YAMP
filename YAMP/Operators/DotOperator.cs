using System;

namespace YAMP
{
	public abstract class DotOperator : BinaryOperator
	{
		BinaryOperator _top;

		public DotOperator(BinaryOperator top) : base("." + top.Op, top.Level)
		{
			_top = top;
		}

		public abstract ScalarValue Operation(ScalarValue left, ScalarValue right);

		public override Value Perform(Value left, Value right)
		{
			if (!(left is NumericValue))
				throw new OperationNotSupportedException(Op, left);
			else if (!(right is NumericValue))
				throw new OperationNotSupportedException(Op, right);

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

		public MatrixValue Dot(MatrixValue left, MatrixValue right)
		{
			if (left.DimensionX != right.DimensionX)
				throw new DimensionException(left.DimensionX, right.DimensionX);

			if (left.DimensionY != right.DimensionY)
				throw new DimensionException(left.DimensionY, right.DimensionY);

			var m = new MatrixValue(left.DimensionY, left.DimensionX);

			for (var i = 1; i <= left.DimensionX; i++)
				for (var j = 1; j <= left.DimensionY; j++)
					m[j, i] = Operation(left[j, i], right[j, i]);

			return m;
		}

		public MatrixValue Dot(MatrixValue left, ScalarValue right)
		{
			var m = new MatrixValue(left.DimensionY, left.DimensionX);

			for (var i = 1; i <= left.DimensionX; i++)
				for (var j = 1; j <= left.DimensionY; j++)
					m[j, i] = Operation(left[j, i], right);

			return m;
		}

		public MatrixValue Dot(ScalarValue left, MatrixValue right)
		{
			var m = new MatrixValue(right.DimensionY, right.DimensionX);

			for (var i = 1; i <= right.DimensionX; i++)
				for (var j = 1; j <= right.DimensionY; j++)
					m[j, i] = Operation(left, right[j, i]);

			return m;
		}
	}
}

