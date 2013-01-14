/*
	Copyright (c) 2012-2013, Florian Rappl.
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
    /// <summary>
    /// The class for representing a matrix value.
    /// </summary>
	public class MatrixValue : NumericValue, IFunction, ISetFunction
	{
		#region Members

		IDictionary<MatrixIndex, ScalarValue> _values;
		
		int dimX;
		int dimY;

		#endregion

        #region Properties

        /// <summary>
        /// Gets the maximum length of a cell in the default string representation.
        /// </summary>
        public int MaximumLength
        {
            get
            {
                var max = 0;

                foreach (var el in _values.Values)
                {
                    var length = el.Length;

                    if (length > max)
                        max = length;
                }

                return max;
            }
        }

        /// <summary>
        /// Gets a boolean if the matrix is only 1x1.
        /// </summary>
        public bool IsScalar
        {
            get { return DimensionX == 1 && DimensionY == 1; }
        }

        /// <summary>
        /// Gets a boolean if the matrix is only a row (rows = 1) or column (columns = 1) vector.
        /// </summary>
        public bool IsVector
        {
            get { return (DimensionX == 1 && DimensionY > 1) || (DimensionY == 1 && DimensionX > 1); }
        }

        /// <summary>
        /// Gets the number of columns.
        /// </summary>
		public int DimensionX
		{
			get { return dimX;  }
			protected set { dimX = value; }
		}
		
        /// <summary>
        /// Gets the number of rows.
        /// </summary>
		public int DimensionY
		{
			get { return dimY; }
			protected set { dimY = value; }
		}

		/// <summary>
		/// Gets the number of columns (alias for DimensionX).
		/// </summary>
		public int Columns
		{
			get { return dimX; }
			protected set { dimX = value; }
		}

		/// <summary>
		/// Gets the number of rows (alias for DimensionY).
		/// </summary>
		public int Rows
		{
			get { return dimY; }
			protected set { dimY = value; }
		}
		
        /// <summary>
        /// Gets the length of the matrix, i.e. rows * columns.
        /// </summary>
		public int Length
		{
			get { return DimensionX * DimensionY; }
		}

        /// <summary>
        /// Gets a value if the matrix is symmetric, i.e. M_ij = M_ji
        /// </summary>
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

        /// <summary>
        /// Gets a boolean if the matrix has any complex (im != 0.0) entries.
        /// </summary>
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

        /// <summary>
        /// Gets the maximum exponent used by values within the matrix.
        /// </summary>
        public int Exponent
        {
            get
            {
                if (_values.Count == 0)
                    return 0;

                var exp = int.MinValue;

                foreach (var value in _values.Values)
                    exp = Math.Max(value.Exponent, exp);

                return exp;
            }
        }

		#endregion

		#region ctors

        /// <summary>
        /// Constructs a new matrix.
        /// </summary>
		public MatrixValue()
		{
			_values = new Dictionary<MatrixIndex, ScalarValue>();
		}
		
        /// <summary>
        /// Constructs a new matrix with the given dimension.
        /// </summary>
        /// <param name="rows">The number of rows.</param>
        /// <param name="cols">The number of columns.</param>
		public MatrixValue(int rows, int cols) : this()
		{
			dimX = cols;
			dimY = rows;
		}

        /// <summary>
        /// Constructs a new matrix based on the jagged double array.
        /// </summary>
        /// <param name="values">The values to use.</param>
        /// <param name="rows">The number of rows in the new matrix.</param>
        /// <param name="cols">The number of columns in the matrix.</param>
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

        /// <summary>
        /// Constructs a new matrix based on the given two dimensional array.
        /// </summary>
        /// <param name="values">The values which set the dimensions and starting values of the matrix.</param>
		public MatrixValue(double[,] values) : this(values.GetLength(0), values.GetLength(1))
		{
			for (var j = 0; j < dimY; j++)
			{
				for (var i = 0; i < dimX; i++)
				{
					if (values[j, i] == 0.0)
						continue;

					this[j + 1, i + 1] = new ScalarValue(values[j, i]);
				}
			}
		}

        /// <summary>
        /// Constructs a new (column) vector based on the given double array.
        /// </summary>
        /// <param name="vector"></param>
		public MatrixValue(double[] vector) : this(vector.Length, 1)
		{
			for (var j = 0; j < vector.Length; j++)
				this[j + 1] = new ScalarValue(vector[j]);
		}

        /// <summary>
        /// Constructs a new matrix with the given dimension and sets each entry to the given value.
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="cols"></param>
        /// <param name="filling"></param>
        public MatrixValue(int rows, int cols, ScalarValue filling) : this(rows, cols)
        {
            for (var i = 1; i <= dimX; i++)
                for (var j = 1; j <= dimY; j++)
                    this[j, i] = filling.Clone();
        }

		#endregion

		#region Statics

        /// <summary>
        /// Creates a new matrix with the help of a specified value.
        /// </summary>
        /// <param name="value">The value to initialize the matrix with.</param>
        /// <returns>A matrix containing the given value.</returns>
		public static MatrixValue Create(Value value)
		{
			if(value is MatrixValue)
                return (MatrixValue)value;
			else if(value is ScalarValue)
			{
				var m = new MatrixValue();
				m[1, 1] = (ScalarValue)value;
				return m;
			}
			
			throw new YAMPNonNumericException();
		}

        /// <summary>
        /// Creates a new identity matrix of the given dimension.
        /// </summary>
        /// <param name="dimension">The rank of the identity matrix.</param>
        /// <returns>A new identity matrix.</returns>
		public static MatrixValue One(int dimension)
		{
			var m = new MatrixValue(dimension, dimension);

			for (var i = 1; i <= dimension; i++)
				m[i, i] = new ScalarValue(1.0);

			return m;
		}

        /// <summary>
        /// Creates a matrix containing only ones.
        /// </summary>
        /// <param name="rows">The number of rows in the new matrix.</param>
        /// <param name="cols">The number of columns in the new matrix.</param>
        /// <returns>A new matrix containing only ones.</returns>
		public static MatrixValue Ones(int rows, int cols)
		{
			var m = new MatrixValue(rows, cols);

			for (var j = 1; j <= rows; j++)
				for (var i = 1; i <= cols; i++)
					m[j, i] = new ScalarValue(1.0);

			return m;
		}

		#endregion

        #region Serialization

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

                foreach (var entry in _values)
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
            dimY = BitConverter.ToInt32(content, 0);
            dimX = BitConverter.ToInt32(content, 4);
            var count = BitConverter.ToInt32(content, 8);
            var pos = 12;

            for (var i = 0; i < count; i++)
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

        #endregion

        #region Geometry

        public override void Clear()
        {
            for (var i = 1; i <= DimensionX; i++)
                for (var j = 1; j <= DimensionY; j++)
                    this[j, i].Clear();
        }

        public MatrixValue AddColumn(Value value)
        {
            var that = Clone();

            if (value is MatrixValue)
            {
                var t = value as MatrixValue;
                int j, i = 1, offset = DimensionX + 1;

                for (var k = 1; k <= t.DimensionY; k++)
                {
                    j = offset;

                    for (var l = 1; l <= t.DimensionX; l++)
                        that[i, j++] = t[k, l];

                    i++;
                }

                return that;
            }
            else if (value is ScalarValue)
            {
                var t = value as ScalarValue;
                that[1, DimensionX + 1] = t;
                return that;
            }

            throw new YAMPOperationInvalidException(",", value);
        }

        public MatrixValue AddRow(Value value)
        {
            var that = Clone();

            if (value is MatrixValue)
            {
                var t = value as MatrixValue;
                int j, i = DimensionY + 1;

                for (var k = 1; k <= t.DimensionY; k++)
                {
                    j = 1;

                    for (var l = 1; l <= t.DimensionX; l++)
                        that[i, j++] = t[k, l];

                    i++;
                }

                return that;
            }
            else if (value is ScalarValue)
            {
                var t = value as ScalarValue;
                that[DimensionY + 1, 1] = t;
                return that;
            }

            throw new YAMPOperationInvalidException(";", value);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the maximum length of cell by considering the given context.
        /// </summary>
        /// <param name="context">The parse context which holds the state information.</param>
        /// <returns>The length of the string in chars.</returns>
        public int GetMaximumLength(ParseContext context)
        {
            var max = 0;

            foreach (var el in _values.Values)
            {
                var length = el.GetLength(context);

                if (length > max)
                    max = length;
            }

            return max;
        }

        /// <summary>
        /// Sorts the values of the matrix and outputs the values in a
        /// new vector.
        /// </summary>
        /// <returns>The matrix instance holding the sorted values as a vector.</returns>
		public MatrixValue VectorSort()
		{
			var v = new MatrixValue(1, Length);

			for (var k = 1; k <= Length; k++)
				v._values.Add(GetIndex(k), this[k].Clone());

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

        /// <summary>
        /// Creates a deep copy of the matrix.
        /// </summary>
        /// <returns>A clone of the current instance.</returns>
		public virtual MatrixValue Clone()
		{
			var m = new MatrixValue();

			foreach (var entry in _values)
				m._values.Add(entry.Key, entry.Value.Clone());

			m.dimX = dimX;
			m.dimY = dimY;
			return m;
		}

        #endregion

        #region String Representation

        public override string ToString(ParseContext context)
        {
            var sb = new StringBuilder();

            for (var j = 1; j <= DimensionY; j++)
            {
                for (var i = 1; i <= DimensionX; i++)
                {
                    sb.Append(this[j, i].ToString(context));

                    if (i < DimensionX)
                        sb.Append("\t");
                }

                if (j < DimensionY)
                    sb.AppendLine();
            }

            return sb.ToString();
        }

        #endregion

        #region Special Matrix operations

        public MatrixValue Inverse()
        {
            var target = One(DimensionX);

            if (DimensionX < 24)
            {
                var lu = new YAMP.Numerics.LUDecomposition(this);
                return lu.Solve(target);
            }
            else if (IsSymmetric)
            {
                var cho = new YAMP.Numerics.CholeskyDecomposition(this);
                return cho.Solve(target);
            }

            var qr = new YAMP.Numerics.QRDecomposition(this);
            return qr.Solve(target);
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
                    //I guess that's right
					return this[1, 1] * (this[2, 2] * 
                                (this[3, 3] * this[4, 4] - this[3, 4] * this[4, 3]) + this[2, 3] * 
                                    (this[3, 4] * this[4, 2] - this[3, 2] * this[4, 4]) + this[2, 4] * 
                                        (this[3, 2] * this[4, 3] - this[3, 3] * this[4, 2])) -
							this[1, 2] * (this[2, 1] * 
                                (this[3, 3] * this[4, 4] - this[3, 4] * this[4, 3]) + this[2, 3] * 
                                    (this[3, 4] * this[4, 1] - this[3, 1] * this[4, 4]) + this[2, 4] * 
                                        (this[3, 1] * this[4, 3] - this[3, 3] * this[4, 1])) +
							this[1, 3] * (this[2, 1] * 
                                (this[3, 2] * this[4, 4] - this[3, 4] * this[4, 2]) + this[2, 2] * 
                                    (this[3, 4] * this[4, 1] - this[3, 1] * this[4, 4]) + this[2, 4] * 
                                        (this[3, 1] * this[4, 2] - this[3, 2] * this[4, 1])) -
							this[1, 4] * (this[2, 1] * 
                                (this[3, 2] * this[4, 3] - this[3, 3] * this[4, 2]) + this[2, 2] * 
                                    (this[3, 3] * this[4, 1] - this[3, 1] * this[4, 3]) + this[2, 3] * 
                                        (this[3, 1] * this[4, 2] - this[3, 2] * this[4, 1]));
				}

				var lu = new YAMP.Numerics.LUDecomposition(this);
				return new ScalarValue(lu.Determinant());
			}

			return new ScalarValue();
		}

        #endregion

        #region Comparison

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

        #endregion

        #region Submatrices and extractions

        /// <summary>
        /// Gets a real vector of a submatrix.
        /// </summary>
        /// <param name="yoffset">The offset in rows.</param>
        /// <param name="ylength">The length in rows.</param>
        /// <param name="xoffset">The offset in columns.</param>
        /// <param name="xlength">The length in columns.</param>
        /// <returns>The double array (real vector).</returns>
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

        /// <summary>
        /// Gets a real vector of the complete matrix.
        /// </summary>
        /// <returns>A double array (real vector).</returns>
		public double[] GetRealVector()
		{
			var array = new double[Length];

			for (var i = 1; i <= Length; i++)
				array[i - 1] = this[i].Value;

			return array;
		}

        /// <summary>
        /// Gets a real matrix of the complete matrix.
        /// </summary>
        /// <returns>A jagged 2D array.</returns>
		public double[][] GetRealMatrix()
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

        /// <summary>
        /// Creates a sub matrix of the given instance.
        /// </summary>
        /// <param name="yoffset">Vertical offset in rows.</param>
        /// <param name="yfinal">Final row-index.</param>
        /// <param name="xoffset">Horizontal offset in columns.</param>
        /// <param name="xfinal">Final column-index.</param>
        /// <returns>The new instance with the corresponding entries.</returns>
		public MatrixValue GetSubMatrix(int yoffset, int yfinal, int xoffset, int xfinal)
		{
			var X = new MatrixValue(yfinal - yoffset, xfinal - xoffset);

			for (int j = yoffset + 1; j <= yfinal; j++)
				for (int i = xoffset + 1; i <= xfinal; i++)
					X[j - yoffset, i - xoffset] = this[j, i].Clone();

			return X;
		}

        /// <summary>
        /// Creates a sub matrix of the given instance.
        /// </summary>
        /// <param name="y">Row-indices to consider.</param>
        /// <param name="xoffset">Horizontal offset in columns.</param>
        /// <param name="xfinal">Final column-index.</param>
        /// <returns>The new instance with the corresponding entries.</returns>
		public MatrixValue GetSubMatrix(int[] y, int xoffset, int xfinal)
		{
			var X = new MatrixValue(y.Length, xfinal - xoffset);

			for (int j = 1; j <= y.Length; j++)
				for (int i = xoffset + 1; i <= xfinal; i++)
					X[j, i - xoffset] = this[y[j - 1], i].Clone();

			return X;
		}

        #endregion

        #region Vectors

        /// <summary>
        /// Gets the j-th row vector, i.e. a vector which is spanned over all columns of one row.
        /// </summary>
        /// <param name="j">The index of the row to get the vector from.</param>
        /// <returns>The extracted row vector.</returns>
        public MatrixValue GetRowVector(int j)
        {
            var m = new MatrixValue(1, DimensionX);

            for (var i = 1; i <= DimensionX; i++)
                m[1, i] = this[j, i].Clone();

            return m;
        }

        /// <summary>
        /// Sets the j-th row vector to be of the given matrix.
        /// </summary>
        /// <param name="j">The index of the row to set the vector to.</param>
        /// <param name="m">The matrix with values to set the j-th row to.</param>
        /// <returns>The current instance.</returns>
        public MatrixValue SetRowVector(int j, MatrixValue m)
        {
            for (var i = 1; i <= m.Length; i++)
                this[j, i] = m[i].Clone();

            return this;
        }

        /// <summary>
        /// Gets the i-th column vector, i.e. a vector which is spanned over all rows of one column.
        /// </summary>
        /// <param name="i">The index of the column to get the vector from.</param>
        /// <returns>The extracted column vector.</returns>
        public MatrixValue GetColumnVector(int i)
        {
            var m = new MatrixValue(DimensionY, 1);

            for (var j = 1; j <= DimensionY; j++)
                m[j, 1] = this[j, i].Clone();

            return m;
        }

        /// <summary>
        /// Sets the i-th column vector to be of the given matrix.
        /// </summary>
        /// <param name="j">The index of the column to set the vector to.</param>
        /// <param name="m">The matrix with values to set the i-th column to.</param>
        /// <returns>The current instance.</returns>
        public MatrixValue SetColumnVector(int i, MatrixValue m)
        {
            for (var j = 1; j <= m.Length; j++)
                this[j, i] = m[j].Clone();

            return this;
        }

        #endregion

        #region Other operations

        /// <summary>
        /// Computes the L2-Norm of the matrix (seen as a vector).
        /// </summary>
        /// <returns>Returns a scalar value that is the square root of the absolute squared values.</returns>
        public ScalarValue Abs()
        {
            var sum = 0.0;

            for(var i = 1; i <= DimensionX; i++)
                for(var j = 1; j <= DimensionY; j++)
                    sum += this[j, i].AbsSquare();

            return new ScalarValue(Math.Sqrt(sum));
        }

        /// <summary>
        /// Computes the sum of all entries.
        /// </summary>
        /// <returns>Returns the sum of all entries.</returns>
        public ScalarValue Sum()
        {
            var s = new ScalarValue();

            for (var i = 1; i <= DimensionX; i++)
                for (var j = 1; j <= DimensionY; j++)
                    s += this[j, i];

            return s;
        }

        /// <summary>
        /// Produces a dot product of the given instance
        /// (seen as a vector) with another vector.
        /// </summary>
        /// <param name="w">The second operand of the dot-product.</param>
        /// <returns>The resulting scalar.</returns>
        public ScalarValue Dot(MatrixValue w)
        {
            var length = Math.Min(Length, w.Length);
            var sum = new ScalarValue();

            for (var i = 1; i <= length; i++)
                sum += this[i] * w[i];

            return sum;
        }

        /// <summary>
        /// Produces a complex dot (this operand is c.c.) product of the
        /// given instance (seen as a vector) with another vector.
        /// </summary>
        /// <param name="w">The second operand of the complex dot-product.</param>
        /// <returns>The resulting scalar.</returns>
        public ScalarValue ComplexDot(MatrixValue w)
        {
            var length = Math.Min(Length, w.Length);
            var sum = new ScalarValue();

            for (var i = 1; i <= length; i++)
                sum += this[i].Conjugate() * w[i];

            return sum;
        }

        #endregion

        #region Indexers

        public virtual ScalarValue this[int j, int i]
		{
			get
			{
				if (i > dimX || i < 1)
					throw new YAMPIndexOutOfBoundException(i, 1, dimX);
                else if (j > dimY || j < 1)
                    throw new YAMPIndexOutOfBoundException(j, 1, dimY);

				var index = new MatrixIndex();
				index.Column = i;
				index.Row = j;
				
				if(_values.ContainsKey(index))
					return _values[index];

				return new ScalarValue();
			}
			set
            {
                if (i < 1)
                    throw new YAMPIndexOutOfBoundException(i, 1);
                else if (j < 1)
                    throw new YAMPIndexOutOfBoundException(j, 1);

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
                    throw new YAMPIndexOutOfBoundException(i, 1, Length);

                var index = GetIndex(i);
                return this[index.Row, index.Column];
			}
			set
			{
                if (i < 1)
                    throw new YAMPIndexOutOfBoundException(i, 1);

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
        
		#region Standard Operators

		public static MatrixValue operator *(MatrixValue A, MatrixValue B)
		{
            if (A.DimensionX != B.DimensionY)
                throw new YAMPMatrixMultiplyException(A.DimensionX, B.DimensionY);

            var M = new MatrixValue(A.DimensionY, B.DimensionX);

            for (var j = 1; j <= B.DimensionX; j++)
            {
                for (var i = 1; i <= A.DimensionY; i++)
                {
                    for (var k = 1; k <= A.DimensionX; k++)
                        M[i, j] = M[i, j] + (A[i, k] * B[k, j]);
                }
            }

            return M;
		}

		public static MatrixValue operator *(ScalarValue s, MatrixValue M)
        {
            var A = new MatrixValue(M.DimensionY, M.DimensionX);

            for (var i = 1; i <= M.DimensionX; i++)
                for (var j = 1; j <= M.DimensionY; j++)
                    A[j, i] = M[j, i] * s;

            return A;
		}

		public static MatrixValue operator /(MatrixValue l, MatrixValue r)
		{
            return l * r.Inverse();
		}

        public static MatrixValue operator /(MatrixValue l, ScalarValue r)
        {
            var m = new MatrixValue(l.DimensionY, l.DimensionX);

            for (var j = 1; j <= l.DimensionY; j++)
                for (var i = 1; i <= l.DimensionX; i++)
                    m[j, i] = l[j, i] / r;

            return m;
        }

		public static MatrixValue operator -(MatrixValue l, MatrixValue r)
		{
            if (r.DimensionX != l.DimensionX || r.DimensionY != l.DimensionY)
                throw new YAMPDifferentDimensionsException(l, r);

            var m = new MatrixValue(l.DimensionY, l.DimensionX);

            for (var j = 1; j <= l.DimensionY; j++)
                for (var i = 1; i <= l.DimensionX; i++)
                    m[j, i] = l[j, i] - r[j, i];

            return m;
		}

		public static MatrixValue operator +(MatrixValue l, MatrixValue r)
        {
            if (r.DimensionX != l.DimensionX || r.DimensionY != l.DimensionY)
                throw new YAMPDifferentDimensionsException(l, r);

            var m = new MatrixValue(l.DimensionY, l.DimensionX);

            for (var j = 1; j <= l.DimensionY; j++)
                for (var i = 1; i <= l.DimensionX; i++)
                    m[j, i] = l[j, i] + r[j, i];

            return m;
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

        #region Register Operators

        public override void RegisterElement()
        {
            PlusOperator.Register(typeof(MatrixValue), typeof(MatrixValue), AddMM);
            PlusOperator.Register(typeof(MatrixValue), typeof(ScalarValue), AddMS);
            PlusOperator.Register(typeof(ScalarValue), typeof(MatrixValue), AddSM);

            MinusOperator.Register(typeof(MatrixValue), typeof(MatrixValue), SubtractMM);
            MinusOperator.Register(typeof(MatrixValue), typeof(ScalarValue), SubtractMS);
            MinusOperator.Register(typeof(ScalarValue), typeof(MatrixValue), SubtractSM);

            MultiplyOperator.Register(typeof(MatrixValue), typeof(MatrixValue), MultiplyMM);
            MultiplyOperator.Register(typeof(ScalarValue), typeof(MatrixValue), MultiplySM);
            MultiplyOperator.Register(typeof(MatrixValue), typeof(ScalarValue), MultiplyMS);

            RightDivideOperator.Register(typeof(MatrixValue), typeof(MatrixValue), DivideMM);
            RightDivideOperator.Register(typeof(ScalarValue), typeof(MatrixValue), DivideSM);
            RightDivideOperator.Register(typeof(MatrixValue), typeof(ScalarValue), DivideMS);

            PowerOperator.Register(typeof(MatrixValue), typeof(ScalarValue), PowMS);
            PowerOperator.Register(typeof(ScalarValue), typeof(MatrixValue), PowSM);

            ModuloOperator.Register(typeof(ScalarValue), typeof(MatrixValue), ModuloSM);
            ModuloOperator.Register(typeof(MatrixValue), typeof(ScalarValue), ModuloMS);
        }

        public static MatrixValue AddMM(Value left, Value right)
        {
            var l = (MatrixValue)left;
            var r = (MatrixValue)right;
            return l + r;
        }

        public static MatrixValue AddSM(Value left, Value right)
        {
            var s = (ScalarValue)left;
            var m = (MatrixValue)right;
            var M = new MatrixValue(m.DimensionY, m.DimensionX);

            for (var j = 1; j <= m.DimensionY; j++)
                for (var i = 1; i <= m.DimensionX; i++)
                    M[j, i] = m[j, i] + s;

            return M;
        }

        public static MatrixValue AddMS(Value left, Value right)
        {
            return AddSM(right, left);
        }

        public static Value SubtractMM(Value left, Value right)
        {
            var l = (MatrixValue)left;
            var r = (MatrixValue)right;
            return l - r;
        }

        public static MatrixValue SubtractMS(Value left, Value right)
        {
            return SubtractSM(right, left);
        }

        public static MatrixValue SubtractSM(Value left, Value right)
        {
            var s = (ScalarValue)left;
            var r = (MatrixValue)right;
            var m = new MatrixValue(r.DimensionY, r.DimensionX);

            for (var j = 1; j <= r.DimensionY; j++)
                for (var i = 1; i <= r.DimensionX; i++)
                    m[j, i] = s - r[j, i];

            return m;
        }

        public static Value MultiplyMM(Value left, Value right)
        {
            var A = (MatrixValue)left;
            var B = (MatrixValue)right;
            var C = A * B;

            if(1 == C.DimensionX && C.DimensionY == 1)
                return C[1, 1];

            return C;
        }

        public static MatrixValue MultiplyMS(Value left, Value right)
        {
            return MultiplySM(right, left);
        }

        public static MatrixValue MultiplySM(Value left, Value right)
        {
            var l = (ScalarValue)left;
            var r = (MatrixValue)right;
            return l * r;
        }

        public static MatrixValue PowMS(Value basis, Value exponent)
        {
            var l = (MatrixValue)basis;
            var exp = (ScalarValue)exponent;

            if (l.DimensionX != l.DimensionY)
                throw new YAMPMatrixFormatException(SpecialMatrixFormat.Square);

            if (exp.ImaginaryValue != 0.0 || Math.Floor(exp.Value) != exp.Value)
                throw new YAMPOperationInvalidException("^", exponent);

            var eye = MatrixValue.One(l.DimensionX);
            var multiplier = exp.Value < 0 ? l.Inverse() : l;
            var count = (int)Math.Abs(exp.Value);

            for (var i = 0; i < count; i++)
                eye = eye * multiplier;

            return eye;
        }

        public static MatrixValue PowSM(Value basis, Value exponent)
        {
            var l = (ScalarValue)basis;
            var r = (MatrixValue)exponent;
            var m = new MatrixValue(r.DimensionY, r.DimensionX);

            for (var i = 1; i <= r.DimensionX; i++)
                for (var j = 1; j <= r.DimensionY; j++)
                    m[j, i] = l.Pow(r[j, i]);

            return m;
        }

        public static MatrixValue DivideMS(Value left, Value right)
        {
            var l = (MatrixValue)left;
            var r = (ScalarValue)right;
            return l / r;
        }

        public static MatrixValue DivideSM(Value left, Value right)
        {
            var l = (ScalarValue)left;
            var r = (MatrixValue)right;
            return l * r.Inverse();
        }

        public static MatrixValue DivideMM(Value left, Value right)
        {
            var L = (MatrixValue)left;
            var Q = (MatrixValue)right;
            return L / Q;
        }

        public static MatrixValue ModuloMS(Value left, Value right)
        {
            var l = (MatrixValue)left;
            var r = (ScalarValue)right;
            return ModFunction.Mod(l, r);
        }

        public static MatrixValue ModuloSM(Value left, Value right)
        {
            var l = (ScalarValue)left;
            var r = (MatrixValue)right;
            return ModFunction.Mod(l, r);
        }

        #endregion

        #region Behavior as method

        public Value Perform(ParseContext context, Value argument, Value values)
        {
            if (!(values is NumericValue))
                throw new YAMPOperationInvalidException("Matrix", values);

            var indices = new List<MatrixIndex>();

            if (argument is ArgumentsValue)
            {
                var ags = (ArgumentsValue)argument;

                if (ags.Length == 1)
                    return Perform(context, ags[1], values);
                else if (ags.Length > 2)
                    throw new YAMPArgumentNumberException("Matrix", ags.Length, 2);

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
                throw new YAMPOperationInvalidException("Matrix", argument);

            if (values is MatrixValue)
            {
                var index = 1;
                var m = (MatrixValue)values;

                if (m.Length != indices.Count)
                    throw new YAMPDifferentLengthsException(m.Length, indices.Count);

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
                else if (ags.Length > 2)
                    throw new YAMPArgumentNumberException("Matrix", ags.Length, 2);

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

            throw new YAMPOperationInvalidException("Matrix", argument);
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