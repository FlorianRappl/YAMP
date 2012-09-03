using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace YAMP
{
	public class VectorValue : Value
	{
		List<ScalarValue> _values;
		
		public IEnumerable<ScalarValue> Values
		{
			get { return _values; }
		}
		
		public int Dimension
		{
			get { return _values.Count; }
		}
		
		public VectorValue ()
		{
			_values = new List<ScalarValue>();
		}
		
		public VectorValue (ScalarValue value) : this(new []{ value })
		{
		}
		
		public VectorValue(IEnumerable<ScalarValue> values) : this()
		{
			_values.AddRange(values);
		}
		
		public VectorValue(Value left, Value right) : this()
		{
			AddValue(left);
			AddValue(right);
		}

        public VectorValue Clone()
        {
            var v = new VectorValue();

            foreach (var scalar in this.Values)
                v.AddValue(scalar.Clone());

            return v;
        }
		
		void AddValue(Value value)
		{
			if(value is ScalarValue)
				_values.Add(value as ScalarValue);
			else if(value is VectorValue)
				_values.AddRange((value as VectorValue).Values);
			else
				throw new OperationNotSupportedException(",", value);
		}
	
		public override Value Add (Value right)
		{
			if(right is VectorValue)
			{
                var v = new VectorValue();
				var r = right as VectorValue;
				
				if(r.Dimension != Dimension)
					throw new DimensionException(Dimension, r.Dimension);
				
				for(var i = 0; i < r.Dimension; i++)
					v.AddValue(_values[i].Add(r._values[i]));
				
				return v;
			}
            else if (right is ScalarValue)
            {
                var v = new VectorValue();
                var r = right as ScalarValue;

                for (var i = 0; i < Dimension; i++)
                    v.AddValue(r.Add(_values[i]));

                return v;
            }
			
			throw new OperationNotSupportedException("+", right);
		}
		
		public override Value Subtract (Value right)
		{
			if(right is VectorValue)
            {
                var v = new VectorValue();
				var r = right as VectorValue;
				
				if(r.Dimension != Dimension)
					throw new DimensionException(Dimension, r.Dimension);
				
				for(var i = 0; i < r.Dimension; i++)
					v.AddValue(_values[i].Subtract(r._values[i]));
				
				return v;
			}
            else if (right is ScalarValue)
            {
                var v = new VectorValue();
                var r = right as ScalarValue;

                for (var i = 0; i < Dimension; i++)
                    v.AddValue(_values[i].Subtract(r));

                return v;
            }
			
			throw new OperationNotSupportedException("-", right);
		}

		public override Value Multiply (Value right)
		{
			if(right is ScalarValue)
            {
                var v = new VectorValue();
				var r = right as ScalarValue;
				
				for(var i = 0; i < Dimension; i++)
					v.AddValue(_values[i].Multiply(r));
				
				return v;
			}
			
			throw new OperationNotSupportedException("*", right);
		}

		public override Value Divide (Value denominator)
		{
			if(denominator is ScalarValue)
            {
                var v = new VectorValue();
				var d = denominator as ScalarValue;
				
				for(var i = 0; i < Dimension; i++)
					v.AddValue(_values[i].Divide(d));
				
				return v;
			}
			
			throw new OperationNotSupportedException("/", denominator);
		}
		
		public override Value Faculty ()
		{
			throw new OperationNotSupportedException("!", this);
		}
		
		public override Value Power (Value exponent)
		{
			throw new OperationNotSupportedException("^", this);
		}
		
		public override string ToString ()
		{			
			return string.Format ("Vector: {0}", VectorToString());
		}
		
		public ScalarValue Abs()
		{
			var sum = 0.0;
			
			foreach(var p in _values)
			{
				sum += (p.Value * p.Value + p.ImaginaryValue * p.ImaginaryValue);
			}
			
			return new ScalarValue(Math.Sqrt(sum));
		}
		
		public ScalarValue this[int i]
		{
			get
			{
				if(i < 1 || i > Dimension)
					throw new ArgumentOutOfRangeException("Access in Vector out of bounds.");
				
				return _values[i - 1];
			}
			set
			{
				if(i < 1 || i > Dimension)
					throw new ArgumentOutOfRangeException("Access in Vector out of bounds.");
				
				_values[i - 1] = value;
			}
		}
		
		public string VectorToString()
		{
			var sb = new StringBuilder();
			sb.Append("(").Append(_values[0].ValueToString());
			
			for(var i = 1; i < Dimension; i++)
				sb.Append(",").Append(_values[i].ValueToString());
			
			sb.Append(")");
			return sb.ToString();
		}
	}
}

