using System;

namespace YAMP
{
	public class ScalarValue : Value
	{
		const double epsilon = 1e-12;
		
		double _real;
		double _imag;
		
		public ScalarValue () : this(0.0)
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

		public int IntValue
		{
			get { return (int)_real; } 
		}
		
		public double Value 
		{
			get { return _real; }
			set { _real = value; }
		}
		
		public double ImaginaryValue 
		{
			get { return _imag; }
			set { _imag = value; }
		}
		
		public ScalarValue Clone()
		{
			return new ScalarValue(this);
		}
		
		public static ScalarValue I
		{
			get { return new ScalarValue(0.0, 1.0); }
		}
		
		public ScalarValue Abs()
		{
			return new ScalarValue(abs());
		}

        public ScalarValue AbsSquare()
        {
            return new ScalarValue(_real * _real + _imag * _imag);
        }
		
		public ScalarValue Conjugate()
		{
			return new ScalarValue(_real, -_imag);
		}
		
		public override Value Add (Value right)
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
		
		public override Value Subtract (Value right)
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
		
		public override Value Multiply (Value right)
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
		
		public override Value Divide (Value right)
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
		
		public override Value Power (Value exponent)
		{
            if(exponent is ScalarValue)
			{
                if (Value == 0.0 && ImaginaryValue == 0.0)
                    return new ScalarValue();

                var exp = exponent as ScalarValue;
                var theta = _real == 0.0 ? Math.PI / 2 * Math.Sign(ImaginaryValue) : Math.Atan(_imag / _real);
                var L = _real / Math.Cos(theta);
                var phi = Ln();
                phi._real *= exp._imag;
                phi._imag *= exp._imag;
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
		
		public override byte[] Serialize ()
		{
			var re = BitConverter.GetBytes(_real);
			var im = BitConverter.GetBytes(_imag);
			var ov = new byte[re.Length + im.Length];
			re.CopyTo(ov, 0);
			im.CopyTo(ov, re.Length);
			return ov;
		}

		public override Value Deserialize (byte[] content)
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
		
		public ScalarValue Faculty()
		{
            var re = faculty(_real);
            var im = faculty(_imag);

            if (_imag == 0.0)
                im = 0.0;
            else if (_real == 0.0 && _imag != 0.0)
                re = 0.0;

			return new ScalarValue(re, im);
		}
		
		double faculty(double r)
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
		
		public override string ToString ()
		{
			//TODO
			if(Math.Abs(_imag) < epsilon)
				return string.Format(Tokens.NumberFormat, "{0}", Math.Abs (Value) < epsilon ? 0.0 : Math.Round(Value, 4));
			else if(Math.Abs(_real) < epsilon)
				return string.Format(Tokens.NumberFormat, "{0}i", Math.Round(ImaginaryValue, 4));
			
			return string.Format (Tokens.NumberFormat, "{0}{2}{1}i", Math.Round(Value, 4), Math.Round(ImaginaryValue, 4), ImaginaryValue < 0.0 ? string.Empty : "+");
		}
		
		public override bool Equals (object obj)
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
		
		public override int GetHashCode ()
		{
			return (_real + _imag).GetHashCode();
        }

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

		public static ScalarValue operator <(ScalarValue l, ScalarValue r)
		{
			if(l.ImaginaryValue == 0.0 && r.ImaginaryValue == 0)
			{
				if(l._real < r._real)
					return new ScalarValue(1.0, 0.0);
			}
			else if(l.abs() < r.abs ())
				return new ScalarValue(1.0, 0.0);

			return new ScalarValue();
		}
		
		public static ScalarValue operator >(ScalarValue l, ScalarValue r)
		{
			if(l.ImaginaryValue == 0.0 && r.ImaginaryValue == 0)
			{
				if(l._real > r._real)
					return new ScalarValue(1.0, 0.0);
			}
			else if(l.abs() > r.abs ())
				return new ScalarValue(1.0, 0.0);
			
			return new ScalarValue();
		}
		
		public static ScalarValue operator ==(ScalarValue l, ScalarValue r)
		{
			if(Math.Abs (l.ImaginaryValue - r.ImaginaryValue) > epsilon)
				return new ScalarValue();

			if(Math.Abs(l.Value - r.Value) > epsilon)
				return new ScalarValue();
			
			return new ScalarValue(1.0);
		}
		
		public static ScalarValue operator !=(ScalarValue l, ScalarValue r)
		{
			if(Math.Abs (l.ImaginaryValue - r.ImaginaryValue) > epsilon)
				return new ScalarValue(1.0);
			
			if(Math.Abs(l.Value - r.Value) > epsilon)
				return new ScalarValue(1.0);
			
			return new ScalarValue();
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
    }
}

