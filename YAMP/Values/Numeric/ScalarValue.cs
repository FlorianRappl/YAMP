/*
    Copyright (c) 2012-2014, Florian Rappl.
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
using YAMP.Numerics;

namespace YAMP
{
    /// <summary>
    /// A scalar value, which is a complex double type.
    /// </summary>
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

        /// <summary>
        /// Creates a new empty scalar value.
        /// </summary>
        public ScalarValue()
            : this(0.0, 0.0)
        {
        }

        /// <summary>
        /// Creates a new scalar value from a boolean, i.e. 1.0 or 0.0.
        /// </summary>
        /// <param name="boolean">True for 1.0, False for 0.0.</param>
        public ScalarValue(bool boolean)
            : this(boolean ? 1.0 : 0.0, 0.0)
        {
        }

        /// <summary>
        /// Creates a new scalar value which is real.
        /// </summary>
        /// <param name="real">The real value.</param>
        public ScalarValue(double real)
            : this(real, 0.0)
        {
        }

        /// <summary>
        /// Creates a new scalar value from the given one.
        /// </summary>
        /// <param name="value">Copies the contents of the given value.</param>
        public ScalarValue(ScalarValue value)
            : this(value._real, value._imag)
        {
        }

        /// <summary>
        /// Creates a new scalar value with the given complex parameters.
        /// </summary>
        /// <param name="real">The real part of the complex scalar.</param>
        /// <param name="imag">The imaginary part of the complex scalar.</param>
        public ScalarValue(double real, double imag)
        {
            _real = real;
            _imag = imag;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value if the scalar value is actually on true.
        /// </summary>
        public bool IsTrue
        {
            get { return this == ScalarValue.True; }
        }

        /// <summary>
        /// Gets a value if the scalar value is actually on false.
        /// </summary>
        public bool IsFalse
        {
            get { return this == ScalarValue.False; }
        }

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
        /// Gets a boolean if the number is a real integer.
        /// </summary>
        public bool IsInt
        {
            get
            {
                return Math.Floor(_real) == _real && _imag == 0.0;
            }
        }

        /// <summary>
        /// Gets a boolean if the number is real and a prime number.
        /// </summary>
        public bool IsPrime
        {
            get
            {
                if (IsInt && _real > 0)
                    return IsPrimeNumber(IntValue);

                return false;
            }
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

        #region General methods

        /// <summary>
        /// Gets the integer value or throws an exception if either the imaginary part is not 0 or the real part is not integer.
        /// </summary>
        /// <param name="argumentName">The name of the variable that is requested.</param>
        /// <param name="functionName">The name of the function where this is requested.</param>
        /// <returns>The scalar value represented as an integer.</returns>
        public int GetIntegerOrThrowException(string argumentName, string functionName)
        {
            if (!IsInt)
                throw new YAMPArgumentWrongTypeException("Scalar", "Integer", functionName, argumentName);

            return IntValue;
        }

        #endregion

        #region Statics

        static readonly ScalarValue _I = new ScalarValue(0.0, 1.0);

        static readonly ScalarValue _ONE = new ScalarValue(1.0, 0.0);

        static readonly ScalarValue _ZERO = new ScalarValue(0.0, 0.0);

        static readonly ScalarValue _TRUE = new ScalarValue(true);

        static readonly ScalarValue _FALSE = new ScalarValue(false);

        static readonly ScalarValue _REINFINITY = new ScalarValue(double.PositiveInfinity, 0.0);

        static readonly ScalarValue _IMINFINITY = new ScalarValue(0.0, double.PositiveInfinity);

        /// <summary>
        /// Gets the imaginary unit, which is just i.
        /// Real: 0, Imaginary: 1
        /// </summary>
        public static ScalarValue I
        {
            get { return _I; }
        }

        /// <summary>
        /// Gets the identity, which is just 1.
        /// Real: 1, Imaginary: 0
        /// </summary>
        public static ScalarValue One
        {
            get { return _ONE; }
        }

        /// <summary>
        /// Gets the neutral element of addition, which is zero.
        /// Real: 0, Imaginary: 0
        /// </summary>
        public static ScalarValue Zero
        {
            get { return _ZERO; }
        }

        /// <summary>
        /// Gets the constant for true.
        /// </summary>
        public static ScalarValue True
        {
            get { return _TRUE; }
        }

        /// <summary>
        /// Gets the constant for false.
        /// </summary>
        public static ScalarValue False
        {
            get { return _FALSE; }
        }

        /// <summary>
        /// Gets the a value that is positive infinity on the real axis.
        /// Real: pos. infinity, Imaginary: 0
        /// </summary>
        public static ScalarValue RealInfinity
        {
            get { return _REINFINITY; }
        }

        /// <summary>
        /// Gets the a value that is positive infinity on the imgainary axis.
        /// Real: 0, Imaginary: pos. infinity
        /// </summary>
        public static ScalarValue ImaginaryInfinity
        {
            get { return _IMINFINITY; }
        }

        #endregion

        #region Geometry

        /// <summary>
        /// Resets the current value.
        /// </summary>
        public override void Clear()
        {
            this._real = 0.0;
            this._imag = 0.0;
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
        /// Copies the current instance.
        /// </summary>
        /// <returns>A copy of the current scalar.</returns>
        public override Value Copy()
        {
            return Clone();
        }

        #endregion

        #region Mathematics

        /// <summary>
        /// Computes Atan2(imaginary, real), i.e. the angle in the complex (gaussian) plane.
        /// </summary>
        /// <returns>The angle of the y-x ratio.</returns>
        public double Arg()
        {
            return Math.Atan2(_imag, _real);
        }

        /// <summary>
        /// Computes the absolute value of the current scalar.
        /// </summary>
        /// <returns>The absolute value, which is a real scalar.</returns>
        public double Abs()
        {
            return Math.Sqrt(_real * _real + _imag * _imag);
        }

        /// <summary>
        /// Computes the absolute value squared of the current scalar.
        /// </summary>
        /// <returns>The squared absolute value, which is a real scalar.</returns>
        public double AbsSquare()
        {
            return _real * _real + _imag * _imag;
        }

        /// <summary>
        /// Computes z x z = z^2.
        /// </summary>
        /// <returns>The square of the current instance.</returns>
        public ScalarValue Square()
        {
            return this * this;
        }

        /// <summary>
        /// Computes z^* x z = |z|^2, which is AbsSquare().
        /// </summary>
        /// <returns>The absolute square of the current instance.</returns>
        public ScalarValue ComplexSquare()
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

        /// <summary>
        /// Raises the scalar to the specified power.
        /// </summary>
        /// <param name="exponent">The exponent for raising the scalar.</param>
        /// <returns>A new scalar that represents the result of the operation.</returns>
        public ScalarValue Pow(ScalarValue exponent)
        {
            if (Value == 0.0 && ImaginaryValue == 0.0)
                return new ScalarValue();

            var theta = _real == 0.0 ? Math.PI / 2 * Math.Sign(ImaginaryValue) : Math.Atan2(_imag, _real);
            var li = _real / Math.Cos(theta);
            var phi = Ln() * exponent._imag;
            var ri = (I * phi).Exp();
            var alpha = _real == 0.0 ? 1.0 : Math.Pow(Math.Abs(li), exponent._real);
            var beta = theta * exponent._real;
            var cos = Math.Cos(beta);
            var sin = Math.Sin(beta);
            var re = alpha * (cos * ri._real - sin * ri._imag);
            var im = alpha * (cos * ri._imag + sin * ri._real);

            if (li < 0)
                return new ScalarValue(-im, re);

            return new ScalarValue(re, im);
        }

        /// <summary>
        /// Takes the square root of the scalar.
        /// </summary>
        /// <returns>The square root of the value.</returns>
        public ScalarValue Sqrt()
        {
            return Pow(new ScalarValue(0.5));
        }

        /// <summary>
        /// Gives the signum of the scalar.
        /// </summary>
        /// <returns>The sign of the value.</returns>
        public ScalarValue Sign()
        {
            if (this == ScalarValue.Zero)
                return ScalarValue.Zero;

            var arg = Arg();
            return new ScalarValue(Math.Cos(arg), Math.Sin(arg));
        }

        /// <summary>
        /// Takes the cosine of the scalar.
        /// </summary>
        /// <returns>The cosine of the value.</returns>
        public ScalarValue Cos()
        {
            var re = Math.Cos(_real) * Math.Cosh(_imag);
            var im = -Math.Sin(_real) * Math.Sinh(_imag);
            return new ScalarValue(re, im);
        }

        /// <summary>
        /// Takes the sine of the scalar.
        /// </summary>
        /// <returns>The sine of the value.</returns>
        public ScalarValue Sin()
        {
            var re = Math.Sin(_real) * Math.Cosh(_imag);
            var im = Math.Cos(_real) * Math.Sinh(_imag);
            return new ScalarValue(re, im);
        }

        /// <summary>
        /// Takes the tangent (sin / cos) of the scalar.
        /// </summary>
        /// <returns>The tangent of the value.</returns>
        public ScalarValue Tan()
        {
            return Sin() / Cos();
        }

        /// <summary>
        /// Takes the co-tangent (cos / sin) of the scalar.
        /// </summary>
        /// <returns>The co-tangent of the value.</returns>
        public ScalarValue Cot()
        {
            return Cos() / Sin();
        }

        /// <summary>
        /// Computes the inverse sine of the scalar (arcsin).
        /// </summary>
        /// <returns>The arcsin of the value.</returns>
        public ScalarValue Arcsin()
        {
            var x = _real;
            var y = _imag;
            var yy = y * y;
            var rtp = Math.Sqrt(Math.Pow(x + 1.0, 2.0) + yy);
            var rtn = Math.Sqrt(Math.Pow(x - 1.0, 2.0) + yy);
            var alpha = 0.5 * (rtp + rtn);
            var beta = 0.5 * (rtp - rtn);
            var re = Math.Asin(beta);
            var im = Math.Sign(y) * Math.Log(alpha + Math.Sqrt(alpha * alpha - 1.0));
            return new ScalarValue(re, im);
        }

        /// <summary>
        /// Computes the inverse cosine of the scalar (arccos).
        /// </summary>
        /// <returns>The arccos of the value.</returns>
        public ScalarValue Arccos()
        {
            var x = _real;
            var y = _imag;
            var yy = y * y;
            var rtp = Math.Sqrt(Math.Pow(x + 1.0, 2.0) + yy);
            var rtn = Math.Sqrt(Math.Pow(x - 1.0, 2.0) + yy);
            var alpha = 0.5 * (rtp + rtn);
            var beta = 0.5 * (rtp - rtn);
            var re = Math.Acos(beta);
            var im = -Math.Sign(y) * Math.Log(alpha + Math.Sqrt(alpha * alpha - 1.0));
            return new ScalarValue(re, im);
        }

        /// <summary>
        /// Computes the inverse tangent of the scalar (arctan).
        /// </summary>
        /// <returns>The arctan of the value.</returns>
        public ScalarValue Arctan()
        {
            var x = _real;
            var y = _imag;
            var xx = x * x;
            var yy = y * y;
            var re = 0.5 * Math.Atan2(2.0 * x, 1.0 - xx - yy);
            var im = 0.25 * Math.Log((xx + Math.Pow(y + 1.0, 2.0)) / (xx + Math.Pow(y - 1.0, 2.0)));
            return new ScalarValue(re, im);
        }

        /// <summary>
        /// Computes the inverse cotangent of the scalar (arccot).
        /// </summary>
        /// <returns>The arccot of the value.</returns>
        public ScalarValue Arccot()
        {
            return (1.0 / this).Arctan();
        }

        /// <summary>
        /// Computes the inverse secant of the scalar (arcsec).
        /// </summary>
        /// <returns>The arcsec of the value.</returns>
        public ScalarValue Arcsec()
        {
            return (1.0 / this).Arccos();
        }

        /// <summary>
        /// Computes the inverse cosecant of the scalar (arccsc).
        /// </summary>
        /// <returns>The arccsc of the value.</returns>
        public ScalarValue Arccsc()
        {
            return (1.0 / this).Arcsin();
        }

        /// <summary>
        /// Takes the exponential of the scalar.
        /// </summary>
        /// <returns>The exponential of the value.</returns>
        public ScalarValue Exp()
        {
            var f = Math.Exp(_real);
            var re = f * Math.Cos(_imag);
            var im = f * Math.Sin(_imag);
            return new ScalarValue(re, im);
        }

        /// <summary>
        /// Takes the natural logarithm of the scalar.
        /// </summary>
        /// <returns>The natural logarithm of the value.</returns>
        public ScalarValue Ln()
        {
            var re = Math.Log(Abs());
            var im = Arg();
            return new ScalarValue(re, im);
        }

        /// <summary>
        /// Takes the natural logarithm of the scalar.
        /// </summary>
        /// <returns>The natural logarithm of the value.</returns>
        public ScalarValue Log()
        {
            var re = Math.Log(Abs());
            var im = Arg();
            return new ScalarValue(re, im);
        }

        /// <summary>
        /// Takes the general logarithm of the scalar.
        /// </summary>
        /// <param name="basis">The basis to use for the logarithm.</param>
        /// <returns>The general logarithm (in an arbitrary basis) of the value.</returns>
        public ScalarValue Log(double basis)
        {
            var re = Math.Log(Abs(), basis);
            var im = Arg();
            return new ScalarValue(re, im);
        }

        /// <summary>
        /// Takes the base-10 logarithm of the scalar.
        /// </summary>
        /// <returns>The base-10 logarithm of the value.</returns>
        public ScalarValue Log10()
        {
            var re = Math.Log(Abs(), 10.0);
            var im = Arg();
            return new ScalarValue(re, im);
        }

        /// <summary>
        /// Computes the factorial of the scalar.
        /// </summary>
        /// <returns>The factorial of the scalar x!+iy!.</returns>
        public ScalarValue Factorial()
        {
            var re = 0.0;
            var im = 0.0;

            if (_imag != 0.0)
                im = _imag == Math.Floor(_imag) ? Helpers.Factorial((int)_imag) : Gamma.LinearGamma(_imag + 1.0);

            if (_real != 0.0 || _imag == 0.0)
                re = _real == Math.Floor(_real) ? Helpers.Factorial((int)_real) : Gamma.LinearGamma(_real + 1.0);

            return new ScalarValue(re, im);
        }

        #endregion

        #region Serialization

        /// <summary>
        /// Transforms the instance into a binary representation.
        /// </summary>
        /// <returns>The binary representation.</returns>
        public override byte[] Serialize()
        {
            var re = BitConverter.GetBytes(_real);
            var im = BitConverter.GetBytes(_imag);
            var ov = new byte[re.Length + im.Length];
            re.CopyTo(ov, 0);
            im.CopyTo(ov, re.Length);
            return ov;
        }

        /// <summary>
        /// Transforms a binary representation into a new instance.
        /// </summary>
        /// <param name="content">The binary data.</param>
        /// <returns>The new instance.</returns>
        public override Value Deserialize(byte[] content)
        {
            var real = BitConverter.ToDouble(content, 0);
            var imag = BitConverter.ToDouble(content, 8);
            return new ScalarValue(real, imag);
        }

        #endregion

        #region Math Helpers

        static bool IsPrimeNumber(int n)
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

        #endregion

        #region Comparison

        /// <summary>
        /// Is the given object equal to this.
        /// </summary>
        /// <param name="obj">The compare object.</param>
        /// <returns>A boolean.</returns>
        public override bool Equals(object obj)
        {
            if (obj is ScalarValue)
            {
                var sv = obj as ScalarValue;
                return sv._real == _real && sv._imag == _imag;
            }

            if (obj is double && _imag == 0.0)
                return (double)obj == _real;

            return false;
        }

        /// <summary>
        /// Computes the hashcode of the value inside.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            return (_real + _imag).GetHashCode();
        }

        #endregion

        #region String Representations

        /// <summary>
        /// Gets the length of the output in the given parse context.
        /// </summary>
        /// <param name="context">The query context to consider.</param>
        /// <returns>The length of the string representation</returns>
        public int GetLengthOfString(ParseContext context)
        {
            return ToString(context).Length;
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

            if (Math.Abs(_imag) < epsilon)
                return re;

            var im = Format(context, Math.Abs(_imag) * f) + "i";

            if (Math.Abs(_real) < epsilon)
                return (_imag < 0.0 ? "-" : string.Empty) + im;

            return string.Format("{0} {2} {1}", re, im, _imag < 0.0 ? "-" : "+");
        }

        /// <summary>
        /// Uses the standard string representation.
        /// </summary>
        /// <param name="context">The context to use.</param>
        /// <returns>The string representation.</returns>
        public override string ToString(ParseContext context)
        {
            return ToString(context, 0);
        }

        #endregion

        #region Operators

        /// <summary>
        /// l + r.
        /// </summary>
        /// <param name="l">Left operand</param>
        /// <param name="r">Right operand</param>
        /// <returns>The result.</returns>
        public static ScalarValue operator +(ScalarValue l, ScalarValue r)
        {
            var re = l._real + r._real;
            var im = l._imag + r._imag;
            return new ScalarValue(re, im);
        }

        /// <summary>
        /// a + b.
        /// </summary>
        /// <param name="a">Left operand</param>
        /// <param name="b">Right operand</param>
        /// <returns>The result.</returns>
        public static ScalarValue operator +(ScalarValue a, double b)
        {
            return new ScalarValue(a._real + b, a._imag);
        }

        /// <summary>
        /// b + a.
        /// </summary>
        /// <param name="b">Left operand</param>
        /// <param name="a">Right operand</param>
        /// <returns>The result.</returns>
        public static ScalarValue operator +(double b, ScalarValue a)
        {
            return a + b;
        }

        /// <summary>
        /// l - r.
        /// </summary>
        /// <param name="l">Left operand</param>
        /// <param name="r">Right operand</param>
        /// <returns>The result.</returns>
        public static ScalarValue operator -(ScalarValue l, ScalarValue r)
        {
            var re = l._real - r._real;
            var im = l._imag - r._imag;
            return new ScalarValue(re, im);
        }

        /// <summary>
        /// a - b.
        /// </summary>
        /// <param name="a">Left operand</param>
        /// <param name="b">Right operand</param>
        /// <returns>The result.</returns>
        public static ScalarValue operator -(ScalarValue a, double b)
        {
            return new ScalarValue(a._real - b, a._imag);
        }

        /// <summary>
        /// b - a.
        /// </summary>
        /// <param name="b">Left operand</param>
        /// <param name="a">Right operand</param>
        /// <returns>The result.</returns>
        public static ScalarValue operator -(double b, ScalarValue a)
        {
            return new ScalarValue(b - a._real, -a._imag);
        }

        /// <summary>
        /// l * r.
        /// </summary>
        /// <param name="l">Left operand</param>
        /// <param name="r">Right operand</param>
        /// <returns>The result.</returns>
        public static ScalarValue operator *(ScalarValue l, ScalarValue r)
        {
            if (l.IsZero)
                return new ScalarValue();
            else if (l._real == 0.0)
                return new ScalarValue(-l._imag * r._imag, r._real * l._imag);
            else if (l._imag == 0.0)
                return new ScalarValue(l._real * r._real, l._real * r._imag);

            var re = l._real * r._real - l._imag * r._imag;
            var im = l._real * r._imag + l._imag * r._real;
            return new ScalarValue(re, im);
        }

        /// <summary>
        /// b * a.
        /// </summary>
        /// <param name="b">Left operand</param>
        /// <param name="a">Right operand</param>
        /// <returns>The result.</returns>
        public static ScalarValue operator *(double b, ScalarValue a)
        {
            return a * b;
        }

        /// <summary>
        /// l lighter than r.
        /// </summary>
        /// <param name="l">Left operand</param>
        /// <param name="r">Right operand</param>
        /// <returns>The result.</returns>
        public static bool operator <(ScalarValue l, ScalarValue r)
        {
            if (l.ImaginaryValue == 0.0 && r.ImaginaryValue == 0)
            {
                if (l._real < r._real)
                    return true;
            }
            else if (l.Abs() < r.Abs())
                return true;

            return false;
        }

        /// <summary>
        /// l greater than r.
        /// </summary>
        /// <param name="l">Left operand</param>
        /// <param name="r">Right operand</param>
        /// <returns>The result.</returns>
        public static bool operator >(ScalarValue l, ScalarValue r)
        {
            if (l.ImaginaryValue == 0.0 && r.ImaginaryValue == 0)
            {
                if (l._real > r._real)
                    return true;
            }
            else if (l.Abs() > r.Abs())
                return true;

            return false;
        }

        /// <summary>
        /// l ligher or equal than r.
        /// </summary>
        /// <param name="l">Left operand</param>
        /// <param name="r">Right operand</param>
        /// <returns>The result.</returns>
        public static bool operator <=(ScalarValue l, ScalarValue r)
        {
            if (l.ImaginaryValue == 0.0 && r.ImaginaryValue == 0)
            {
                if (l._real <= r._real)
                    return true;
            }
            else if (l.Abs() <= r.Abs())
                return true;

            return false;
        }

        /// <summary>
        /// l greater or equal than r.
        /// </summary>
        /// <param name="l">Left operand</param>
        /// <param name="r">Right operand</param>
        /// <returns>The result.</returns>
        public static bool operator >=(ScalarValue l, ScalarValue r)
        {
            if (l.ImaginaryValue == 0.0 && r.ImaginaryValue == 0)
            {
                if (l._real >= r._real)
                    return true;
            }
            else if (l.Abs() >= r.Abs())
                return true;

            return false;
        }

        /// <summary>
        /// l == r.
        /// </summary>
        /// <param name="l">Left operand</param>
        /// <param name="r">Right operand</param>
        /// <returns>The result.</returns>
        public static bool operator ==(ScalarValue l, ScalarValue r)
        {
            if (ReferenceEquals(l, r))
                return true;

            if ((object)l == null || (object)r == null)
                return false;

            if (Math.Abs(l.ImaginaryValue - r.ImaginaryValue) > epsilon)
                return false;

            if (Math.Abs(l.Value - r.Value) > epsilon)
                return false;

            return true;
        }

        /// <summary>
        /// l == r.
        /// </summary>
        /// <param name="l">Left operand</param>
        /// <param name="r">Right operand</param>
        /// <returns>The result.</returns>
        public static bool operator ==(ScalarValue l, double r)
        {
            if (l.ImaginaryValue != 0.0)
                return false;

            if (l.Value == r)
                return true;

            return false;
        }

        /// <summary>
        /// l != r.
        /// </summary>
        /// <param name="l">Left operand</param>
        /// <param name="r">Right operand</param>
        /// <returns>The result.</returns>
        public static bool operator !=(ScalarValue l, ScalarValue r)
        {
            return !(l == r);
        }

        /// <summary>
        /// l != r.
        /// </summary>
        /// <param name="l">Left operand</param>
        /// <param name="r">Right operand</param>
        /// <returns>The result.</returns>
        public static bool operator !=(ScalarValue l, double r)
        {
            return !(l == r);
        }

        /// <summary>
        /// a * b.
        /// </summary>
        /// <param name="a">Left operand</param>
        /// <param name="b">Right operand</param>
        /// <returns>The result.</returns>
        public static ScalarValue operator *(ScalarValue a, double b)
        {
            return new ScalarValue(a._real * b, a._imag * b);
        }

        /// <summary>
        /// a % b.
        /// </summary>
        /// <param name="a">Left operand</param>
        /// <param name="b">Right operand</param>
        /// <returns>The result.</returns>
        public static ScalarValue operator %(ScalarValue a, ScalarValue b)
        {
            return new ScalarValue(a.IntValue % b.IntValue);
        }

        /// <summary>
        /// l / r.
        /// </summary>
        /// <param name="l">Left operand</param>
        /// <param name="r">Right operand</param>
        /// <returns>The result.</returns>
        public static ScalarValue operator /(ScalarValue l, ScalarValue r)
        {
            if (r.IsZero)
            {
                if (l.IsZero)
                    return new ScalarValue(1.0);

                return new ScalarValue(l._real != 0.0 ? l._real / 0.0 : 0.0, l._imag != 0.0 ? l._imag / 0.0 : 0.0);
            }

            r = r.Conjugate();
            var q = r._real * r._real + r._imag * r._imag;
            var re = (l._real * r._real - l._imag * r._imag) / q;
            var im = (l._real * r._imag + l._imag * r._real) / q;
            return new ScalarValue(re, im);
        }

        /// <summary>
        /// b / a.
        /// </summary>
        /// <param name="b">Left operand</param>
        /// <param name="a">Right operand</param>
        /// <returns>The result.</returns>
        public static ScalarValue operator /(double a, ScalarValue b)
        {
            return new ScalarValue(a, 0.0) / b;
        }

        /// <summary>
        /// a / b.
        /// </summary>
        /// <param name="a">Left operand</param>
        /// <param name="b">Right operand</param>
        /// <returns>The result.</returns>
        public static ScalarValue operator /(ScalarValue a, double b)
        {
            return new ScalarValue(a._real / b, a._imag / b);
        }

        /// <summary>
        /// -a.
        /// </summary>
        /// <param name="a">Unary operand</param>
        /// <returns>The result.</returns>
        public static ScalarValue operator -(ScalarValue a)
        {
            return new ScalarValue(-a._real, -a._imag);
        }

        #endregion

        #region Register Operators

        /// <summary>
        /// Registers all operators that are associated with the scalar.
        /// </summary>
        protected override void RegisterOperators()
        {
            RegisterPlus(typeof(ScalarValue), typeof(ScalarValue), Add);
            RegisterMinus(typeof(ScalarValue), typeof(ScalarValue), Subtract);
            RegisterMultiply(typeof(ScalarValue), typeof(ScalarValue), Multiply);
            RegisterDivide(typeof(ScalarValue), typeof(ScalarValue), Divide);
            RegisterPower(typeof(ScalarValue), typeof(ScalarValue), Pow);
            RegisterModulo(typeof(ScalarValue), typeof(ScalarValue), Mod);
        }

        /// <summary>
        /// Scalar + Scalar
        /// </summary>
        /// <param name="left">Must be a scalar.</param>
        /// <param name="right">Must be a scalar.</param>
        /// <returns>The new scalar.</returns>
        public static ScalarValue Add(Value left, Value right)
        {
            var l = (ScalarValue)left;
            var r = (ScalarValue)right;
            return l + r;
        }

        /// <summary>
        /// Scalar - Scalar
        /// </summary>
        /// <param name="left">Must be a scalar.</param>
        /// <param name="right">Must be a scalar.</param>
        /// <returns>The new scalar.</returns>
        public static ScalarValue Subtract(Value left, Value right)
        {
            var l = (ScalarValue)left;
            var r = (ScalarValue)right;
            return l - r;
        }

        /// <summary>
        /// Scalar * Scalar
        /// </summary>
        /// <param name="left">Must be a scalar.</param>
        /// <param name="right">Must be a scalar.</param>
        /// <returns>The new scalar.</returns>
        public static ScalarValue Multiply(Value left, Value right)
        {
            var l = (ScalarValue)left;
            var r = (ScalarValue)right;
            return l * r;
        }

        /// <summary>
        /// Scalar / Scalar
        /// </summary>
        /// <param name="left">Must be a scalar.</param>
        /// <param name="right">Must be a scalar.</param>
        /// <returns>The new scalar.</returns>
        public static ScalarValue Divide(Value left, Value right)
        {
            var l = (ScalarValue)left;
            var r = (ScalarValue)right;
            return l / r;
        }

        /// <summary>
        /// Scalar ^ Scalar
        /// </summary>
        /// <param name="basis">Must be a scalar.</param>
        /// <param name="exponent">Must be a scalar.</param>
        /// <returns>The new scalar.</returns>
        public static ScalarValue Pow(Value basis, Value exponent)
        {
            var l = (ScalarValue)basis;
            var r = (ScalarValue)exponent;
            return l.Pow(r);
        }

        /// <summary>
        /// Scalar % Scalar
        /// </summary>
        /// <param name="left">Must be a scalar.</param>
        /// <param name="right">Must be a scalar.</param>
        /// <returns>The new scalar.</returns>
        public static ScalarValue Mod(Value left, Value right)
        {
            var l = (ScalarValue)left;
            var r = (ScalarValue)right;
            return l % r;
        }

        #endregion
    }
}