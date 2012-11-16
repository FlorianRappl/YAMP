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
	public class MatrixValue : NumericValue, IHasIndex, ISign
	{
		#region Members

		IDictionary<MatrixIndex, ScalarValue> _values;
		
		int dimX;
		int dimY;

		#endregion

		#region Properties

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
			for (var j = 0; j < rows; j++)
			{
				for (var i = 0; i < cols; i++)
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

		public virtual Value ChangeSign()
		{
			var m = new MatrixValue(DimensionY, DimensionX);

			for (var i = 1; i <= DimensionX; i++)
			{
				for (var j = 1; j <= DimensionY; j++)
				{
					m[j, i].Value = -this[j, i].Value;
					m[j, i].ImaginaryValue = -this[j, i].ImaginaryValue;
				}
			}

			return m;
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

		        for (var i = 1; i <= dimX; i++)
		        {
		            for (var j = 1; j <= dimY; j++)
		            {
		                var buffer = this[j, i].Serialize();
		                ms.Write(buffer, 0, buffer.Length);
		            }
		        }

		        content = ms.ToArray();
		    }

		    return content;
		}

		public override Value Deserialize(byte[] content)
		{
			var dy = BitConverter.ToInt32 (content, 0);
			var dx = BitConverter.ToInt32 (content, 4);
			var pos = 8;

			for(var i = 1; i <= dx; i++)
			{
				for(var j = 1; j <= dy; j++)
				{
					var re = BitConverter.ToDouble(content, pos);
					var im = BitConverter.ToDouble(content, pos + 8);
					this[j, i] = new ScalarValue(re, im);
					pos += 16;
				}
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
			var m = new MatrixValue(dimX, dimY);

			for (var i = 1; i <= DimensionY; i++)
			{
				for (var j = 1; j <= DimensionX; j++)
					m[j, i] = this[i, j].Conjugate();
			}

			return m;
		}

		public MatrixValue Transpose()
		{
			var m = Clone();

			foreach (var pair in _values)
			{
				var index = pair.Key;
				var temp = index.Row;
				index.Row = index.Column;
				index.Column = temp;
			}

			return m;
		}

		public override ScalarValue Abs()
		{
			var sum = 0.0;

			foreach (var p in _values)
				sum += (p.Value.Value * p.Value.Value + p.Value.ImaginaryValue * p.Value.ImaginaryValue);

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

		public MatrixValue SubMatrix(int dy, int dimy, int dx, int dimx)
		{
			var X = new MatrixValue(dimy - dy, dimx - dx);

			for (int j = dy + 1; j <= dimy; j++)
				for (int i = dx + 1; i <= dimx; i++)
					X[j - dy, i - dx] = this[j, i].Clone();

			return X;
		}

		public MatrixValue SubMatrix(int[] y, int dx, int dimx)
		{
			var X = new MatrixValue(y.Length, dimx - dx);

			for (int j = 1; j <= y.Length; j++)
				for (int i = dx + 1; i <= dimx; i++)
					X[j, i - dx] = this[y[j - 1], i].Clone();

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

				var row = (i - 1) % dimY + 1;
				var col = (i - 1) / dimY + 1;
				return this[row, col];
			}
			set
			{
				if(i < 1)
					throw new ArgumentOutOfRangeException("Access in Matrix out of bounds.");

				if (dimY == 0)
					dimY = 1;

				var row = (i - 1) % dimY + 1;
				var col = (i - 1) / dimY + 1;
				this[row, col] = value;
			}
		}

		protected bool ContainsIndex(MatrixIndex index)
		{
			return _values.ContainsKey(index);
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

		#region Index

		public int[] Dimensions
		{
			get
			{
				return new int[] { DimensionY, DimensionX };
			}
		}

		public IHasIndex Create(int[] _dimensions)
		{
			if(_dimensions.Length == 1)
				return new MatrixValue(_dimensions[0], 1);

			return new MatrixValue(_dimensions[0], _dimensions[1]);
		}

		public Value Get(IIsIndex index)
		{
			if (index is VectorIndex)
				return this[((VectorIndex)index).Entry];

			var mi = (MatrixIndex)index;
			return this[mi.Row, mi.Column];
		}

		public void Set(IIsIndex index, Value value)
		{
			if (!(value is ScalarValue))
				throw new OperationNotSupportedException("Index", value);

			var scalar = value as ScalarValue;

			if (index is VectorIndex)
			{
				this[((VectorIndex)index).Entry] = scalar;
				return;
			}
			
			var mi = (MatrixIndex)index;
			this[mi.Row, mi.Column] = scalar;
		}

		#endregion
	}
}

