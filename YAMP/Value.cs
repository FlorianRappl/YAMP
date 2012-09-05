using System;

namespace YAMP
{
	public abstract class Value
	{
		static readonly Value _empty = new ScalarValue();
		
		public static Value Empty
		{
			get
			{
				return _empty;
			}
		}
		
		public abstract Value Add(Value right);
		
		public abstract Value Subtract(Value right);
		
		public abstract Value Multiply(Value right);
		
		public abstract Value Divide(Value denominator);
		
		public abstract Value Power(Value exponent);
	}
}

