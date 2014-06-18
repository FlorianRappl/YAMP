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
using System.Text;
using System.Collections.Generic;
using System.IO;

namespace YAMP
{
    /// <summary>
    /// The value type for containing an array of arbitrary Value instances.
    /// This class is used for transporting arguments as well as returning
    /// multiple output Value instances.
    /// </summary>
	public sealed class ArgumentsValue : Value, IFunction, IEnumerable<Value>
	{
		#region Members

		List<Value> _values;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public ArgumentsValue()
        {
            _values = new List<Value>();
        }

        /// <summary>
        /// Creates a new instance containing the specified values.
        /// </summary>
        /// <param name="values">The values to include.</param>
        public ArgumentsValue(params Value[] values)
            : this()
        {
            foreach (var value in values)
                _values.Add(value);
        }

        #endregion

		#region Properties

        /// <summary>
        /// Gets the array of contained values.
        /// </summary>
		public Value[] Values
		{
			get { return _values.ToArray(); }
		}

        /// <summary>
        /// Gets the length of the list of values.
        /// </summary>
		public int Length
		{
			get { return _values.Count; }
		}

        /// <summary>
        /// Gets or sets the value in the 1-based list of values.
        /// </summary>
        /// <param name="i">The 1-based (NOT 0-based!) index.</param>
        /// <returns>The Value of the corresponding index.</returns>
		public Value this[int i]
		{
			get
            {
                if (i < 1 || i > _values.Count)
                    throw new YAMPIndexOutOfBoundException(i, 1, _values.Count);

				return _values[i - 1];
			}
            set
            {
                if (i < 1)
                    throw new YAMPIndexOutOfBoundException(i, 1);
                else if (i == _values.Count + 1)
                    _values.Add(value);
                else if(i <= _values.Count)
                    _values[i] = value;
            }
		}

        #endregion

        #region Serialization

        /// <summary>
        /// Serializes the instance.
        /// </summary>
        /// <returns>The binary content for creating such an instance again.</returns>
        public override byte[] Serialize()
        {
            byte[] content;
            using (var ms = new System.IO.MemoryStream())
            {
                var len = BitConverter.GetBytes(_values.Count);
                ms.Write(len, 0, len.Length);

                foreach (var value in _values)
                {
                    var idx = Encoding.UTF8.GetBytes(value.Header);
                    len = BitConverter.GetBytes(idx.Length);
                    ms.Write(len, 0, len.Length);
                    ms.Write(idx, 0, idx.Length);
                    var entry = value.Serialize();
                    len = BitConverter.GetBytes(entry.Length);
                    ms.Write(len, 0, len.Length);
                    ms.Write(entry, 0, entry.Length);
                }

                content = ms.ToArray();
            }
            return content;
        }

        /// <summary>
        /// Create a new instance from the given binary content.
        /// </summary>
        /// <param name="content">The content in bytes.</param>
        /// <returns>The new instance.</returns>
        public override Value Deserialize(byte[] content)
        {
            var A = new ArgumentsValue();

            using (var ms = new MemoryStream(content))
            {
                var buffer = new byte[4];
                ms.Read(buffer, 0, buffer.Length);
                var count = BitConverter.ToInt32(buffer, 0);

                for (var i = 0; i < count; i++)
                {
                    ms.Read(buffer, 0, buffer.Length);
                    var length = BitConverter.ToInt32(buffer, 0);
                    var stringBuffer = new byte[length];
                    ms.Read(stringBuffer, 0, stringBuffer.Length);
                    var name = Encoding.UTF8.GetString(stringBuffer, 0, stringBuffer.Length);
                    ms.Read(buffer, 0, buffer.Length);
                    length = BitConverter.ToInt32(buffer, 0);
                    var contentBuffer = new byte[length];
                    ms.Read(contentBuffer, 0, contentBuffer.Length);
                    A._values.Add(Deserialize(name, contentBuffer));
                }
            }

            return A;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the first value contained in the arguments.
        /// </summary>
        /// <returns>The first value or null.</returns>
        public Value First()
        {
            if (_values.Count > 0)
                return _values[0];

            return null;
        }

        /// <summary>
        /// Gets the last value contained in the arguments.
        /// </summary>
        /// <returns>The last value or null.</returns>
        public Value Last()
        {
            if (_values.Count > 0)
                return _values[_values.Count - 1];

            return null;
        }

        /// <summary>
        /// Appends a value or a list of values to the list of values.
        /// </summary>
        /// <param name="value">The instance to append to the list.</param>
        /// <returns>The current instance.</returns>
		internal ArgumentsValue Append(Value value)
		{
			if(value is ArgumentsValue)
				_values.AddRange((value as ArgumentsValue)._values);
			else
				_values.Add(value);

			return this;
		}

        /// <summary>
        /// Returns an array of the contained values.
        /// </summary>
        /// <returns>The array with all the values.</returns>
        public Value[] ToArray()
        {
            return _values.ToArray();
        }

        /// <summary>
        /// Inserts a value to the end of the list. A list of values will be treated as one value.
        /// </summary>
        /// <param name="value">The instance to add to the list.</param>
        /// <returns>The current instance.</returns>
		public ArgumentsValue Insert(Value value)
		{
			_values.Add(value);
            return this;
		}

        /// <summary>
        /// Inserts a value to the end of the list. A list of values will be treated as one value.
        /// </summary>
        /// <param name="index">Where to insert the value.</param>
        /// <param name="value">The instance to add to the list.</param>
        /// <returns>The current instance.</returns>
        public ArgumentsValue Insert(int index, Value value)
        {
            _values.Insert(index, value);
            return this;
        }

		#endregion

		#region Static

        /// <summary>
        /// Creates a new ArgumentsValue with 2 values.
        /// </summary>
        /// <param name="left">The first value to include.</param>
        /// <param name="right">The second value to include.</param>
        /// <returns>The new ArgumentsValue instance.</returns>
		public static ArgumentsValue Create (Value left, Value right)
		{
			var a = new ArgumentsValue();
			a.Append(left).Append(right);
			return a;
		}

		#endregion

        #region Overrides

        /// <summary>
        /// Returns a string representation of the content.
        /// </summary>
        /// <param name="context">The context to consider.</param>
        /// <returns>The string representation.</returns>
        public override string ToString(ParseContext context)
        {
            var first = First();

            if (first == null)
                return string.Empty;

            return first.ToString(context);
        }

        #endregion

        #region Functional Behavior

        /// <summary>
        /// Uses the instance from YAMP like a function.
        /// </summary>
        /// <param name="context">The context in which this instance is used.</param>
        /// <param name="argument">The indices that have been specified.</param>
        /// <returns>The value behind the given index.</returns>
        public Value Perform(ParseContext context, Value argument)
        {
            if (argument is ScalarValue)
                return this[((ScalarValue)argument).IntValue];

            throw new YAMPArgumentWrongTypeException(argument.Header, "Scalar", "Arguments");
        }

        #endregion

        #region Enumerable

        /// <summary>
        /// Gets an enumerator of this ArgumentsValue.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<Value> GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        #endregion
    }
}