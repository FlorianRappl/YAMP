using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace YAMP
{
	public class MatrixValue : Value
	{
		List<VectorValue> _values;
		
		public IEnumerable<VectorValue> Values
		{
			get { return _values; }
		}
		
		public int DimensionX
		{
			get { return _values.Count; }
		}
		
		public int DimensionY
		{
			get { return _values.Count > 0 ? _values[0].Dimension : 0; }
		}
		
		public MatrixValue ()
		{
			_values = new List<VectorValue>();
		}
		
		public MatrixValue(IEnumerable<VectorValue> values) : this()
		{
			_values.AddRange(values);
		}
		
		public MatrixValue(Value left) : this()
		{
			AddValue(left);
		}
		
		public MatrixValue(Value left, Value right) : this()
		{
			AddValue(left);
			AddValue(right);
		}

        public MatrixValue Clone()
        {
            var m = new MatrixValue();

            foreach (var vec in this.Values)
                m.AddValue(vec.Clone());

            return m;
        }
		
		void AddValue(Value value)
		{
			if(value is VectorValue)
			{
				var t = value as VectorValue;
				
				if(DimensionX > 0 && t.Dimension != DimensionY)
					throw new DimensionException(t.Dimension, DimensionY);
					
				_values.Add(t);
			}
			else if(value is MatrixValue)
			{
				var t = value as MatrixValue;
				
				if(DimensionX > 0 && t.DimensionY != DimensionY)
					throw new DimensionException(t.DimensionY, DimensionY);
				
				_values.AddRange(t.Values);
			}
			else if(value is ScalarValue)
			{
				var t = value as ScalarValue;
				
				if(DimensionX > 0 && DimensionY	!= 1)
					throw new DimensionException(1, DimensionY);
				
				_values.Add(new VectorValue(t));
			}
			else
				throw new OperationNotSupportedException(";", value);
		}
	
		public override Value Add (Value right)
		{
			if(right is MatrixValue)
			{
                var m = new MatrixValue();
				var r = right as MatrixValue;
				
				if(r.DimensionX != DimensionX)
					throw new DimensionException(DimensionX, r.DimensionX);
				
				for(var i = 0; i < r.DimensionX; i++)
					m.AddValue(_values[i].Add(r._values[i]));
				
				return m;
			}
            else if (right is ScalarValue)
            {
                var m = new MatrixValue();

                foreach (var col in _values)
                    m.AddValue(col.Add(right));

                return m;
            }
			
			throw new OperationNotSupportedException("+", right);
		}
		
		public override Value Power (Value exponent)
		{
			throw new NotImplementedException ();
		}
		
		public override Value Subtract (Value right)
		{
			if(right is MatrixValue)
            {
                var m = new MatrixValue();
				var r = right as MatrixValue;
				
				if(r.DimensionX != DimensionX)
					throw new DimensionException(DimensionX, r.DimensionX);
				
				for(var i = 0; i < r.DimensionX; i++)
					m.AddValue(_values[i].Subtract(r._values[i]));
				
				return m;
            }
            else if (right is ScalarValue)
            {
                var m = new MatrixValue();

                foreach (var col in _values)
                    m.AddValue(col.Subtract(right));

                return m;
            }
			
			throw new OperationNotSupportedException("-", right);
		}

		public override Value Multiply (Value right)
		{	
			if(right is MatrixValue || right is VectorValue)
			{
				var A = this;
				var B = right is MatrixValue ? right as MatrixValue : new MatrixValue(right);
				
				if(A.DimensionX != B.DimensionY)
					throw new DimensionException(A.DimensionX, B.DimensionY);
				
				var m = new List<VectorValue>();
				
				for(var j = 1; j <= B.DimensionX; j++)
				{
					var v = new List<ScalarValue>();
					
					for(var i = 1; i <= A.DimensionY; i++)
					{
						var sum = new ScalarValue();
						
						for(var k = 1; k <= A.DimensionX; k++)
						{
							sum = sum.Add(A[i, k].Multiply(B[k, j])) as ScalarValue;
						}
						
						v.Add(sum);
					}
					
					m.Add(new VectorValue(v));
				}
				
				return new MatrixValue(m);
			}
			else if(right is ScalarValue)
			{
                var A = this.Clone();

				for(var i = 1; i <= DimensionX; i++)
					for(var j = 1; j <= DimensionY; j++)
						A[j, i] = A[j, i].Multiply(right) as ScalarValue;

                return A;
			}
			
			throw new OperationNotSupportedException("*", right);
		}

		public override Value Divide (Value denominator)
		{
			if(denominator is ScalarValue)
			{
                var m = new MatrixValue();
				
				for(var i = 0; i < DimensionX; i++)
					m.AddValue(_values[i].Divide(denominator));
				
				return m;
			}
			//TODO
			
			throw new OperationNotSupportedException("/", denominator);
		}
		
		public override Value Faculty ()
		{
			throw new OperationNotSupportedException("!", this);
		}
		
		public override string ToString ()
		{			
			return string.Format ("Matrix: {0}", MatrixToString());
		}
							
		public ScalarValue this[int j, int i]
		{
			get
			{
				if(i > DimensionX || i < 1)
					throw new ArgumentOutOfRangeException("Access in Matrix out of bounds.");
									
				return _values[i - 1][j];
			}
			set
			{
				if(i > DimensionX || i < 1)
					throw new ArgumentOutOfRangeException("Access in Matrix out of bounds.");
									
				_values[i - 1][j] = value;
			}
		}
		
		public VectorValue this[int i]
		{
			get
			{
				if(i > DimensionX || i < 1)
					throw new ArgumentOutOfRangeException("Access in Matrix out of bounds.");
									
				return _values[i - 1];
			}
		}
		
		public VectorValue GetRowVector(int j)
		{
			var v = new List<ScalarValue>();
			
			for(var i = 1; i <= DimensionX; i++)
				v.Add(this[j, i]);
			
			return new VectorValue(v);
		}
		
		public VectorValue GetColumnVector(int i)
		{
			return this[i];
		}
		
		public MatrixValue Transpose()
		{
			var m = new List<VectorValue>();
			
			for(var i = 1; i <= DimensionY; i++)
			{
				var v = new List<ScalarValue>();
				
				for(var j = 1; j <= DimensionX; j++)
				{
					v.Add(this[i, j]);
				}
				
				m.Add(new VectorValue(v));
			}
			
			return new MatrixValue(m);
		}
		
		public ScalarValue Det()
		{
			if(DimensionX == DimensionY)
			{
				if(DimensionX == 1)
					return this[1, 1];
                else if (DimensionX == 2)
                    return this[1, 1] * this[2, 2] - this[1, 2] * this[2, 1];
			}
			
			return new ScalarValue(0.0);
		}
		
		public string MatrixToString()
		{
			var sb = new StringBuilder();
			sb.Append("(").Append(_values[0].VectorToString());
			
			for(var i = 1; i < _values.Count; i++)
				sb.Append(";").Append(_values[i].VectorToString());
			
			sb.Append(")");
			return sb.ToString();
		}
	}
}

