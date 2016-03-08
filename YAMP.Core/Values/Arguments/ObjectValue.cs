namespace YAMP
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using YAMP.Exceptions;

    /// <summary>
    /// The class for representing an object value.
    /// </summary>
    public sealed class ObjectValue : Value, IFunction, ISetFunction
    {
        #region Fields

        static readonly String Intendation = "  ";

        readonly Dictionary<String, Value> _values;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public ObjectValue()
            : this(new Dictionary<String, Value>())
        {
        }

        /// <summary>
        /// Creates a new instance and sets the values.
        /// </summary>
        /// <param name="values">The initial values to use.</param>
        public ObjectValue(Object values)
            : this(values.ToDictionary())
        {
        }

        /// <summary>
        /// Creates a new instance and sets the values.
        /// </summary>
        /// <param name="values">The initial values to use.</param>
        public ObjectValue(Dictionary<String, Value> values)
        {
            _values = values;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the keys of the object.
        /// </summary>
        public IEnumerable<String> Keys
        {
            get { return _values.Keys; }
        }

        /// <summary>
        /// Gets the number of entries.
        /// </summary>
        public Int32 Length
        {
            get { return _values.Count; }
        }

        /// <summary>
        /// Gets or sets the value for the given key.
        /// </summary>
        public Value this[String key]
        {
            get { return _values[key]; }
            set { _values[key] = value; }
        }

        #endregion

        #region Serialization

        /// <summary>
        /// Returns a copy of this object value instance.
        /// </summary>
        /// <returns>The cloned object value.</returns>
        public override Value Copy()
        {
            var clone = new Dictionary<String, Value>(_values);
            return new ObjectValue(clone);
        }

        /// <summary>
        /// Converts the given value into binary data.
        /// </summary>
        /// <returns>The bytes array containing the data.</returns>
        public override Byte[] Serialize()
        {
            using (var ms = Serializer.Create())
            {
                ms.Serialize(Length);

                foreach (var element in _values)
                {
                    ms.Serialize(element.Key);
                    ms.Serialize(element.Value);
                }

                return ms.Value;
            }
        }

        /// <summary>
        /// Creates a new object value from the binary content.
        /// </summary>
        /// <param name="content">The data which contains the content.</param>
        /// <returns>The new instance.</returns>
        public override Value Deserialize(Byte[] content)
        {
            var dict = new Dictionary<String, Value>();

            using (var ds = Deserializer.Create(content))
            {
                var length = ds.GetInt();

                for (var i = 0; i < length; i++)
                {
                    var key = ds.GetString();
                    var value = ds.GetValue();
                    dict[key] = value;
                }
            }

            return new ObjectValue(dict);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The method used by YAMP to get a value from an object.
        /// </summary>
        /// <param name="context">The context where this is happening.</param>
        /// <param name="argument">The key of the value to retrieve.</param>
        /// <returns>The value that has been requested.</returns>
        public Value Perform(ParseContext context, Value argument)
        {
            var key = argument as StringValue;

            if (key == null)
            {
                throw new YAMPArgumentWrongTypeException(argument.Header, "String", String.Empty);
            }

            var name = key.Value;
            var value = default(Value);

            if (_values.TryGetValue(name, out value))
            {
                return value;
            }

            return new ObjectValue();
        }

        /// <summary>
        /// Method used by YAMP to set a value to an object.
        /// </summary>
        /// <param name="context">The context where this is happening.</param>
        /// <param name="argument">The key of the value to retrieve.</param>
        /// <param name="value">The value to set.</param>
        /// <returns>The value that has been set.</returns>
        public Value Perform(ParseContext context, Value argument, Value value)
        {
            var key = argument as StringValue;

            if (key == null)
            {
                throw new YAMPArgumentWrongTypeException(argument.Header, "String", String.Empty);
            }

            var name = key.Value;
            _values[name] = value;
            return value;
        }

        /// <summary>
        /// Returns the string content of this instance.
        /// </summary>
        /// <param name="context">The context of the invocation.</param>
        /// <returns>The value of the object.</returns>
        public override String ToString(ParseContext context)
        {
            var sb = new StringBuilder().AppendLine("{");

            foreach (var element in _values)
            {
                var name = element.Key;
                var value = Intend(element.Value.ToString(context));
                sb.Append(Intendation);
                sb.Append(name);
                sb.Append(" = ");
                sb.Append(value);
                sb.AppendLine(",");
            }

            return sb.Append('}').ToString();
        }

        #endregion

        #region Helpers

        static String Intend(String value)
        {
            var lines = value.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            return String.Join(Environment.NewLine + Intendation, lines);
        }

        #endregion
    }
}
