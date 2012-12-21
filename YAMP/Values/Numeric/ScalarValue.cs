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
using System.Collections.Generic;
using System.Text;

namespace YAMP
{
	public class ScalarValue : NumericValue
    {
        #region Constants

        const double epsilon = 1e-12;

        #endregion

        #region Members

        double _real;
		double _imag;

        #endregion

        #region ctors

        public ScalarValue () : this(0.0, 0.0)
		{
		}

		public ScalarValue(bool boolean) : this(boolean ? 1.0 : 0.0, 0.0)
		{
		}
		
		public ScalarValue(double real) : this(real, 0.0)
		{
		}
		
		public ScalarValue(ScalarValue value) : this(value._real, value._imag)
		{
		}
		
		public ScalarValue(double real, double imag)
		{
			_real = real;
			_imag = imag;
		}

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the real part of the scalar.
        /// </summary>
        public double Re
        {
            get { return _real; }
            set { _real = value; }
        }

        /// <summary>
        /// Gets or sets the imaginary part of the scalar.
        /// </summary>
        public double Im
        {
            get { return _imag; }
            set { _imag = value; }
        }

        /// <summary>
        /// Gets the maximum exponent of the current scalar.
        /// </summary>
        public int Exponent
        {
            get
            {
                if (_imag == 0.0)
                    return GetExponent(_real);
                else if (_real == 0.0)
                    return GetExponent(_imag);

                return Math.Max(GetExponent(_real), GetExponent(_imag));
            }
        }

        /// <summary>
        /// Gets the length of the output in the default representation.
        /// </summary>
		public int Length
		{
			get
			{
				return ToString().Length;
			}
		}

        /// <summary>
        /// Gets the length of the output in the given parse context.
        /// </summary>
        /// <param name="context">The query context to consider.</param>
        /// <returns>The length of the string representation</returns>
        public int GetLength(ParseContext context)
        {
            return ToString(context).Length;
        }

        /// <summary>
        /// Gets the integer part of the real contribution.
        /// </summary>
        public int IntValue
        {
            get { return (int)_real; }
        }

        /// <summary>
        /// Gets the integer part of the imaginary contribution.
        /// </summary>
        public int ImaginaryIntValue
        {
            get { return (int)_imag; }
        }
		
        /// <summary>
        /// Gets or sets the real part of the scalar.
        /// </summary>
		public double Value 
		{
			get { return _real; }
			set { _real = value; }
		}
		
        /// <summary>
        /// Gets or sets the imaginary part of the scalar.
        /// </summary>
		public double ImaginaryValue 
		{
			get { return _imag; }
			set { _imag = value; }
		}

        /// <summary>
        /// Gets a boolean if the number is real (imaginary part zero).
        /// </summary>
        public bool IsReal
        {
            get { return Math.Abs(_imag) < epsilon; }
        }

        /// <summary>
        /// Gets a boolean if the number if complex (imaginary part not zero).
        /// </summary>
        public bool IsComplex
        {
            get { return !IsReal; }
        }

        /// <summary>
        /// Gets a boolean if the number is exactly zero (real and imaginary part).
        /// </summary>
        public bool IsZero
        {
            get
            {
                return _real == 0.0 && _imag == 0.0;
            }
        }

        #endregion

        #region Statics

        static readonly ScalarValue _I = new ScalarValue(0.0, 1.0);

