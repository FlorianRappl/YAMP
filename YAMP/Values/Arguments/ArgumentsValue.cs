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
using System.Text;
using System.Collections.Generic;

namespace YAMP
{
	public class ArgumentsValue : Value, ISupportsIndex
	{
		#region Members

		List<Value> _values;

		#endregion

		#region implemented abstract members of Value

		public override Value Add (Value right)
		{
			return ArgumentsValue.Create(this, right);
		}

		public override Value Subtract (Value right)
		{
			throw new OperationNotSupportedException("-", this);
		}

		public override Value Multiply (Value right)
		{
			throw new OperationNotSupportedException("*", this);
		}

		public override Value Divide (Value denominator)
		{
			throw new OperationNotSupportedException("/", this);
		}

		public override Value Power (Value exponent)
		{
			throw new OperationNotSupportedException("^", this);
		}

		public override byte[] Serialize ()
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

		public override Value Deserialize (byte[] content)
		{
            using (var ms = new System.IO.MemoryStream(content))
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
                    _values.Add(Deserialize(name, contentBuffer));
                }
            }
		    return this;
		}

		#endregion

		#region Properties

		public Value[] Values
		{
			get { return _values.ToArray(); }
		}

		public int Length
		{
			get { return _values.Count; }
		}

		public Value this[int i]
		{
			get
            {
                if (i < 1 || i > _values.Count)
                    throw new ArgumentOutOfRangeException("Access in list out of bounds.");

				return _values[i - 1];
			}
            set
            {
                if(i < 1)
                    throw new ArgumentOutOfRangeException("Access in list out of bounds.");
                else if (i == _values.Count + 1)
                    _values.Add(value);
                else if(i <= _values.Count)
                    _values[i] = value;
            }
		}

		#endregion

		#region ctor

		public ArgumentsValue ()
		{
			_values = new List<Value>();
		}

        public ArgumentsValue(params Value[] values) : this()
        {
            foreach (var value in values)
                _values.Add(value);
        }

		#endregion

        #region Methods

        public Value First()
        {
            if (_values.Count > 0)
                return _values[0];

            return null;
        }

		ArgumentsValue Append (Value value)
		{
			if(value is ArgumentsValue)
				_values.AddRange((value as ArgumentsValue)._values);
			else
				_values.Add (value);

			return this;
		}

        public Value[] ToArray()
        {
            return _values.ToArray();
        }

		#endregion

		#region Static

		public static ArgumentsValue Create (Value left, Value right)
		{
			var a = new ArgumentsValue();
			a.Append(left).Append(right);
			return a;
		}

		#endregion

        #region Overrides

        public override string ToString(ParseContext context)
        {
            //TODO
            return base.ToString(context);
        }

        #endregion

        #region Implementation of ISupportsIndex

        public int GetDimension(int dimension)
        {
            if (dimension == 0)
                return Length;

            return 1;
        }

        public int Dimensions
        {
            get
            {
                return 1;
            }
        }

        public Value Get(int[] indices)
        {
            return this[indices[0]];
        }

        public void Set(int[] indices, Value value)
        {
            this[indices[0]] = value;
        }

        public ISupportsIndex Create(int[] dimensions)
        {
            return new ArgumentsValue();
        }

        #endregion
    }
}