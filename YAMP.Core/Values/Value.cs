namespace YAMP
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Reflection;
    using System.Text;

    /// <summary>
    /// Abstract base value for any value type.
    /// </summary>
	public abstract class Value : IRegisterElement
    {
        #region Fields

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
        public static Double CastStringToDouble(String value)
        {
            var x = 0.0;

            if (Double.TryParse(value, NumberStyles.Float, numberFormat, out x))
            {
                return x;
            }

            return Double.NaN;
        }

        /// <summary>
        /// Gets the exponent (10^n) of a double value.
        /// </summary>
        /// <param name="value">The value to get the exponent from.</param>
        /// <returns>The exponent n (10^n) of the double value.</returns>
        protected Int32 GetExponent(Double value)
        {
            var log = 0.0;
            value = Math.Abs(value);

            if (value < 1.0)
            {
                log = Math.Log10(1.0 / value);
            }
            else
            {
                log = Math.Log10(value);
            }

            return (Int32)Math.Floor(log);
        }

        /// <summary>
        /// Builds the index from the given value.
        /// </summary>
        /// <param name="arg">The argument to inspect.</param>
        /// <param name="max">The maximum number of arguments.</param>
        /// <returns></returns>
        protected static Int32[] BuildIndex(Value arg, Int32 max)
        {
            if (arg is ScalarValue)
            {
                return new int[] { ((ScalarValue)arg).IntValue };
            }

            var idx = new List<Int32>();

            if (arg is RangeValue)
            {
                var r = (RangeValue)arg;
                var step = (Int32)r.Step;
                var maxLength = r.All ? max : (Int32)r.End;

                for (var j = (Int32)r.Start; j <= maxLength; j += step)
                {
                    idx.Add(j);
                }
            }
            else if (arg is MatrixValue)
            {
                var m = (MatrixValue)arg;

                for (var j = 1; j <= m.Length; j++)
                {
                    idx.Add(m[j].IntValue);
                }
            }

            return idx.ToArray();
        }

        /// <summary>
        /// Registers the element at a certain point.
        /// </summary>
        public void RegisterElement(IElementMapping elementMapping)
        {
            var name = Header;

            if (!knownTypes.ContainsKey(name))
            {
                knownTypes.Add(name, this);
            }

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

            if (clone != null)
            {
                foreach (var fi in fis)
                {
                    fi.SetValue(clone, fi.GetValue(this));
                }

                return clone;
            }

            return this;
        }

        #endregion

        #region Serialization

        /// <summary>
        /// Converts the instance to bytes.
        /// </summary>
        /// <returns>The binary content.</returns>
        public abstract Byte[] Serialize();

        /// <summary>
        /// Creates a new instance from bytes.
        /// </summary>
        /// <param name="content">The binary content.</param>
        /// <returns>The new instance.</returns>
		public abstract Value Deserialize(Byte[] content);

        /// <summary>
        /// Creates a new named instance from the content.
        /// </summary>
        /// <param name="name">The name of the instance.</param>
        /// <param name="content">The raw content.</param>
        /// <returns>The created value.</returns>
		public static Value Deserialize(String name, Byte[] content)
		{
            if (knownTypes.ContainsKey(name))
            {
                return knownTypes[name].Deserialize(content);
            }

			return Value.Empty;
		}

        #endregion

        #region String Representation

        /// <summary>
        /// Returns a string representation of the value.
        /// </summary>
        /// <returns>The string.</returns>
        public override String ToString()
        {
            return ToString(ParseContext.Root);
        }

        /// <summary>
        /// Returns a string representation of the value.
        /// </summary>
        /// <param name="context">The calling context.</param>
        /// <returns>The string representation.</returns>
        public virtual String ToString(ParseContext context)
        {
            return String.Empty;
        }

        #endregion

        #region String Representation Helpers

        /// <summary>
        /// Formats a given double value with the rules of the context.
        /// </summary>
        /// <param name="context">The context with the rules.</param>
        /// <param name="value">The double precision value.</param>
        /// <returns>The string representation.</returns>
        public static String Format(ParseContext context, Double value)
        {
            if (Double.IsPositiveInfinity(value))
            {
                return "inf";
            }
            else if (Double.IsNegativeInfinity(value))
            {
                return "-inf";
            }
            else if (Double.IsNaN(value))
            {
                return "nan";
            }

            var sign = Math.Sign(value);
            var v = value * sign;
            var t = v;
            var u = v >= 1 ? (Int32)Math.Log10(v) + 1 : 0;
            var l = 0;

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

        static String ScientificFormat(ParseContext context, Double value, Int32 sign, Int32 U, Int32 L)
        {
            var v = sign * value;

            if (U > 1)
            {
                return Compose(context, v * Math.Pow(10.0, 1 - U), context.Precision, U - 1);
            }
            else if (U == 0 && L > 0)
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

        static String EngineeringFormat(ParseContext context, Double value, Int32 sign, Int32 U, Int32 L)
        {
            var v = sign * value;

            if (U > 3)
            {
                var pwr = 3 * ((U - 1) / 3);
                return Compose(context, v * Math.Pow(10.0, -pwr), context.Precision, pwr);
            }
            else if (U == 0 && L > 0)
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

        static String StandardFormat(ParseContext context, Double v, Int32 sign, Int32 U, Int32 L)
        {
            return Compose(context, v * sign, Math.Min(context.Precision, L), 0);
        }

        static string Compose(ParseContext context, Double value, Int32 decimals, Int32 exponent)
        {
            if (exponent == 0)
            {
                return String.Format(numberFormat, "{0:F" + decimals + "}", value);
            }
            else if (context.CustomExponent)
            {
                return String.Format(numberFormat, "{0:F" + decimals + "}{1}", value, ToSuperScript(exponent));
            }

            return String.Format(numberFormat, "{0:F" + decimals + "}e{1}", value, exponent);
        }

        static String ToSuperScript(Int32 exp)
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
            Register.BinaryOperator(PlusOperator.Symbol, left, right, add);
        }

        /// <summary>
        /// Helper for registering a multiplication operator.
        /// </summary>
        /// <param name="left">The type on the left side.</param>
        /// <param name="right">The type on the right side.</param>
        /// <param name="multiply">The function to execute.</param>
        protected static void RegisterMultiply(Type left, Type right, Func<Value, Value, Value> multiply)
        {
            Register.BinaryOperator(MultiplyOperator.Symbol, left, right, multiply);
        }

        /// <summary>
        /// Helper for registering a division operator.
        /// </summary>
        /// <param name="left">The type on the left side.</param>
        /// <param name="right">The type on the right side.</param>
        /// <param name="divide">The function to execute.</param>
        protected static void RegisterDivide(Type left, Type right, Func<Value, Value, Value> divide)
        {
            Register.BinaryOperator(RightDivideOperator.Symbol, left, right, divide);
        }

        /// <summary>
        /// Helper for registering a minus operator.
        /// </summary>
        /// <param name="left">The type on the left side.</param>
        /// <param name="right">The type on the right side.</param>
        /// <param name="sub">The function to execute.</param>
        protected static void RegisterMinus(Type left, Type right, Func<Value, Value, Value> sub)
        {
            Register.BinaryOperator(MinusOperator.Symbol, left, right, sub);
        }

        /// <summary>
        /// Helper for registering a power operator.
        /// </summary>
        /// <param name="left">The type on the left side.</param>
        /// <param name="right">The type on the right side.</param>
        /// <param name="power">The function to execute.</param>
        protected static void RegisterPower(Type left, Type right, Func<Value, Value, Value> power)
        {
            Register.BinaryOperator(PowerOperator.Symbol, left, right, power);
        }

        /// <summary>
        /// Helper for registering a modulo operator.
        /// </summary>
        /// <param name="left">The type on the left side.</param>
        /// <param name="right">The type on the right side.</param>
        /// <param name="mod">The function to execute.</param>
        protected static void RegisterModulo(Type left, Type right, Func<Value, Value, Value> mod)
        {
            Register.BinaryOperator(ModuloOperator.Symbol, left, right, mod);
        }

        #endregion
    }
}