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
using System.Reflection;
using System.Text;

namespace YAMP
{
	public abstract class Value
    {
        #region Members

        static readonly Value _empty = new ScalarValue();

        public static readonly Type[] EmptyTypes = new Type[0];

        #endregion

        #region Properties

        public static Value Empty
		{
			get
			{
				return _empty;
			}
		}

		public string Header
		{
			get
			{
				return GetType().Name.RemoveValueConvention();
			}
		}

        #endregion

        #region Methods

        protected int GetExponent(double value)
        {
            var log = Math.Log10(Math.Abs(value));
            return (int)Math.Floor(log);
        }

        public abstract Value Add(Value right);
		
		public abstract Value Subtract(Value right);
		
		public abstract Value Multiply(Value right);
		
		public abstract Value Divide(Value denominator);
		
		public abstract Value Power(Value exponent);

		public abstract byte[] Serialize();

		public abstract Value Deserialize(byte[] content);

		internal static Value Deserialize(string name, byte[] content)
		{
			name = name + "Value";
			var types = Assembly.GetCallingAssembly().GetTypes();

			foreach(var target in types)
			{
				if(target.Name.Equals(name))
				{
                    var value = target.GetConstructor(EmptyTypes).Invoke(null) as Value;
					return value.Deserialize(content);
				}
			}

			return Value.Empty;
		}

        public override string ToString()
        {
            return ToString(ParseContext.Default);
        }

        public virtual string ToString(ParseContext context)
        {
            return string.Empty;
        }

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

        #endregion

        #region String Representation Helpers

        public static string Format(ParseContext context, double value)
        {
            double amt;
            int sign, exponent;

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
                return string.Format(context.NumberFormat, "{" + f + decimals + "}", sign * v);
            else if (context.CustomExponent)
                return string.Format(context.NumberFormat, "{" + f + decimals + "}{1}", sign * v, ToSuperScript(expsign * exponent));
            
            return string.Format(context.NumberFormat, "{" + f + decimals + "}e{1}", sign * v, expsign * exponent);
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
    }
}