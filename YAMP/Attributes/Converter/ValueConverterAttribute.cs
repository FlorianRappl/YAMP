using System;
using System.Linq;
using System.Reflection;

namespace YAMP.Converter
{
	public abstract class ValueConverterAttribute : Attribute
	{
		public ValueConverterAttribute(Type expected, Func<Value, object> converter)
		{
			Converter = converter;
			Expected = expected;
		}

		public ValueConverterAttribute(Type expected)
		{
			Expected = expected;
		}

		public Func<Value, object> Converter { get; set; }

		public object Convert(Value argument)
		{
			return Converter.Invoke(argument);
		}

		public bool CanConvertFrom(Value argument)
		{
			return Expected.IsInstanceOfType(argument);
		}

		public Type Expected
		{
			get;
			set;
		}

		public string Type
		{
			get { return Expected.Name.RemoveValueConvention(); }
		}
	}
}
