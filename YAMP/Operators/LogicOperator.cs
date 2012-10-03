using System;

namespace YAMP
{
    public abstract class LogicOperator : BinaryOperator
	{
		public LogicOperator (string op) : base(op, 3)
		{
		}

		public abstract ScalarValue Compare(ScalarValue left, ScalarValue right);
		
		public override Value Perform (Value left, Value right)
		{
			if(!(left is ScalarValue || left is MatrixValue))
				throw new OperationNotSupportedException(Op, left);

			if(!(right is ScalarValue || right is MatrixValue))
				throw new OperationNotSupportedException(Op, right);
			
			if(left is ScalarValue && right is ScalarValue)
			{
				return Compare (left as ScalarValue, right as ScalarValue);
			}
			else if(left is MatrixValue && right is ScalarValue)
			{
				var l = left as MatrixValue;
				var r = right as ScalarValue;
				var m = new MatrixValue(l.DimensionY, l.DimensionX);
				
				for(var i = 1; i <= m.DimensionX; i++)
					for(var j = 1; j <= m.DimensionY; j++)
						m[j, i] = Compare(l[j, i], r);
				
				return m;
			}
			else if(left is ScalarValue && right is MatrixValue)
			{
				var l = left as ScalarValue;
				var r = right as MatrixValue;
				var m = new MatrixValue(r.DimensionY, r.DimensionX);
				
				for(var i = 1; i <= m.DimensionX; i++)
					for(var j = 1; j <= m.DimensionY; j++)
						m[j, i] = Compare(l, r[j, i]);
				
				return m;
			}
			else
			{
				var l = left as MatrixValue;
				var r = right as MatrixValue;

				if(l.DimensionX != r.DimensionX)
					throw new DimensionException(l.DimensionX, r.DimensionX);

				if(l.DimensionY != r.DimensionY)
					throw new DimensionException(l.DimensionY, r.DimensionY);

				var m = new MatrixValue(l.DimensionY, l.DimensionX);
				
				for(var i = 1; i <= m.DimensionX; i++)
					for(var j = 1; j <= m.DimensionY; j++)
						m[j, i] = Compare(l[j, i], r[j, i]);
				
				return m;
			}
		}
	}
}

