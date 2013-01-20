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
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace YAMP
{
    /// <summary>
    /// Abstract base value for any value type.
    /// </summary>
	public abstract class Value : IRegisterElement
    {
        #region Members

        static readonly Value _empty = new ScalarValue();
        static readonly NumberFormatInfo numberFormat = new CultureInfo("en-US").NumberFormat;
        static readonly Dictionary<string, Value> knownTypes = new Dictionary<string, Value>();

        /// <summary>
        /// A little helper for reflection (same as Type.Empty in the full .NET stack).
        /// </summary>
        public static readonly Type[] EmptyTypes = new Type[0];

        #endregion

        #region Properties

        /// <summary>
        /// Gets the empty value (a simple scalar).
        /// </summary>
        public static Value Empty
		{
			get
			{
				return _empty;
			}
		}

        /// <summary>
        /// Gets the name of the type (just removes the Value convention).
        /// </summary>
		public string Header
		{
			get
			{
				return GetType().Name.RemoveValueConvention();
			}
		}

        #endregion

        #region Methods

        /// <summary>
        /// This represents a save cast from a string to a double.
        /// </summary>
        /// <param name="value">The string value.</param>
        /// <returns>The double value (or NaN if it could not been casted).</returns>
        public static double CastStringToDouble(string value)
        {
            double x;

            if(double.TryParse(value, NumberStyles.Float, numberFormat, out x))
                return x;

            return double.NaN;
        }

        /// <summary>
        /// Gets the exponent (10^n) of a double value.
        /// </summary>
        /// <param name="value">The value to get the exponent from.</param>
        /// <returns>The exponent n (10^n) of the double value.</returns>
        protected int GetExponent(double value)
        {
            var log = Math.Log10(Math.Abs(value));
            return (int)Math.Floor(log);
        }

        /// <summary>
        /// Builds the index from the given value.
        /// </summary>
        /// <param name="arg">The argument to inspect.</param>
        /// <param name="max">The maximum number of arguments.</param>
        /// <returns></returns>
        protected static int[] BuildIndex(Value arg, int max)
        {
            if (arg is ScalarValue)
                return new int[] { ((ScalarValue)arg).IntValue };

            var idx = new List<int>();

            if (arg is RangeValue)
            {
                var r = (RangeValue)arg;
                var step = (int)r.Step;
                var maxLength = r.All ? max : (int)r.End;

                for (var j = (int)r.Start; j <= maxLength; j += step)
                    idx.Add(j);
            }
            else if (arg is MatrixValue)
            {
                var m = (MatrixValue)arg;

                for (var j = 1; j <= m.Length; j++)
                    idx.Add(m[j].IntValue);
            }

            return idx.ToArray();
        }

        /// <summary>
        /// Registers the element at a certain point.
        /// </summary>
        public void RegisterElement()
        {
            var name = Header;

            if (!knownTypes.ContainsKey(name))
                knownTypes.Add(name, this);

            RegisterOperators();
        }

        /// <summary>
        /// Registers the operators (if there are any).
        /// </summary>
        protected virtual void RegisterOperators()
        {
            //Nothing to register here.
        }

        #endregion

        #region Serialization

        /// <summary>
        /// Converts the instance to bytes.
        /// </summary>
        /// <returns>The binary content.</returns>
        public abstract byte[] Serialize();

        /// <summary>
        /// Creates a new instance from bytes.
        /// </summary>
        /// <param name="content">The binary content.</param>
        /// <returns>The new instance.</returns>
		public abstract Value Deserialize(byte[] content);

		internal static Value Deserialize(string name, byte[] content)
		{
            if (knownTypes.ContainsKey(name))
                return knownTypes[name].Deserialize(content);

			return Value.Empty;
		}

        #endregion

        #region String Representation

        /// <summary>
        /// Returns a string representation of the value.
        /// </summary>
        /// <returns>The string.</returns>
        public override string ToString()
        {
            return ToString(ParseContext.Default);
        }

        /// <summary>
        /// Returns a string representation of the value.
        /// </summary>
        /// <param name="context">The calling context.</param>
        /// <returns>The string representation.</returns>
        public virtual string ToString(ParseContext context)
        {
            return string.Empty;
        }

        #endregion

        #region String Representation Helpers

        /// <summary>
        /// Formats a given double value with the rules of the context.
        /// </summary>
        /// <param name="context">The context with the rules.</param>
        /// <param name="value">The double precision value.</param>
        /// <returns>The string representation.</returns>
        public static string Format(ParseContext context, double value)
        {
            double amt;
            int sign, exponent;

            if (double.IsPositiveInfinity(value))
                return "inf";
            else if (double.IsNegativeInfinity(value))
                return "-inf";
            else if (double.IsNaN(value))
                return "nan";

            switch (context.DefaultDisplayStyle)
            {
                case DisplayStyle.Scientific:
                    ScientificFormat(context, value, out sign, out amt, out exponent);
                    break;
                case DisplayStyle.Engineering:
                    EngineeringFormat(context, value, out sign, out amt, out exponent);
                    break;
                default:
                    sign = Math.Sign(value);
                    amt = sign * value;
                    exponent = 0;
                    break;
            }
            
            return ComposeFormat(context, sign, amt, exponent);
        }

        static void ScientificFormat(ParseContext context, double value, out int sign, out double new_value, out int exponent)
        {
            sign = Math.Sign(value);
            value = sign * value;
            var prec = (double)context.Precision;
            var suggested = Math.Pow(10.0, prec);

            if (value > 1.0)
                exponent = (int)(Math.Floor(Math.Log10(value) / prec) * prec);
            else if (value > 0.0)
                exponent = (int)(Math.Ceiling(Math.Log10(value)));
            else
                exponent = 0;

            new_value = value * Math.Pow(10.0, -exponent);

            if (new_value >= suggested)
            {
                new_value /= suggested;
                exponent += context.Precision;
            }

            if (new_value < 1 && new_value > 0)
            {
                new_value *= 10.0;
                exponent -= 1;
            }
        }

        static void EngineeringFormat(ParseContext context, double value, out int sign, out double new_value, out int exponent)
        {
            sign = Math.Sign(value);
            value = sign * value;

            if (value > 1.0)
                exponent = (int)(Math.Floor(Math.Log10(value) / 3.0) * 3.0);
            else if(value > 0.0)
                exponent = (int)(Math.Ceiling(Math.Log10(value) / 3.0) * 3.0);
            else
                exponent = 0;

            new_value = value * Math.Pow(10.0, -exponent);

            if (new_value >= 1e3)
            {
                new_value *= 1e-3;
                exponent += 3;
            }

            if (new_value <= 1e-3 && new_value > 0)
            {
                new_value *= 1e3;
                exponent -= 3;
            }
        }

        static string ComposeFormat(ParseContext context, int sign, double v, int exponent)
        {
            int expsign = Math.Sign(exponent);
            int significant_digits = context.Precision;
            exponent = Math.Abs(exponent);
            int digits = v > 0 ? (int)Math.Log10(v) + 1 : 0;
            significant_digits += Math.Abs(v) < 1.0 ? 1 : 0;
            int decimals = Math.Max(significant_digits - digits, 0);
            double round = Math.Pow(10, -decimals);
            digits = v > 0 ? (int)Math.Log10(v + 0.5 * round) + 1 : 0;
            decimals = Math.Max(significant_digits - digits, 0);
            string f = "0:F";

            if (exponent == 0)
                return string.Format(numberFormat, "{" + f + decimals + "}", sign * v);
            else if (context.CustomExponent)
                return string.Format(numberFormat, "{" + f + decimals + "}{1}", sign * v, ToSuperScript(expsign * exponent));
            
            return string.Format(numberFormat, "{" + f + decimals + "}e{1}", sign * v, expsign * exponent);
        }

        static string ToSuperScript(int exp)
        {
            var sb = new StringBuilder();
            sb.Append("·10");
            var str = exp.ToString();

            foreach (var ch in str)
            {
                var target = '⁻';

                switch(ch)
                {
                    case '0': target = '⁰'; break;
                    case '1': target = '¹'; break;
                    case '2': target = '²'; break;
                    case '3': target = '³'; break;
                    case '4': target = '⁴'; break;
                    case '5': target = '⁵'; break;
                    case '6': target = '⁶'; break;
                    case '7': target = '⁷'; break;
                    case '8': target = '⁸'; break;
                    case '9': target = '⁹'; break;
                }

                sb.Append(target);
            }

            return sb.ToString();
        }

        #endregion

        #region Register Operators

        /// <summary>
        /// Helper for registering a plus operator.
        /// </summary>
        /// <param name="left">The type on the left side.</param>
        /// <param name="right">The type on the right side.</param>
        /// <param name="add">The function to execute.</param>
        protected static void RegisterPlus(Type left, Type right, Func<Value, Value, Value> add)
        {
            PlusOperator.Register(left, right, add);
        }

        /// <summary>
        /// Helper for registering a multiplication operator.
        /// </summary>
        /// <param name="left">The type on the left side.</param>
        /// <param name="right">The type on the right side.</param>
        /// <param name="multiply">The function to execute.</param>
        protected static void RegisterMultiply(Type left, Type right, Func<Value, Value, Value> multiply)
        {
            MultiplyOperator.Register(left, right, multiply);
        }

        /// <summary>
        /// Helper for registering a division operator.
        /// </summary>
        /// <param name="left">The type on the left side.</param>
        /// <param name="right">The type on the right side.</param>
        /// <param name="divide">The function to execute.</param>
        protected static void RegisterDivide(Type left, Type right, Func<Value, Value, Value> divide)
        {
            RightDivideOperator.Register(left, right, divide);
        }

        /// <summary>
        /// Helper for registering a minus operator.
        /// </summary>
        /// <param name="left">The type on the left side.</param>
        /// <param name="right">The type on the right side.</param>
        /// <param name="sub">The function to execute.</param>
        protected static void RegisterMinus(Type left, Type right, Func<Value, Value, Value> sub)
        {
            MinusOperator.Register(left, right, sub);
        }

        /// <summary>
        /// Helper for registering a power operator.
        /// </summary>
        /// <param name="left">The type on the left side.</param>
        /// <param name="right">The type on the right side.</param>
        /// <param name="power">The function to execute.</param>
        protected static void RegisterPower(Type left, Type right, Func<Value, Value, Value> power)
        {
            PowerOperator.Register(left, right, power);
        }

        /// <summary>
        /// Helper for registering a modulo operator.
        /// </summary>
        /// <param name="left">The type on the left side.</param>
        /// <param name="right">The type on the right side.</param>
        /// <param name="mod">The function to execute.</param>
        protected static void RegisterModulo(Type left, Type right, Func<Value, Value, Value> mod)
        {
            ModuloOperator.Register(left, right, mod);
        }

        #endregion
    }
}