/*
	Copyright (c) 2012, Florian Rappl.
	All rights reserved.

	Redistribution and use in source and binary forms, with or without
	modification, are permitted provided that the following conditions are met:
		* Redistributions of source code must retain the above copyright
		  notice, this list of conditions and the following disclaimer.
		* Redistributions in binary form must reproduce the above copyright
		  notice, this list of conditions and the following disclaimer in the
		  documentation and/or other materials provided with the distribution.
		* Neither the name of the YAMP team nor the names of its contributors
		  may be used to endorse or promote products derived from this
		  software without specific prior written permission.

	THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
	ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
	WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
	DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
	DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
	(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
	LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
	ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
	(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
	SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace YAMP
{
	public class MatrixValue : NumericValue, IFunction, ISetFunction
	{
		#region Members

		IDictionary<MatrixIndex, ScalarValue> _values;
		
		int dimX;
		int dimY;

		#endregion

		#region Properties

        public bool IsScalar
        {
            get { return DimensionX == 1 && DimensionY == 1; }
        }

        public bool IsVector
        {
            get { return (DimensionX == 1 && DimensionY > 1) || (DimensionY == 1 && DimensionX > 1); }
        }

		public int DimensionX
		{
			get { return dimX;  }
			protected set { dimX = value; }
		}
		
		public int DimensionY
		{
			get { return dimY; }
			protected set { dimY = value; }
		}
		
		public int Length
		{
			get { return DimensionX * DimensionY; }
		}

		public bool IsSymmetric
		{
			get
			{
				if(dimX != dimY)
					return false;

				for (var i = 1; i <= dimX; i++)
				{
					for (var j = 1; j < i; j++)
					{
						if (this[i, j].Value != this[j, i].Value)
							return false;

						if (this[i, j].ImaginaryValue != -this[j, i].ImaginaryValue)
							return false;
					}
				}

				return true;
			}
		}

		public bool IsComplex
		{
			get
			{
				for (var i = 1; i <= dimY; i++)
				{
					for (var j = 1; j < dimX; j++)
					{
						if (this[i, j].IsComplex)
							return true;
					}
				}

				return false;
			}
		}

		#endregion

		#region ctors

		public MatrixValue ()
		{
			_values = new Dictionary<MatrixIndex, ScalarValue>();
		}
		
		public MatrixValue(int rows, int cols) : this()
		{
			dimX = cols;
			dimY = rows;
		}

		public MatrixValue(double[][] values, int rows, int cols) : this(rows, cols)
		{
			for (var j = 0; j < values.Length; j++)
			{
				for (var i = 0; i < values[j].Length; i++)
				{
					if (values[j][i] == 0.0)
						continue;

					this[j + 1, i + 1] = new ScalarValue(values[j][i]);
				}
			}
		}

		#endregion

		#region Statics

		public static MatrixValue Create(Value value)
		{
			if(value is MatrixValue)
				return value as MatrixValue;
			else if(value is ScalarValue)
			{
				var m = new MatrixValue();
				m[1, 1] = value as ScalarValue;
				return m;
			}
			
			throw new ArgumentException("matrix");
		}

		public static MatrixValue One(int dimension)
		{
			var m = new MatrixValue(dimension, dimension);

			for (var i = 1; i <= dimension; i++)
				m[i, i] = new ScalarValue(1.0);

			return m;
		}

		public static MatrixValue Ones(int rows, int cols)
		{
			var m = new MatrixValue(rows, cols);

			for (var j = 1; j <= rows; j++)
				for (var i = 1; i <= cols; i++)
					m[j, i] = new ScalarValue(1.0);

			return m;
		}

		#endregion

		#region Methods

		public int MaximumLength
		{
			get
			{
				var max = 0;

				foreach (var el in _values.Values)
					if (el.Length > max)
						max = el.Length;

				return max;
			}
		}

		public MatrixValue VectorSort()
		{
			var v = new MatrixValue(1, Length);

			for (var k = 1; k <= Length; k++)
				v[k] = this[k].Clone();

			for (var i = 1; i < Length; i++)
			{
				for (var j = i + 1; j <= Length; j++)
				{
					if (v[j] < v[i])
					{
						var index1 = v.GetIndex(j);
						var index2 = v.GetIndex(i);

						var tmp = v._values[index1];
						v._values[index1] = v._values[index2];
						v._values[index2] = tmp;
					}
				}
			}

			return v;
		}

		public ScalarValue Sum()
		{
			var s = new ScalarValue();

			foreach (var entry in _values)
				s += entry.Value;

			return s;
		}

		public override void Clear()
		{
			for (var i = 1; i <= DimensionX; i++)
				for (var j = 1; j <= DimensionY; j++)
					this[j, i].Clear();
		}

		public virtual MatrixValue Clone()
		{
			var m = new MatrixValue();

			foreach (var entry in _values)
				m._values.Add(entry.Key, entry.Value.Clone());

			m.dimX = dimX;
			m.dimY = dimY;
			return m;
		}
		
		public MatrixValue AddColumn(Value value)
		{
			var that = Clone();

			if(value is MatrixValue)
			{
				var t = value as MatrixValue;				
				int j, i = 1, offset = DimensionX + 1;
				
				for(var k = 1; k <= t.DimensionY; k++)
				{
					j = offset;
					
					for(var l = 1; l <= t.DimensionX; l++)
						that[i, j++] = t[k, l];
					
					i++;
				}

				return that;
			}
			else if(value is ScalarValue)
			{
				var t = value as ScalarValue;
				that[1, DimensionX + 1] = t;
				return that;
			}
			
			throw new OperationNotSupportedException(",", value);
		}
		
		public MatrixValue AddRow(Value value)
		{
			var that = Clone();

			if(value is MatrixValue)
			{
				var t = value as MatrixValue;				
				int j, i = DimensionY + 1;
				
				for(var k = 1; k <= t.DimensionY; k++)
				{
					j = 1;
					
					for(var l = 1; l <= t.DimensionX; l++)
						that[i, j++] = t[k, l];
					
					i++;
				}

				return that;
			}
			else if(value is ScalarValue)
			{
				var t = value as ScalarValue;
				that[DimensionY + 1, 1] = t;
				return that;
			}
			
			throw new OperationNotSupportedException(";", value);
		}
	
		public override Value Add(Value right)
		{
			if(right is MatrixValue)
			{
				var r = right as MatrixValue;
				
				if(r.DimensionX != DimensionX)
					throw new DimensionException(DimensionX, r.DimensionX);
				
				if(r.DimensionY != DimensionY)
					throw new DimensionException(DimensionY, r.DimensionY);
				
				var m = new MatrixValue(DimensionY, DimensionX);
				
				for(var j = 1; j <= DimensionY; j++)
					for(var i = 1; i <= DimensionX; i++)
						m[j, i] = this[j, i].Add(r[j, i]) as ScalarValue;				
				
				return m;
			}
			else if (right is ScalarValue)
			{
				var m = new MatrixValue(DimensionY, DimensionX);

				for(var j = 1; j <= DimensionY; j++)
					for(var i = 1; i <= DimensionX; i++)
						m[j, i] = this[j, i].Add(right) as ScalarValue;

				return m;
			}
			
			throw new OperationNotSupportedException("+", right);
		}
		
		public override Value Power(Value exponent)
		{
			if (DimensionX != DimensionY)
				throw new DimensionException(DimensionX, DimensionY);

			if (exponent is ScalarValue)
			{
				if (DimensionX == 1)
					return this[1, 1].Power(exponent);

				var exp = exponent as ScalarValue;
				
				if(exp.ImaginaryValue != 0.0 || Math.Floor(exp.Value) != exp.Value)
					throw new OperationNotSupportedException("^", exponent);

				var eye = MatrixValue.One(DimensionX);
				var multiplier = this;
				var count = (int)Math.Abs(exp.Value);

				if (exp.Value < 0)
					multiplier = this.Inverse();

				for (var i = 0; i < count; i++)
					eye = eye.Multiply(multiplier) as MatrixValue;

				return eye;
			}

			throw new OperationNotSupportedException("^", exponent);
		}
		
		public override Value Subtract(Value right)
		{
			if(right is MatrixValue)
			{
				var r = right as MatrixValue;
				
				if(r.DimensionX != DimensionX)
					throw new DimensionException(DimensionX, r.DimensionX);
				
				if(r.DimensionY != DimensionY)
					throw new DimensionException(DimensionY, r.DimensionY);
				
				var m = new MatrixValue(DimensionY, DimensionX);
				
				for(var j = 1; j <= DimensionY; j++)
					for(var i = 1; i <= DimensionX; i++)
						m[j, i] = this[j, i].Subtract(r[j, i]) as ScalarValue;				
				
				return m;
			}
			else if (right is ScalarValue)
			{
				var m = new MatrixValue(DimensionY, DimensionX);

				for(var j = 1; j <= DimensionY; j++)
					for(var i = 1; i <= DimensionX; i++)
						m[j, i] = this[j, i].Subtract(right) as ScalarValue;

				return m;
			}
			
			throw new OperationNotSupportedException("-", right);
		}

		public override Value Multiply(Value right)
		{	
			if(right is MatrixValue)
			{
				var A = this;
				var B = right as MatrixValue;
				
				if(A.DimensionX != B.DimensionY)
					throw new DimensionException(A.DimensionX, B.DimensionY);
				
				var M = new MatrixValue(A.DimensionY, B.DimensionX);
				
				for(var j = 1; j <= B.DimensionX; j++)
				{					
					for(var i = 1; i <= A.DimensionY; i++)
					{						
						for(var k = 1; k <= A.DimensionX; k++)
							M[i, j] = M[i, j].Add(A[i, k].Multiply(B[k, j])) as ScalarValue;
						
						if(A.DimensionY == B.DimensionX && A.DimensionY == 1)
							return M[1, 1];
					}
				}
				
				return M;
			}
			else if(right is ScalarValue)
			{
				var A = new MatrixValue(DimensionY, DimensionX);

				for(var i = 1; i <= DimensionX; i++)
					for(var j = 1; j <= DimensionY; j++)
						A[j, i] = this[j, i].Multiply(right) as ScalarValue;

				return A;
			}
			
			throw new OperationNotSupportedException("*", right);
		}

		public override Value Divide(Value denominator)
		{
			if(denominator is ScalarValue)
			{
				var m = new MatrixValue(DimensionY, DimensionX);
				
				for(var j = 1; j <= DimensionY; j++)
					for(var i = 1; i <= DimensionX; i++)
						m[j, i] = this[j, i].Divide(denominator) as ScalarValue;
				
				return m;
			}
			else if (denominator is MatrixValue)
			{
				var Q = denominator as MatrixValue;
				
				if(DimensionX != Q.DimensionX)
					throw new DimensionException(DimensionX, Q.DimensionX);
				
				return this.Multiply(Q.Inverse());
			}
			
			throw new OperationNotSupportedException("/", denominator);
		}
		
		public override byte[] Serialize()
		{
		    byte[] content;

		    using (var ms = new MemoryStream())
		    {
		        var dy = BitConverter.GetBytes(dimY);
		        ms.Write(dy, 0, dy.Length);
		        var dx = BitConverter.GetBytes(dimX);
		        ms.Write(dx, 0, dx.Length);
				var count = BitConverter.GetBytes(_values.Count);
				ms.Write(count, 0, count.Length);

		        foreach(var entry in _values)
		        {
					var j = BitConverter.GetBytes(entry.Key.Row);
					var i = BitConverter.GetBytes(entry.Key.Column);
					var buffer = entry.Value.Serialize();
					ms.Write(j, 0, j.Length);
					ms.Write(i, 0, i.Length);
					ms.Write(buffer, 0, buffer.Length);
		        }

		        content = ms.ToArray();
		    }

		    return content;
		}

		public override Value Deserialize(byte[] content)
		{
			dimY = BitConverter.ToInt32 (content, 0);
			dimX = BitConverter.ToInt32(content, 4);
			var count = BitConverter.ToInt32(content, 8);
			var pos = 12;

			for(var i = 0; i < count; i++)
			{
				var row = BitConverter.ToInt32(content, pos);
				var col = BitConverter.ToInt32(content, pos + 4);
				var re = BitConverter.ToDouble(content, pos + 8);
				var im = BitConverter.ToDouble(content, pos + 16);
				_values.Add(new MatrixIndex
				{
					Column = col,
					Row = row
				}, new ScalarValue(re, im));

				pos += 24;
			}

			return this;
		}
		
		public MatrixValue Inverse()
		{
			var target = One(DimensionX);

			if(DimensionX != DimensionY || DimensionX < 32)
			{
				var lu = new YAMP.Numerics.LUDecomposition(this);
				return lu.Solve(target);
			}

			var qr = new YAMP.Numerics.QRDecomposition(this);
			return qr.Solve(target);
		}
		
		public override string ToString(ParseContext context)
		{			
			var sb = new StringBuilder();
			
			for(var j = 1; j <= DimensionY; j++)
			{
				for(var i = 1; i <= DimensionX; i++)
				{
					sb.Append(this[j, i].ToString(context));
					
					if(i < DimensionX)
						sb.Append("\t");
				}
				
				if(j < DimensionY)
					sb.AppendLine();
			}
			
			return sb.ToString();
		}

		public MatrixValue GetRowVector(int j)
		{
			var m = new MatrixValue(1, DimensionX);

			for (var i = 1; i <= DimensionX; i++)
				m[1, i] = this[j, i].Clone();

			return m;
		}

		public void SetRowVector(int j, MatrixValue m)
		{
			if (m.Length != DimensionX)
				throw new DimensionException(m.Length, DimensionX);

			for (var i = 1; i <= DimensionX; i++)
				this[j, i] = m[i].Clone();
		}

		public MatrixValue GetColumnVector(int i)
		{
			var m = new MatrixValue(DimensionY, 1);

			for (var j = 1; j <= DimensionY; j++)
				m[j, 1] = this[j, i].Clone();

			return m;
		}

		public void SetColumnVector(int i, MatrixValue m)
		{
			if (m.Length != DimensionY)
				throw new DimensionException(m.Length, DimensionY);

			for (var j = 1; j <= DimensionY; j++)
				this[j, i] = m[j].Clone();
		}

		public MatrixValue Adjungate()
		{
			var m = Transpose();

			foreach (var pair in m._values)
				pair.Value.ImaginaryValue = -pair.Value.ImaginaryValue;

			return m;
		}

		public MatrixValue Transpose()
		{
			var m = Clone();
			var nv = new Dictionary<MatrixIndex, ScalarValue>();

			foreach (var pair in m._values)
			{
				nv.Add(new MatrixIndex
				{
					Row = pair.Key.Column,
					Column = pair.Key.Row
				}, pair.Value);
			}

			m._values = nv;
			m.dimX = dimY;
			m.dimY = dimX;
			return m;
		}

		public override ScalarValue Abs()
		{
			var sum = 0.0;

			for (var i = 1; i <= DimensionX; i++)
				for (var j = 1; j <= DimensionY; j++)
					sum += this[j, i].AbsSquare().Value;

			return new ScalarValue(Math.Sqrt(sum));
		}

		public ScalarValue Trace()
		{
			var sum = new ScalarValue();
			var n = Math.Min(DimensionX, DimensionY);

			for (var i = 1; i <= n; i++)
			{
				sum.Value += this[i, i].Value;
				sum.ImaginaryValue += this[i, i].ImaginaryValue;
			}

			return sum;
		}

		public ScalarValue Det()
		{
			if (DimensionX == DimensionY)
			{
				var n = DimensionX;

				if (n == 1)
					return this[1, 1];
				else if (n == 2)
					return this[1, 1] * this[2, 2] - this[1, 2] * this[2, 1];
				else if (n == 3)
				{
					return this[1, 1] * (this[2, 2] * this[3, 3] - this[2, 3] * this[3, 2]) +
							this[1, 2] * (this[2, 3] * this[3, 1] - this[2, 1] * this[3, 3]) +
							this[1, 3] * (this[2, 1] * this[3, 2] - this[2, 2] * this[3, 1]);
				}
				else if (n == 4)
				{
					return this[1, 1] * (this[2, 2] * (this[3, 3] * this[4, 4] - this[3, 4] * this[4, 3]) + this[2, 3] * (this[3, 4] * this[4, 2] - this[3, 2] * this[4, 4]) + this[2, 4] * (this[3, 2] * this[4, 3] - this[3, 3] * this[4, 2])) -
							this[1, 2] * (this[2, 1] * (this[3, 3] * this[4, 4] - this[3, 4] * this[4, 3]) + this[2, 3] * (this[3, 4] * this[4, 1] - this[3, 1] * this[4, 4]) + this[2, 4] * (this[3, 1] * this[4, 3] - this[3, 3] * this[4, 1])) +
							this[1, 3] * (this[2, 1] * (this[3, 2] * this[4, 4] - this[3, 4] * this[4, 2]) + this[2, 2] * (this[3, 4] * this[4, 1] - this[3, 1] * this[4, 4]) + this[2, 4] * (this[3, 1] * this[4, 2] - this[3, 2] * this[4, 1])) -
							this[1, 4] * (this[2, 1] * (this[3, 2] * this[4, 3] - this[3, 3] * this[4, 2]) + this[2, 2] * (this[3, 3] * this[4, 1] - this[3, 1] * this[4, 3]) + this[2, 3] * (this[3, 1] * this[4, 2] - this[3, 2] * this[4, 1]));
				}

				var lu = new YAMP.Numerics.LUDecomposition(this);
				return new ScalarValue(lu.Determinant());
			}

			return new ScalarValue();
		}

		public override int GetHashCode()
		{
			return dimX + dimY;
		}

		public override bool Equals(object obj)
		{
			if (obj is MatrixValue)
			{
				var m = obj as MatrixValue;

				if (m.DimensionX != DimensionX)
					return false;

				if (m.DimensionY != DimensionY)
					return false;

				for (var i = 1; i <= DimensionX; i++)
					for (var j = 1; j <= DimensionY; j++)
						if (!this[j, i].Equals(m[j, i]))
							return false;

				return true;
			}
			else if (DimensionX == 1 && DimensionY == 1)
				return this[1, 1].Equals(obj);

			return false;
		}

        public double[] GetRealVector(int yoffset, int ylength, int xoffset, int xlength)
        {
            var k = 0;
            var array = new double[ylength * xlength];

            for (var i = 1 + xoffset; i <= xlength; i++)
            {
                for (var j = 1 + yoffset; j <= ylength; j++)
                    array[k++] = this[j, i].Value;
            }

            return array;
        }

		public double[][] GetRealArray()
		{
			var array = new double[DimensionY][];

			for (var j = 0; j < DimensionY; j++)
			{
				array[j] = new double[DimensionX];

				for (var i = 0; i < DimensionX; i++)
					array[j][i] = this[j + 1, i + 1].Value;
			}

			return array;
		}

		public MatrixValue SubMatrix(int yoffset, int yfinal, int xoffset, int xfinal)
		{
			var X = new MatrixValue(yfinal - yoffset, xfinal - xoffset);

			for (int j = yoffset + 1; j <= yfinal; j++)
				for (int i = xoffset + 1; i <= xfinal; i++)
					X[j - yoffset, i - xoffset] = this[j, i].Clone();

			return X;
		}

		public MatrixValue SubMatrix(int[] y, int xoffset, int xfinal)
		{
			var X = new MatrixValue(y.Length, xfinal - xoffset);

			for (int j = 1; j <= y.Length; j++)
				for (int i = xoffset + 1; i <= xfinal; i++)
					X[j, i - xoffset] = this[y[j - 1], i].Clone();

			return X;
		}

		#endregion

		#region Indexers

		public virtual ScalarValue this[int j, int i]
		{
			get
			{
				if (i > dimX || i < 1 || j > dimY || j < 1)
					throw new ArgumentOutOfRangeException("Access in Matrix out of bounds.");

				var index = new MatrixIndex();
				index.Column = i;
				index.Row = j;
				
				if(_values.ContainsKey(index))
					return _values[index];

				return new ScalarValue();
			}
			set
			{
				if(i < 1 || j < 1)
					throw new ArgumentOutOfRangeException("Access in Matrix out of bounds.");

				if (i > dimX)
					dimX = i;

				if (j > dimY)
					dimY = j;

				var index = new MatrixIndex();
				index.Column = i;
				index.Row = j;

				if (value.IsZero)
				{
					if (_values.ContainsKey(index))
						_values.Remove(index);
				}
				else if (_values.ContainsKey(index))
					_values[index] = value;
				else
					_values.Add(index, value);
			}
		}
		
		public virtual ScalarValue this[int i]
		{
			get
			{
				if(i > Length || i < 1)
					throw new ArgumentOutOfRangeException("Access in Matrix out of bounds.");

                var index = GetIndex(i);
                return this[index.Row, index.Column];
			}
			set
			{
				if(i < 1)
					throw new ArgumentOutOfRangeException("Access in Matrix out of bounds.");

                var index = GetIndex(i);
				this[index.Row, index.Column] = value;
			}
		}

		protected bool ContainsIndex(MatrixIndex index)
		{
			return _values.ContainsKey(index);
		}

        protected MatrixIndex GetIndex(int i)
        {
            var dimY = Math.Max(1, this.dimY);
            var row = (i - 1) % dimY + 1;
            var col = (i - 1) / dimY + 1;
                            
            return new MatrixIndex
            {
                Row = row,
                Column = col
            };
        }

		protected ScalarValue GetIndex(MatrixIndex index)
		{
			return _values[index];
		}

		#endregion
        
		#region Operators

		public static MatrixValue operator *(MatrixValue a, MatrixValue b)
		{
			return a.Multiply(b) as MatrixValue;
		}

		public static MatrixValue operator *(ScalarValue a, MatrixValue b)
		{
			return b.Multiply(a) as MatrixValue;
		}

		public static MatrixValue operator /(MatrixValue a, MatrixValue b)
		{
			return a.Divide(b) as MatrixValue;
		}

		public static MatrixValue operator -(MatrixValue a, MatrixValue b)
		{
			return a.Subtract(b) as MatrixValue;
		}

		public static MatrixValue operator +(MatrixValue a, MatrixValue b)
		{
			return a.Add(b) as MatrixValue;
		}

		public static bool operator ==(MatrixValue l, MatrixValue r)
		{
			if (ReferenceEquals(l, r))
				return true;

			if ((object)l == null || (object)r == null)
				return false;

			if (l.DimensionX != r.DimensionX)
				return false;

			if (l.DimensionY != r.DimensionY)
				return false;

			for (var i = 1; i <= l.DimensionX; i++)
				for (var j = 1; j <= l.DimensionY; j++)
					if (l[j, i] != r[j, i])
						return false;

			return true;
		}

		public static bool operator !=(MatrixValue l, MatrixValue r)
		{
			return !(l == r);
		}

		#endregion

        #region Behavior as method

        public Value Perform(ParseContext context, Value argument, Value values)
        {
            if (!(values is NumericValue))
                throw new OperationNotSupportedException("matrix-set", values);

            var indices = new List<MatrixIndex>();

            if (argument is ArgumentsValue)
            {
                var ags = (ArgumentsValue)argument;

                if (ags.Length == 1)
                    return Perform(context, ags[1], values);
                else if (ags.Length > 2)
                    throw new ArgumentsException("matrix-index", 3);

                var rows = BuildIndex(ags[1], DimensionY);
                var columns = BuildIndex(ags[2], DimensionX);

                for (int i = 0; i < columns.Length; i++)
                {
                    for (int j = 0; j < rows.Length; j++)
                    {
                        indices.Add(new MatrixIndex
                        {
                            Column = columns[i],
                            Row = rows[j]
                        });
                    }
                }
            }
            else if (argument is NumericValue)
            {
                if (argument is MatrixValue)
                {
                    var mm = (MatrixValue)argument;

                    if (mm.DimensionX == DimensionX && mm.DimensionY == DimensionY)
                        LogicalSubscripting(mm, indices);
                }

                if (indices.Count == 0)
                {
                    var idx = BuildIndex(argument, Length);

                    for (int i = 0; i < idx.Length; i++)
                        indices.Add(GetIndex(idx[i]));
                }
            }
            else
                throw new OperationNotSupportedException("matrix-index", argument);

            if (values is MatrixValue)
            {
                var index = 1;
                var m = (MatrixValue)values;

                if (m.Length != indices.Count)
                    throw new DimensionException(m.Length, indices.Count);

                foreach (var mi in indices)
                    this[mi.Row, mi.Column] = m[index++];
            }
            else if(values is ScalarValue)
            {
                var value = (ScalarValue)values;

                foreach (var mi in indices)
                    this[mi.Row, mi.Column] = value.Clone();
            }

            return this;
        }

        public Value Perform(ParseContext context, Value argument)
        {
            if (argument is ArgumentsValue)
            {
                var ags = (ArgumentsValue)argument;

                if (ags.Length == 1)
                    return Perform(context, ags[1]);
                else if(ags.Length > 2)
                    throw new ArgumentsException("matrix-index", 3);

                var rows = BuildIndex(ags[1], DimensionY);
                var columns = BuildIndex(ags[2], DimensionX);

                if (rows.Length == 1 && columns.Length == 1)
                    return this[rows[0], columns[0]].Clone();

                var m = new MatrixValue(rows.Length, columns.Length);

                for (int i = 1; i <= m.DimensionX; i++)
                    for (int j = 1; j <= m.DimensionY; j++)
                        m[j, i] = this[rows[j - 1], columns[i - 1]].Clone();

                return m;
            }
            else if (argument is NumericValue)
            {
                if (argument is MatrixValue)
                {
                    var mm = (MatrixValue)argument;

                    if(mm.DimensionX == DimensionX && mm.DimensionY == DimensionY)
                        return LogicalSubscripting(mm);
                }

                var idx = BuildIndex(argument, Length);

                if (idx.Length == 1)
                    return this[idx[0]].Clone();

                var m = new MatrixValue(1, idx.Length);

                for (int i = 1; i <= m.DimensionX; i++)
                    m[i] = this[idx[i - 1]].Clone();

                return m;
            }

            throw new OperationNotSupportedException("matrix-index", argument);
        }

        MatrixValue LogicalSubscripting(MatrixValue m)
        {
            var indices = new List<MatrixIndex>();
            LogicalSubscripting(m, indices);
            var n = new MatrixValue(1, indices.Count);

            for (var i = 1; i <= indices.Count; i++)
            {
                var index = indices[i - 1];
                n[1, i] = this[index.Row, index.Column].Clone();
            }

            return n;
        }

        void LogicalSubscripting(MatrixValue m, List<MatrixIndex> indices)
        {
            for (var i = 1; i <= m.DimensionX; i++)
            {
                for (var j = 1; j <= m.DimensionY; j++)
                {
                    if (m[j, i].Value != 0.0)
                    {
                        indices.Add(new MatrixIndex
                        {
                            Row = j,
                            Column = i
                        });
                    }
                }
            }
        }

        #endregion
    }
}

