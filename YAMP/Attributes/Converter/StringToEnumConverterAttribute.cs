using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace YAMP.Converter
{
    /// <summary>
    /// String to an arbitrary enumeration converter.
    /// </summary>
    public class StringToEnumConverter : ValueConverterAttribute
    {
        /// <summary>
        /// Creates a new String to Enumeration (value) converter.
        /// </summary>
        /// <param name="enumType">The enumeration which contains the possible values.</param>
        public StringToEnumConverter(Type enumType) : base(typeof(StringValue))
        {
            Converter = w =>
            {
                var str = (w as StringValue).Value;

                try
                {
                    return Enum.Parse(enumType, str, true);
                }
                catch
                {
                    var possibilites = enumType.GetFields(BindingFlags.Public | BindingFlags.Static).Select(fi => fi.Name).ToArray();
                    throw new YAMPArgumentValueException(str, possibilites);
                }
            };
        }
    }
}
