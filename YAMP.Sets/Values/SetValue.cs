namespace YAMP.Sets
{
    using System;
    using System.Collections.Generic;
    using YAMP.Converter;
    using YAMP;
    using YAMP.Exceptions;
    using System.Text;
    using System.IO;

    using ISetValueDictionary = System.Collections.Generic.ISet<SetValue.ValueDictionary>;

    //SortedSet is NOT implemented in Silverlight. Borrowed implem from CoreFX.
    using SortedSetValueDictionary = YAMPSystem.Collections.Generic.SortedSet<SetValue.ValueDictionary>;
    using HashSetValueDictionary = System.Collections.Generic.HashSet<SetValue.ValueDictionary>;

    using BaseType = YAMP.Value;

    /// <summary>
    /// The SetValue class.
    /// </summary>
    public sealed class SetValue : Value //, IFunction, ISetFunction
    {
        #region ctor

        /// <summary>
        /// Creates a new SetValue instance.
        /// </summary>
        public SetValue()
        {
            Name = String.Empty;
        }

        /// <summary>
        /// Creates a new instance with a name.
        /// </summary>
        /// <param name="name">The name of the set.</param>
        public SetValue(string name)
        {
            this.Name = name;
            this._set = new HashSetValueDictionary();
        }

        /// <summary>
        /// Creates a new instance and sets the values.
        /// </summary>
        /// <param name="name">The name ot the set.</param>
        /// <param name="set">The initial values to use.</param>
        public SetValue(string name, ISetValueDictionary set)
            : this(name, set, false)
        {
        }

        /// <summary>
        /// Creates a new instance and sets the values.
        /// </summary>
        /// <param name="name">The name ot the set.</param>
        /// <param name="set">The initial values to use.</param>
        public SetValue(string name, ISetValueDictionary set, bool ordered)
        {
            this._set = CreateSet(set, ordered);
            this.Name = name;
        }

        private ISetValueDictionary CreateSet(ISetValueDictionary source, bool ordered)
        {
            if (source == null)
                return ordered ? (ISetValueDictionary)new SortedSetValueDictionary() : (ISetValueDictionary)new HashSetValueDictionary();

            return ordered ? (ISetValueDictionary)new SortedSetValueDictionary(source) : (ISetValueDictionary)new HashSetValueDictionary(source);
        }

        #endregion

        readonly IDictionary<String, IFunction> _functions = new Dictionary<String, IFunction>(StringComparer.OrdinalIgnoreCase);

        #region Fields

        static readonly String Indentation = "  ";

        readonly ISetValueDictionary _set;
        string _name = string.Empty;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the HashSet name.
        /// </summary>
        [StringToStringConverter]
        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public bool Sorted
        {
            get { return _set is SortedSetValueDictionary; }
            private set { }
        }

        public ISetValueDictionary Set
        {
            get { return _set; }
            private set { }
        }


        #endregion


        #region Serialization

        /// <summary>
        /// Returns a copy of this object value instance.
        /// </summary>
        /// <returns>The cloned object value.</returns>
        public override Value Copy()
        {
            return new SetValue(Name, _set, Sorted);
        }

        /// <summary>
        /// Transforms the instance into a binary representation.
        /// </summary>
        /// <returns>The binary representation.</returns>
        public override Byte[] Serialize()
        {
            var mem = new MemoryStream();
            SerializeString(mem, Name);
            SerializeString(mem, Sorted ? "O" : "U");
            SerializeValueDictionary(mem, _set);

            return mem.ToArray();
        }

        /// <summary>
        /// Transforms a binary representation into a new instance.
        /// </summary>
        /// <param name="content">The binary data.</param>
        /// <returns>The new instance.</returns>
        public override Value Deserialize(Byte[] content)
        {
            var mem = new MemoryStream(content);
            mem.Position = 0;

            string name = DeserializeString(mem);
            string setType = DeserializeString(mem);
            var unit = DeserializeSet(mem, name, setType == "O");
            return unit;
        }

        #endregion


        #region Methods

        /// <summary>
        /// Registers the member operator.
        /// </summary>
        protected override void RegisterOperators()
        {
            RegisterMember(typeof(SetValue), typeof(StringValue), MethodNameCall);
        }

        public static Value MethodNameCall(Value left, Value right)
        {
            MemberFunction fn = null;
            var fnName = (right as StringValue).Value.ToLowerInvariant();
            switch (fnName)
            {
                case "setcount":
                    fn = new SetValue.CountFunction();
                    break;
                case "setunion":
                    fn = new SetValue.UnionFunction();
                    break;
                default:
                    throw new YAMPSetsFunctionMissingException(fnName, "SetValue");
            }
            if (fn!=null)
            {
                fn.@this = left;
                return new FunctionValue(fn);
            }

            return SetValue.Empty;
        }


        /// <summary>
        /// Returns the string content of this instance.
        /// </summary>
        /// <param name="context">The context of the invocation.</param>
        /// <returns>The value of the object.</returns>
        public override String ToString(ParseContext context)
        {
            var sb = new StringBuilder();

            sb.AppendLine("{");
            sb.AppendLine(string.Format("Name = {0}, Sorted = {1}", Name, Sorted));

            foreach (var element in _set)
            {
                //sb.AppendLine();
                sb.Append(element.ToString(context));
            }

            sb.Append("}");
            return sb.ToString();
        }

        #endregion

        #region Helpers

        static String Indent(String value)
        {
            var lines = value.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            return String.Join(Environment.NewLine + Indentation, lines);
        }

        public static void SerializeString(MemoryStream mem, String value)
        {
            var bytes = Encoding.Unicode.GetBytes(value);
            var length = BitConverter.GetBytes(bytes.Length);
            mem.Write(length, 0, length.Length);
            mem.Write(bytes, 0, bytes.Length);
        }

        private void SerializeValueDictionary(MemoryStream mem, ISetValueDictionary set)
        {
            var bytes = BitConverter.GetBytes(set.Count);
            mem.Write(bytes, 0, bytes.Length);

            foreach (var setItem in set)
            {
                setItem.Serialize(mem);
            }
        }

        public static string DeserializeString(MemoryStream mem)
        {
            var buffer = new Byte[4];
            mem.Read(buffer, 0, buffer.Length);
            var length = BitConverter.ToInt32(buffer, 0);
            buffer = new Byte[length];
            mem.Read(buffer, 0, buffer.Length);
            return Encoding.Unicode.GetString(buffer, 0, buffer.Length);
        }

        private SetValue DeserializeSet(MemoryStream mem, string name, bool ordered)
        {
            var buffer = new Byte[sizeof(int)];
            mem.Read(buffer, 0, buffer.Length);
            var count = BitConverter.ToInt32(buffer, 0);

            SetValue ret = new SetValue(name, null, ordered);
            var set = ret._set;

            for (int i = 0; i < count; i++)
            {
                ValueDictionary item = new ValueDictionary();
                item.Deserialize(mem);

                set.Add(item);
            }

            return ret;
        }

        #endregion


        #region Nested types

        /// <summary>
        /// Represents one Entry in the Set.
        /// </summary>
        public struct ValueDictionary : IEquatable<ValueDictionary>, IComparable<ValueDictionary>
        {
            //The Fields Dictionary is completely ignored from all the Hash / Equals / ==...
            //We will only consider ID for that matter.

            private BaseType _id;
            private Dictionary<string, Value> _fields;

            public ValueDictionary(BaseType id)
            {
                _id = id;
                _fields = new Dictionary<string, Value>();
            }

            public static implicit operator ValueDictionary(Value id)  // explicit BaseType to ValueDictionary conversion operator
            {
                return new ValueDictionary(id);
            }

            public BaseType ID
            {
                get { return _id; }
                private set { }
            }

            public Dictionary<String, Value> Fields
            {
                get { return _fields == null ? _fields = new Dictionary<string, Value>() : _fields; }
                private set { }
            }

            internal void SerializeID(MemoryStream mem, BaseType id)
            {
                SerializeValue(mem, id);
            }

            internal void SerializeValue(MemoryStream mem, Value val)
            {
                SetValue.SerializeString(mem, val.Header);

                var bytes = val.Serialize();
                var bytesLen = BitConverter.GetBytes(bytes.Length);
                mem.Write(bytesLen, 0, bytesLen.Length);
                mem.Write(bytes, 0, bytes.Length);
            }

            internal void Serialize(MemoryStream mem)
            {
                SerializeID(mem, _id);

                var bytes = BitConverter.GetBytes(_fields.Count);
                mem.Write(bytes, 0, bytes.Length);

                foreach (var field in _fields)
                {
                    SetValue.SerializeString(mem, field.Key);
                    SerializeValue(mem, field.Value);
                }
            }

            internal void DeserializeID(MemoryStream mem)
            {
                _id = DeserializeValue(mem);
            }

            internal Value DeserializeValue(MemoryStream mem)
            {
                string header = SetValue.DeserializeString(mem);

                var buffer = new Byte[sizeof(int)];
                mem.Read(buffer, 0, buffer.Length);
                var len = BitConverter.ToInt32(buffer, 0);

                buffer = new Byte[len];
                mem.Read(buffer, 0, buffer.Length);

                Value val = Value.Deserialize(header, buffer);
                return val;
            }


            internal void Deserialize(MemoryStream mem)
            {
                DeserializeID(mem);

                var buffer = new Byte[sizeof(int)];
                mem.Read(buffer, 0, buffer.Length);
                var count = BitConverter.ToInt32(buffer, 0);

                _fields = new Dictionary<string, Value>(count);

                for (int i = 0; i < count; i++)
                {
                    string key = SetValue.DeserializeString(mem);

                    Value val = DeserializeValue(mem);

                    _fields[key] = val;
                }
            }

            public override string ToString()
            {
                return ToString(new ParseContext());
            }


            internal string ToString(ParseContext context)
            {
                var sb = new StringBuilder();

                sb.Append(Indentation);
                sb.AppendLine(string.Format("ID = {0} {{", _id));

                foreach (var element in _fields)
                {
                    var name = element.Key;
                    var value = Indent(element.Value.ToString(context));

                    sb.Append(Indentation).Append(Indentation);
                    sb.Append(name);
                    sb.Append(" = ");
                    sb.Append(value);
                    sb.AppendLine(",");
                }

                sb.Append(Indentation).AppendLine("}");

                return sb.ToString();
            }

            public override int GetHashCode()
            {
                int hash = _id.GetHashCode();
                return hash;
            }

            public static bool operator ==(ValueDictionary x, ValueDictionary y)
            {
                return x._id == y._id;
            }

            public static bool operator !=(ValueDictionary x, ValueDictionary y)
            {
                return !(x == y);
            }

            public override bool Equals(object obj)
            {
                return obj is ValueDictionary && this == (ValueDictionary)obj;
            }

            public bool Equals(ValueDictionary other)
            {
                bool eq = _id.Equals(other._id);

                return eq;
            }

            public int CompareTo(ValueDictionary other)
            {
                Value left = this._id;
                Value right = other._id;

                if (left == null && right == null) return 0;
                if (right == null) return 1;
                if (left == null) return -1;

                if (left is ScalarValue && right is ScalarValue)
                    return (left as ScalarValue).Value.CompareTo((right as ScalarValue).Value);

                if (left is StringValue && right is StringValue)
                    return (left as StringValue).Value.CompareTo((right as StringValue).Value);

                //ScalarValues will always "preceed" strings...
                if (left is ScalarValue && right is StringValue)
                    return -1;
                if (left is StringValue && right is ScalarValue)
                    return 1;

                //Other types cannot be compared...
                throw new ArgumentException("OrderedSet Element is not ScalarValue neither StringValue");
            }
        }
        #endregion


        #region MemberFunctions

        [Description("The Union Object function.")]
        [Kind(PopularKinds.Function)]
        sealed class UnionFunction : MemberFunction
        {
            [Description("Create a new Set, with the Union of Sets")]
            [Arguments(0, 0)]
            public SetValue Function(ArgumentsValue args)
            {
                SetValue set = @this as SetValue;
                if (set == null)
                    throw new YAMPArgumentValueException(@this.Header, new string[] { "Set" });

                var fn = new YAMP.Sets.UnionFunction();
                return fn.Function(set, args);
            }

        }

        [Description("The Count Object function.")]
        [Kind(PopularKinds.Function)]
        sealed class CountFunction : MemberFunction
        {
            [Description("Get the number of elements in the Set")]
            public ScalarValue Function()
            {
                SetValue set1 = @this as SetValue;
                if (set1 == null)
                    throw new YAMPArgumentValueException(@this.Header, new string[] { "Set" });

                return new ScalarValue(set1.Set.Count);
            }
        }

        #endregion

    }
}
