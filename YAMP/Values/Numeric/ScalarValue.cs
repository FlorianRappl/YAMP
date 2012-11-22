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

		public ScalarValue Square()
		{
			return this * this;
		}

        public ScalarValue Clone()
		{
			return new ScalarValue(this);
		}
		
		public override ScalarValue Abs()
		{
			return new ScalarValue(abs());
		}

        public override ScalarValue AbsSquare()
        {
            return new ScalarValue(_real * _real + _imag * _imag);
        }
		
		public ScalarValue Conjugate()
		{
			return new ScalarValue(_real, -_imag);
		}
		
		public override Value Add(Value right)
		{
			if(right is ScalarValue)
			{
				var r = right as ScalarValue;
                var re = _real + r._real;
                var im = _imag + r._imag;
                return new ScalarValue(re, im);
            }
            else if (right is MatrixValue)
            {
                var r = right as MatrixValue;
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
				var r = right as ScalarValue;
                var re = _real - r._real;
                var im = _imag - r._imag;
                return new ScalarValue(re, im);
            }
            else if (right is MatrixValue)
            {
				var r = right as MatrixValue;
				var m = new MatrixValue(r.DimensionY, r.DimensionX);
				
				for(var j = 1; j <= r.DimensionY; j++)
					for(var i = 1; i <= r.DimensionX; i++)
						m[j, i] = this.Subtract(r[j, i]) as ScalarValue;
				
				return m;
            }
			
			throw new OperationNotSupportedException("-", right);
		}
		
		public override Value Multiply(Value right)
		{
			if(right is ScalarValue)
			{
				var r = right as ScalarValue;
				var re = _real * r._real - _imag * r._imag;
				var im = _real * r._imag + _imag * r._real;
                return new ScalarValue(re, im);
			}
            else if (right is MatrixValue)
            {
                var r = right as MatrixValue;
                return r.Multiply(this);
            }
			
			throw new OperationNotSupportedException("*", right);
		}
		
		public override Value Divide(Value right)
		{
			if(right is ScalarValue)
			{
				var r = (right as ScalarValue).Conjugate();
                var q = r._real * r._real + r._imag * r._imag;
                var re = (_real * r._real - _imag * r._imag) / q;
                var im = (_real * r._imag + _imag * r._real) / q;
				return new ScalarValue(re, im);
			}
			else if(right is MatrixValue)
			{
				var inv = (right as MatrixValue).Inverse();
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

                var exp = exponent as ScalarValue;
                var theta = _real == 0.0 ? Math.PI / 2 * Math.Sign(ImaginaryValue) : Math.Atan2(_imag, _real);
                var L = _real / Math.Cos(theta);
                var phi = Ln() * exp._imag;
                var R = (I.Multiply(phi) as ScalarValue).Exp();
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
				var b = exponent as MatrixValue;
				var m = new MatrixValue(b.DimensionY, b.DimensionX);

				for(var i = 1; i <= b.DimensionX; i++)
					for(var j = 1; j <= b.DimensionY; j++)
						m[j, i] = Power(b[j, i]) as ScalarValue;

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
			var im = arg();
			return new ScalarValue(re, im);
		}
		
		public ScalarValue Log()
		{
			var re = Math.Log(abs(), 10.0);
			var im = arg();
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

                if ((k & 1) == 0)
                    return new ScalarValue(false);

                var sqrt = (int)Math.Sqrt(Value);

				for (var i = 3; i <= sqrt; i += 2)
				{
					if (k % i == 0)
						return new ScalarValue(false);
				}

                return new ScalarValue(true);
            }

			return new ScalarValue(false);
		}
		
		public ScalarValue Factorial()
		{
            var re = factorial(_real);
            var im = factorial(_imag);

            if (_imag == 0.0)
                im = 0.0;
            else if (_real == 0.0 && _imag != 0.0)
                re = 0.0;

			return new ScalarValue(re, im);
		}
		
		double factorial(double r)
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

        double arg()
        {
            return Math.Atan2(_imag, _real);
        }

        public override string ToString(ParseContext context)
        {
            if (Math.Abs(_imag) < epsilon)
                return string.Format(context.NumberFormat, "{0}", Math.Abs(Value) < epsilon ? 0.0 : Math.Round(Value, context.Precision));
            else if (Math.Abs(_real) < epsilon)
                return string.Format(context.NumberFormat, "{0}i", Math.Round(ImaginaryValue, context.Precision));

            return string.Format(context.NumberFormat, "{0}{2}{1}i", Math.Round(Value, context.Precision), Math.Round(ImaginaryValue, context.Precision), ImaginaryValue < 0.0 ? string.Empty : "+");
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

