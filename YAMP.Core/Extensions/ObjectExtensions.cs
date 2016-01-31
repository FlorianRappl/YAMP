namespace YAMP
{
    using System;
    using System.Collections.Generic;

    static class ObjectExtensions
    {
        public static Dictionary<String, Value> ToDictionary(this Object values)
        {
            var symbols = new Dictionary<String, Value>();

            if (values != null)
            {
                var props = values.GetType().GetProperties();

                foreach (var prop in props)
                {
                    var s = prop.GetValue(values, null);
                    var v = s.ToValue();

                    if (v == null)
                        throw new ArgumentException("Cannot execute YAMP queries with a list of values that contains types, which are not of a Value, numeric (int, double, float, long) or string (char, string) type.", "values");

                    symbols.Add(prop.Name, v);
                }
            }

            return symbols;
        }
        public static Value ToValue(this Object s)
        {
            if (s is Value)
            {
                return (Value)s;
            }
            else if (s is Double || s is Int32 || s is Single || s is Int64)
            {
                return new ScalarValue((Double)s);
            }
            else if (s is String || s is Char)
            {
                return new StringValue(s.ToString());
            }

            return null;
        }
    }
}
