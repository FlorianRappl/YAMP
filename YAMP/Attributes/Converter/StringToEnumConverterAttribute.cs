using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace YAMP.Converter
{
    public class StringToEnumConverter : ValueConverterAttribute
    {
        public StringToEnumConverter(Type enumType)
            : base(typeof(StringValue))
        {
            Converter = w =>
            {
                var str = (w as StringValue).Value;
                try
                {
                    return Enum.Parse(enumType, str, true);
                }
                catch (ArgumentException)
                {
                    var possibilites =
                        enumType.GetFields(BindingFlags.Public | BindingFlags.Static).Select(fi => fi.Name).ToArray();
                    throw new ArgumentValueException(str, possibilites);
                }
            };
        }
    }
}
