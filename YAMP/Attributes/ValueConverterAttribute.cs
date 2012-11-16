using System;
using System.Linq;
using System.Reflection;

namespace YAMP
{
	public class StringToEnumConverter : ValueConverterAttribute
	{
		public StringToEnumConverter(Type enumType) : base(typeof(StringValue))
		{
			Converter = w =>
			{
				var str = (w as StringValue).Value;
			    var possibilites = enumType.GetFields(BindingFlags.Public | BindingFlags.Static).Select(fi => fi.Name).ToArray();

				foreach (var possibility in possibilites)
				{
					if(possibility.Equals(str, StringComparison.OrdinalIgnoreCase))
						return Enum.Parse(enumType, possibility, false);
				}

				throw new ArgumentValueException(str, possibilites);
			};
		}
	}

	public class MatrixToDoubleArrayConverterAttribute : ValueConverterAttribute
	{
		public MatrixToDoubleArrayConverterAttribute() : base(typeof(MatrixValue))
		{
			Converter = w =>
			{
				var v = w as MatrixValue;
				var m = new double[v.Length];

				for (var i = 0; i < m.Length; i++)
				{
					m[i] = v[i + 1].Value;
				}

				return m;
			};
		}
	}

	public class ScalarToBooleanConverterAttribute : ValueConverterAttribute
	{
		public ScalarToBooleanConverterAttribute() : base(typeof(ScalarValue), v => (v as ScalarValue).IntValue == 1.0)
		{
		}
	}

	public class ScalarToDoubleConverterAttribute : ValueConverterAttribute
	{
		public ScalarToDoubleConverterAttribute() : base(typeof(ScalarValue), v => (v as ScalarValue).Value)
		{
		}
	}

	public class StringToStringConverterAttribute : ValueConverterAttribute
	{
		public StringToStringConverterAttribute() : base(typeof(StringValue), v => (v as StringValue).Value)
		{
		}
	}

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
