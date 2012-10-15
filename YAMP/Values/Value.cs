using System;
using System.Reflection;

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

		public string Header
		{
			get
			{
				return GetType().Name.RemoveValueConvention();
			}
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
					var value = target.GetConstructor(Type.EmptyTypes).Invoke(null) as Value;
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
	}
}

