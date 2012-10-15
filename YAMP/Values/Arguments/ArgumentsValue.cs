using System;
using System.Text;
using System.Collections.Generic;

namespace YAMP
{
	public class ArgumentsValue : Value
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
			var ms = new System.IO.MemoryStream();
			var len = BitConverter.GetBytes(_values.Count);
			ms.Write(len, 0, len.Length);

			foreach(var value in _values)
			{
				var idx = Encoding.ASCII.GetBytes(value.Header);
				len = BitConverter.GetBytes(idx.Length);
				ms.Write(len, 0, len.Length);
				ms.Write(idx, 0, idx.Length);
				var entry = value.Serialize();
				len = BitConverter.GetBytes(entry.Length);
				ms.Write(len, 0, len.Length);
				ms.Write(entry, 0, entry.Length);
			}

			var content = ms.ToArray();
			ms.Close();
			ms.Dispose();
			return content;
		}

		public override Value Deserialize (byte[] content)
		{
			var ms = new System.IO.MemoryStream(content);
			var buffer = new byte[4];
			ms.Read(buffer, 0, buffer.Length);
			var count = BitConverter.ToInt32 (buffer, 0);

			for(var i = 0; i < count; i++)
			{
				ms.Read(buffer, 0, buffer.Length);
				var length = BitConverter.ToInt32 (buffer, 0);
				var stringBuffer = new byte[length];
				ms.Read(stringBuffer, 0, stringBuffer.Length);
				var name = Encoding.ASCII.GetString(stringBuffer);
				ms.Read(buffer, 0, buffer.Length);
				length = BitConverter.ToInt32 (buffer, 0);
				var contentBuffer = new byte[length];
				ms.Read(contentBuffer, 0, contentBuffer.Length);
				_values.Add(Value.Deserialize(name, contentBuffer));
			}

			ms.Close();
			ms.Dispose();
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
    }
}