using System;

namespace YAMP.Converter
{
    /// <summary>
    /// String to boolean (on, off) converter.
    /// </summary>
	public class StringToBooleanConverterAttribute : ValueConverterAttribute
    {
        /// <summary>
        /// Creates a new String To Bool Converter.
        /// </summary>
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
