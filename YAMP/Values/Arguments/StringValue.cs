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
using System.IO;
using System.Text;

namespace YAMP
{
    /// <summary>
    /// The class for representing a string value.
    /// </summary>
	public sealed class StringValue : Value, IFunction
	{
		#region Members

		string _value;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public StringValue()
            : this(string.Empty)
        {
        }

        /// <summary>
        /// Creates a new instance and sets the value.
        /// </summary>
        /// <param name="value">The string where this value is based on.</param>
        public StringValue(string value)
        {
            _value = value;
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="str">The given character array.</param>
        public StringValue(char[] str)
            : this(new string(str))
        {
        }

        #endregion

		#region Properties

        /// <summary>
        /// Gets the string value.
        /// </summary>
		public string Value
		{
			get { return _value; }
		}

        /// <summary>
        /// Gets the length of string value.
        /// </summary>
		public int Length
		{
			get { return _value.Length; }
		}

        /// <summary>
        /// Gets the number of lines in the string value.
        /// </summary>
		public int Lines
		{
			get
			{
				return _value.Split('\n').Length;
			}
		}

		#endregion

        #region Register Operator

        /// <summary>
        /// Registers the allowed operations.
        /// </summary>
        protected override void RegisterOperators()
        {
            PlusOperator.Register(typeof(StringValue), typeof(Value), Add);
            PlusOperator.Register(typeof(Value), typeof(StringValue), Add);
        }

        /// <summary>
        /// Performs the addition str + x or x + str.
        /// </summary>
        /// <param name="left">An arbitrary value.</param>
        /// <param name="right">Another arbitrary value.</param>
        /// <returns>The result of the operation.</returns>
        public static StringValue Add(Value left, Value right)
        {
            return new StringValue(left.ToString() + right.ToString());
        }

        #endregion

        #region Serialization

        /// <summary>
        /// Returns a copy of this string value instance.
        /// </summary>
        /// <returns>The cloned string value.</returns>
        public override Value Copy()
        {
            return new StringValue(_value);
        }

        /// <summary>
        /// Converts the given value into binary data.
        /// </summary>
        /// <returns>The bytes array containing the data.</returns>
        public override byte[] Serialize()
		{
            using (var ms = Serializer.Create())
            {
                ms.Serialize(_value);
                return ms.Value;
            }
		}

        /// <summary>
        /// Creates a new string value from the binary content.
        /// </summary>
        /// <param name="content">The data which contains the content.</param>
        /// <returns>The new instance.</returns>
		public override Value Deserialize(byte[] content)
		{
            var value = string.Empty;

            using(var ds = Deserializer.Create(content))
            {
                value = ds.GetString();
            }

			return new StringValue(value);
		}

		#endregion

        #region Conversations

        /// <summary>
        /// Explicit conversation from a string to a scalar.
        /// </summary>
        /// <param name="value">The stringvalue that will be casted.</param>
        /// <returns>The scalar with Re = sum over all characters and Im = length of the string.</returns>
        public static explicit operator ScalarValue(StringValue value)
        {
            var sum = 0.0;

            foreach(var c in value.Value)
                sum += (double)c;

            return new ScalarValue(sum, value.Length);
        }

        #endregion

        #region Index

        /// <summary>
        /// Gets the 1-based character of the string.
        /// </summary>
        /// <param name="index">The 1-based character (1 == first character) index.</param>
        /// <returns>The character at the position.</returns>
        public char this[int index]
        {
            get
            {
                if (index < 1 || index > _value.Length)
                    throw new ArgumentOutOfRangeException("Access in string out of bounds.");

                return _value[index - 1];
            }
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Returns the string content of this instance.
        /// </summary>
        /// <param name="context">The context of the invocation.</param>
        /// <returns>The value of the string.</returns>
        public override string ToString (ParseContext context)
		{
			return _value;
		}

		#endregion

        #region Functional behavior

        /// <summary>
        /// If invoked like a function the function reacts like this.
        /// </summary>
        /// <param name="context">The context of invocation.</param>
        /// <param name="argument">The argument(s) that have been given.</param>
        /// <returns>The subset of the string.</returns>
        public Value Perform(ParseContext context, Value argument)
        {
            if (argument is NumericValue)
            {
                var idx = BuildIndex(argument, Length);
                var str = new char[idx.Length];

                for (var i = 0; i < idx.Length; i++)
                    str[i] = _value[idx[i]];

                return new StringValue(str);
            }

            throw new YAMPArgumentWrongTypeException(argument.Header, new [] { "Matrix", "Scalar" }, "String");
        }

        #endregion
    }
}

