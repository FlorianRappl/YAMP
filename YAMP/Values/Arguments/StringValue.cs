using System;
using System.Text;

namespace YAMP
{
	public class StringValue : Value, ISupportsIndex
	{
		#region Members

		string _value;

		#endregion

		#region Properties

		public string Value
		{
			get { return _value; }
		}

		public int Length
		{
			get { return _value.Length; }
		}

		#endregion

		#region implemented abstract members of Value

		public override Value Add (Value right)
		{
			return new StringValue(_value + right.ToString());
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
			var content = Encoding.Unicode.GetBytes(_value);
			var length = BitConverter.GetBytes(content.Length);
			var value = new byte[length.Length + content.Length];
			length.CopyTo(value, 0);
			content.CopyTo(value, length.Length);
			return value;
		}

		public override Value Deserialize (byte[] content)
		{
			var length = BitConverter.ToInt32(content, 0);
			_value = Encoding.Unicode.GetString(content, 4, length);
			return this;
		}

		#endregion

		#region ctor

		public StringValue () : this(string.Empty)
		{
		}

		public StringValue (string value)
		{
			_value = value;
		}

		#endregion

		#region Overrides

		public override string ToString (ParseContext context)
		{
			return _value;
		}

		#endregion

        #region Implementation of ISupportsIndex

        public int Dimensions
        {
            get { return 1; }
        }

        public int GetDimension(int dimension)
        {
            if (dimension == 0)
                return Length;

            return 1;
        }

        public Value Get(int[] indices)
        {
            return new StringValue(_value[indices[0] - 1].ToString());
        }

        public void Set(int[] indices, Value value)
        {
            _value = _value.Remove(indices[0] - 1, 1).Insert(indices[0] - 1, value.ToString());
        }

        public ISupportsIndex Create(int[] dimensions)
        {
            return new StringValue();
        }

        #endregion
    }
}