        public static ScalarValue I
        {
            get { return _I; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Computes Atan2(imaginary, real), i.e. the angle in the complex (gaussian) plane.
        /// </summary>
        /// <returns>The angle of the y-x ratio.</returns>
        public double Arg()
        {
            return Math.Atan2(_imag, _real);
        }

        /// <summary>
        /// Computes z * z = z^2.
        /// </summary>
        /// <returns>The square of the current instance.</returns>
		public ScalarValue Square()
		{
			return this * this;
		}

        /// <summary>
        /// Copies the current instance.
        /// </summary>
        /// <returns>A deep copy of the current scalar.</returns>
        public virtual ScalarValue Clone()
		{
			return new ScalarValue(this);
		}
		
        /// <summary>
        /// Computes the absolute value of the current scalar.
        /// </summary>
        /// <returns>The absolute value, which is a real scalar.</returns>
		public override ScalarValue Abs()
		{
			return new ScalarValue(abs());
		}

        /// <summary>
        /// Computes the absolute value squared of the current scalar.
        /// </summary>
        /// <returns>The squared absolute value, which is a real scalar.</returns>
        public override ScalarValue AbsSquare()
        {
            return new ScalarValue(_real * _real + _imag * _imag);
        }
		
        /// <summary>
        /// Conjugates the current scalar value, i.e. switches the sign of the imaginary value.
        /// </summary>
        /// <returns>The conjugated scalar value.</returns>
		public ScalarValue Conjugate()
		{
			return new ScalarValue(_real, -_imag);
		}
		
		public override Value Add(Value right)
		{
			if(right is ScalarValue)
			{
				var r = (ScalarValue)right;
                var re = _real + r._real;
                var im = _imag + r._imag;
                return new ScalarValue(re, im);
            }
            else if (right is MatrixValue)
            {
				var r = (MatrixValue)right;
                return r.Add(this);
            }
			else if(right is StringValue)
			{
				var t = new StringValue(this.ToString());
				return t.Add(right);
			}
			
			throw new OperationNotSupportedException("+", right);
		}
		
		public override Value Subtract(Value right)
		{
			if(right is ScalarValue)
			{
				var r = (ScalarValue)right;
                var re = _real - r._real;
                var im = _imag - r._imag;
                return new ScalarValue(re, im);
            }
            else if (right is MatrixValue)
            {
				var r = (MatrixValue)right;
				var m = new MatrixValue(r.DimensionY, r.DimensionX);
				
				for(var j = 1; j <= r.DimensionY; j++)
					for(var i = 1; i <= r.DimensionX; i++)
						m[j, i] = (ScalarValue)Subtract(r[j, i]);
				
				return m;
            }
			
			throw new OperationNotSupportedException("-", right);
		}
		
		public override Value Multiply(Value right)
		{
			if(right is ScalarValue)
			{
				var r = (ScalarValue)right;

				if (IsZero)
					return new ScalarValue();
				else if (_real == 0.0)
					return new ScalarValue(-_imag * r._imag, r._real * _imag);
				else if (_imag == 0.0)
					return new ScalarValue(_real * r._real, _real * r._imag);

				var re = _real * r._real - _imag * r._imag;
				var im = _real * r._imag + _imag * r._real;
                return new ScalarValue(re, im);
			}
            else if (right is MatrixValue)
            {
				var r = (MatrixValue)right;
                return r.Multiply(this);
            }
			
			throw new OperationNotSupportedException("*", right);
		}
		
		public override Value Divide(Value right)
		{
			if(right is ScalarValue)
			{
				var r = (ScalarValue)right;

				if (r.IsZero)
				{
					if (IsZero)
						return new ScalarValue(1.0);

					return new ScalarValue(_real != 0.0 ? _real / 0.0 : 0.0, _imag != 0.0 ? _imag / 0.0 : 0.0);
				}

				r = r.Conjugate();
                var q = r._real * r._real + r._imag * r._imag;
                var re = (_real * r._real - _imag * r._imag) / q;
                var im = (_real * r._imag + _imag * r._real) / q;
				return new ScalarValue(re, im);
			}
			else if(right is MatrixValue)
			{
				var inv = ((MatrixValue)right).Inverse();
				return this.Multiply(inv);
			}
			
			throw new OperationNotSupportedException("/", right);
		}
		
		public override Value Power(Value exponent)
		{
            if(exponent is ScalarValue)
			{
                if (Value == 0.0 && ImaginaryValue == 0.0)
                    return new ScalarValue();

				var exp = (ScalarValue)exponent;
                var theta = _real == 0.0 ? Math.PI / 2 * Math.Sign(ImaginaryValue) : Math.Atan2(_imag, _real);
                var L = _real / Math.Cos(theta);
                var phi = Ln() * exp._imag;
				var R = ((ScalarValue)I.Multiply(phi)).Exp();
                var alpha = _real == 0.0 ? 1.0 : Math.Pow(Math.Abs(L), exp._real);
                var beta = theta * exp._real;
                var _cos = Math.Cos(beta);
                var _sin = Math.Sin(beta);
                var re = alpha * (_cos * R._real - _sin * R._imag);
                var im = alpha * (_cos * R._imag + _sin * R._real);

                if (L < 0)
                    return new ScalarValue(-im, re);
                
                return new ScalarValue(re, im);
			}
			else if(exponent is MatrixValue)
			{
				var b = (MatrixValue)exponent;
				var m = new MatrixValue(b.DimensionY, b.DimensionX);

				for(var i = 1; i <= b.DimensionX; i++)
					for(var j = 1; j <= b.DimensionY; j++)
						m[j, i] = (ScalarValue)Power(b[j, i]);

				return m;
			}
			
			throw new OperationNotSupportedException("^", exponent);
		}
		
		public override byte[] Serialize()
		{
			var re = BitConverter.GetBytes(_real);
			var im = BitConverter.GetBytes(_imag);
			var ov = new byte[re.Length + im.Length];
			re.CopyTo(ov, 0);
			im.CopyTo(ov, re.Length);
			return ov;
		}

		public override Value Deserialize(byte[] content)
		{
			_real = BitConverter.ToDouble(content, 0);
			_imag = BitConverter.ToDouble(content, 8);
			return this;
		}

        public ScalarValue Sqrt()
        {
            return Power(new ScalarValue(0.5)) as ScalarValue;
        }
		
		public ScalarValue Cos()
		{
			var re = Math.Cos(_real) * Math.Cosh(_imag);
			var im = -Math.Sin(_real) * Math.Sinh(_imag);
			return new ScalarValue(re, im);
		}
		
		public ScalarValue Sin()
		{
			var re = Math.Sin(_real) * Math.Cosh(_imag);
			var im = Math.Cos(_real) * Math.Sinh(_imag);
			return new ScalarValue(re, im);
		}
		
		public ScalarValue Exp()
		{
			var f = Math.Exp(_real);
			var re = f * Math.Cos(_imag);
			var im = f * Math.Sin(_imag);
			return new ScalarValue(re, im);
		}
		
		public ScalarValue Ln()
		{
			var re = Math.Log(abs());
			var im = Arg();
			return new ScalarValue(re, im);
		}
		
		public ScalarValue Log()
		{
			var re = Math.Log(abs(), 10.0);
			var im = Arg();
			return new ScalarValue(re, im);
		}

		public ScalarValue IsInt()
		{
			return new ScalarValue((double)IntValue == Value && ImaginaryValue == 0.0);
		}

		public ScalarValue IsPrime()
		{
            if (IsInt().Value == 1)
            {
                var k = IntValue;

				if(k > 0)
					return new ScalarValue(IsPrime(k));
            }

			return new ScalarValue(false);
		}

		static bool IsPrime(int n)
		{
			if (n < 8)
				return ((n == 2) || (n == 3) || (n == 5) || (n == 7));
			else
			{
				if (n % 2 == 0) 
					return (false);

				int m = n - 1;
				int d = m;
				int s = 0;

				while (d % 2 == 0)
				{
					s++;
					d = d / 2;
				}

				if (n < 1373653)
					return (IsProbablyPrime(n, m, s, d, 2) && IsProbablyPrime(n, m, s, d, 3));
				
				return (IsProbablyPrime(n, m, s, d, 2) && IsProbablyPrime(n, m, s, d, 7) && IsProbablyPrime(n, m, s, d, 61));

			}
		}

		static bool IsProbablyPrime(int n, int m, int s, int d, int w)
		{
			int x = PowMod(w, d, n);

			if ((x == 1) || (x == m))
				return true;

			for (int i = 0; i < s; i++)
			{
				x = PowMod(x, 2, n);
				if (x == 1) 
					return false;
				if (x == m) 
				
					return true;
			}
			return false;
		}

		static int PowMod(int b, int e, int m)
		{
			if (b < 0) 
				throw new ArgumentOutOfRangeException("b");

			if (e < 1) 
				throw new ArgumentOutOfRangeException("e");

			if (m < 1) 
				throw new ArgumentOutOfRangeException("m");

			long bb = Convert.ToInt64(b);
			long mm = Convert.ToInt64(m);
			long rr = 1;

			while (e > 0)
			{
				if ((e & 1) == 1)
					rr = checked((rr * bb) % mm);

				e = e >> 1;
				bb = checked((bb * bb) % mm);
			}

			return Convert.ToInt32(rr);
		}
		
		public ScalarValue Factorial()
		{
            var re = Factorial(_real);
            var im = Factorial(_imag);

            if (_imag == 0.0)
                im = 0.0;
            else if (_real == 0.0 && _imag != 0.0)
                re = 0.0;

			return new ScalarValue(re, im);
		}
		
		double Factorial(double r)
		{
			var k = (int)Math.Abs(r);
			var value = r < 0 ? -1.0 : 1.0;
			
			while(k > 1)
			{
				value *= k;
				k--;
			}
			
			return value;
        }

        double abs()
        {
            return Math.Sqrt(_real * _real + _imag * _imag);
        }

        /// <summary>
        /// Use this string representation if you have a global exponent like
        /// everything is already e5 or similar. In this case the values get
        /// multiplied with e-5 for the output.
        /// </summary>
        /// <param name="context">The parse context to use.</param>
        /// <param name="exponent">The global exponent that is in use.</param>
        /// <returns>The string representation for this global exponent.</returns>
        public string ToString(ParseContext context, int exponent)
        {
            var f = Math.Pow(10, -exponent);

            var re = Format(context, _real * f);
            var im = Format(context, _imag * f) + "i";

            if (ImaginaryValue == 0.0)
                return re;
            else if (Value == 0.0)
                return im;

            return string.Format("{0}{2}{1}", re, im, ImaginaryValue < 0.0 ? string.Empty : "+");
        }

        public override string ToString(ParseContext context)
        {
            return ToString(context, 0);
        }
		
		public override bool Equals(object obj)
		{
			if(obj is ScalarValue)
			{
				var sv = obj as ScalarValue;
				return sv._real == _real && sv._imag == _imag;
			}
			
			if(obj is double && _imag == 0.0)
				return (double)obj == _real;
			
			return false;
		}
		
		public override int GetHashCode()
		{
			return (_real + _imag).GetHashCode();
        }

        public override void Clear()
        {
            this._real = 0.0;
            this._imag = 0.0;
        }

        #endregion

        #region Operators

        public static ScalarValue operator +(ScalarValue a, ScalarValue b)
        {
            return a.Add(b) as ScalarValue;
        }

        public static ScalarValue operator +(ScalarValue a, double b)
        {
            return new ScalarValue(a._real + b, a._imag);
        }

        public static ScalarValue operator +(double b, ScalarValue a)
        {
            return a + b;
        }

        public static ScalarValue operator -(ScalarValue a, ScalarValue b)
        {
            return a.Subtract(b) as ScalarValue;
        }

        public static ScalarValue operator -(ScalarValue a, double b)
        {
            return new ScalarValue(a._real - b, a._imag);
        }

        public static ScalarValue operator -(double b, ScalarValue a)
        {
            return new ScalarValue(b - a._real, a._imag);
        }

        public static ScalarValue operator *(ScalarValue a, ScalarValue b)
        {
            return a.Multiply(b) as ScalarValue;
        }

        public static ScalarValue operator *(double b, ScalarValue a)
        {
            return a * b;
        }

        public static bool operator <(ScalarValue l, ScalarValue r)
		{
			if(l.ImaginaryValue == 0.0 && r.ImaginaryValue == 0)
			{
				if(l._real < r._real)
					return true;
			}
			else if(l.abs() < r.abs ())
				return true;

			return false;
		}

        public static bool operator >(ScalarValue l, ScalarValue r)
		{
			if(l.ImaginaryValue == 0.0 && r.ImaginaryValue == 0)
			{
				if(l._real > r._real)
					return true;
			}
			else if(l.abs() > r.abs ())
				return true;
			
			return false;
        }

        public static bool operator <=(ScalarValue l, ScalarValue r)
        {
            if (l.ImaginaryValue == 0.0 && r.ImaginaryValue == 0)
            {
                if (l._real <= r._real)
                    return true;
            }
            else if (l.abs() <= r.abs())
                return true;

            return false;
        }

        public static bool operator >=(ScalarValue l, ScalarValue r)
        {
            if (l.ImaginaryValue == 0.0 && r.ImaginaryValue == 0)
            {
                if (l._real >= r._real)
                    return true;
            }
            else if (l.abs() >= r.abs())
                return true;

            return false;
        }

        public static bool operator ==(ScalarValue l, ScalarValue r)
        {
            if (ReferenceEquals(l, r))
                return true;

            if ((object)l == null || (object)r == null)
                return false;

            if (Math.Abs(l.ImaginaryValue - r.ImaginaryValue) > epsilon)
                return false;

            if(Math.Abs(l.Value - r.Value) > epsilon)
                return false;

            return true;
        }
		
        public static bool operator !=(ScalarValue l, ScalarValue r)
        {
            return !(l == r);
        }

        public static ScalarValue operator *(ScalarValue a, double b)
        {
            return new ScalarValue(a._real * b, a._imag * b);
        }

        public static ScalarValue operator /(ScalarValue a, ScalarValue b)
        {
            return a.Divide(b) as ScalarValue;
        }

        public static ScalarValue operator /(double b, ScalarValue a)
        {
            return new ScalarValue(b, 0.0) / a;
        }

        public static ScalarValue operator /(ScalarValue a, double b)
        {
            return new ScalarValue(a._real / b, a._imag / b);
        }

        public static ScalarValue operator -(ScalarValue a)
        {
            return new ScalarValue(-a._real, -a._imag);
        }

        #endregion
    }
}

