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
            double log;
            value = Math.Abs(value);
            
            if (value < 1.0)
                log = Math.Log10(1.0 / value);
            else
                log = Math.Log10(value);

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

        /// <summary>
        /// Creates a copy of the given object using reflection.
        /// Override for using a more specialized version.
        /// </summary>
        /// <returns>The copy or original in case of a failed copy.</returns>
        public virtual Value Copy()
        {
            var T = GetType();

            // Get all the fields of the type, also the privates.
            var fis = T.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var clone = Activator.CreateInstance(T) as Value;

            if (clone == null)
                return this;

            // Loop through all the fields and copy the information from the source to the target
            foreach (var fi in fis)
                fi.SetValue(clone, fi.GetValue(this));

            // Return the cloned object.
            return clone;
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
            if (double.IsPositiveInfinity(value))
                return "inf";
            else if (double.IsNegativeInfinity(value))
                return "-inf";
            else if (double.IsNaN(value))
                return "nan";

            int sign = Math.Sign(value);
            double v = value * sign;
            double t = v;
            int u = v >= 1 ? (int)Math.Log10(v) + 1 : 0;
            int l = 0;

            while (Math.Floor(t) != t)
            {
                t *= 10.0;
                l++;
            }

            switch (context.DefaultDisplayStyle)
            {
                case DisplayStyle.Scientific:
                    return ScientificFormat(context, v, sign, u, l);

                case DisplayStyle.Engineering:
                    return EngineeringFormat(context, v, sign, u, l);

                case DisplayStyle.Default:
                default:
                    return StandardFormat(context, v, sign, u, l);
            }
        }

        static string ScientificFormat(ParseContext context, double value, int sign, int U, int L)
        {
            var v = sign * value;

            if (U > 1)
                return Compose(context, v * Math.Pow(10.0, 1 - U), context.Precision, U - 1);

            if (U == 0 && L > 0)
            {
                var f = 0;

                while (Math.Floor(v) == 0)
                {
                    v *= 10.0;
                    f++;
                }

                return Compose(context, v, context.Precision, -f);
            }

            return Compose(context, v, context.Precision, 0);
        }

        static string EngineeringFormat(ParseContext context, double value, int sign, int U, int L)
        {
            var v = sign * value;

            if (U > 3)
            {
                var pwr = 3 * ((U - 1) / 3);
                return Compose(context, v * Math.Pow(10.0, -pwr), context.Precision, pwr);
            }

            if (U == 0 && L > 0)
            {
                var f = 0;

                while (Math.Floor(v) == 0)
                {
                    f++;
                    v *= 10.0;
                }

                var t = f / 3;
                var t3 = t * 3 - f;

                if (t3 != 0)
                {
                    t++;
                    v *= Math.Pow(10.0, t3 + 3);
                }

                return Compose(context, v, context.Precision, -3 * t);
            }

            return Compose(context, v, context.Precision, 0);
        }

        static string StandardFormat(ParseContext context, double v, int sign, int U, int L)
        {
            return Compose(context, v * sign, Math.Min(context.Precision, L), 0);
        }

        static string Compose(ParseContext context, double value, int decimals, int exponent)
        {
            if (exponent == 0)
                return string.Format(numberFormat, "{0:F" + decimals + "}", value);

            if (context.CustomExponent)
                return string.Format(numberFormat, "{0:F" + decimals + "}{1}", value, ToSuperScript(exponent));

            return string.Format(numberFormat, "{0:F" + decimals + "}e{1}", value, exponent);
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