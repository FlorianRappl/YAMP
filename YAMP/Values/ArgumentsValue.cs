using System;
using System.Collections.Generic;

namespace YAMP
{
	public class ArgumentList : Value
	{
		#region Members

		List<Value> _values;

		#endregion

		#region implemented abstract members of Value

		public override Value Add (Value right)
		{
			return ArgumentList.Create(this, right);
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
				return _values[i];
			}
		}

		#endregion

		#region ctor

		public ArgumentList ()
		{
			_values = new List<Value>();
		}

		#endregion

		#region Methods

		ArgumentList Append (Value value)
		{
			if(value is ArgumentList)
				_values.AddRange((value as ArgumentList)._values);
			else
				_values.Add (value);

			return this;
		}

		#endregion

		#region Static

		public static ArgumentList Create (Value left, Value right)
		{
			var a = new ArgumentList();
			a.Append(left).Append(right);
			return a;
		}

		#endregion
	}
}