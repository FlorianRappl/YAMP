using System;

namespace YAMP.Converter
{
	public class StringToBooleanConverterAttribute : ValueConverterAttribute
	{
		public StringToBooleanConverterAttribute()
			: base(typeof(StringValue), v => {
				var str = (v as StringValue).Value.ToLower();

				if (str == "on")
					return true;
				else if (str == "off")
					return false;

				throw new ArgumentException("[ on, off ]", str);
			})
		{
		}
	}
}
