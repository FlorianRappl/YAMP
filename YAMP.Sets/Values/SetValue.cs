namespace YAMP.Sets
{
    using System;
    using System.Collections.Generic;
    using YAMP.Converter;
    using YAMP;
    using YAMP.Exceptions;
    using System.Text;
    using System.IO;

    using ISetValueDictionary = System.Collections.Generic.ISet<SetValue.ValueWrap>;

    //SortedSet is NOT implemented in Silverlight. Borrowed implementation from CoreFX.
    using SortedSetValueWrap = YAMPSystem.Collections.Generic.SortedSet<SetValue.ValueWrap>;
    using HashSetValueWrap = System.Collections.Generic.HashSet<SetValue.ValueWrap>;

    using BaseType = YAMP.Value;

    /// <summary>
    /// The SetValue class.
    /// </summary>
    public sealed partial class SetValue : Value, IEquatable<SetValue> //, IFunction, ISetFunction
    {
        #region ctor

        /// <summary>
        /// Creates a new SetValue instance.
        /// </summary>
        public SetValue()
            : this(String.Empty, null, false)
        {
        }

        /// <summary>
        /// Creates a new unsorted instance with a name.
        /// </summary>
        /// <param name="name">The name of the set.</param>
        public SetValue(string name)
            : this(name, null, false)
        {
        }

        /// <summary>
        /// Creates a new instance with a name.
        /// </summary>
        /// <param name="name">The name of the set.</param>
        public SetValue(string name, bool ordered)
            : this(name, null, ordered)
        {
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
        /// <param name="set">Ordered or Unordered set.</param>
        public SetValue(string name, ISetValueDictionary set, bool ordered)
        {
            this._set = CreateSet(ordered, set);
            this.Name = name;
            this.ordered = ordered;
        }

        /// <summary>
        /// Creates a new instance and sets the values.
        /// </summary>
        /// <param name="name">The name ot the set.</param>
        /// <param name="set">The initial values to use.</param>
        /// <param name="set">Ordered or Unordered set.</param>
        public SetValue(string name, bool ordered, ScalarValue[] scalarValues) : this(name)
        {
            this._set = CreateSet(ordered, null);
            this.Name = name;
            this.ordered = ordered;

            for (int i = 0; i < scalarValues.Length; i++)
            {
                _set.Add(new ValueWrap(scalarValues[i]));
            }
        }

        private ISetValueDictionary CreateSet(bool ordered)
        {
            return CreateSet(ordered, null);
        }

        private static ISetValueDictionary CreateSet(bool ordered, ISetValueDictionary source)
        {
            if (source == null)
                return ordered ? (ISetValueDictionary)new SortedSetValueWrap() : (ISetValueDictionary)new HashSetValueWrap();

            return ordered ? (ISetValueDictionary)new SortedSetValueWrap(source) : (ISetValueDictionary)new HashSetValueWrap(source);
        }

        #endregion

        #region Fields

        static readonly String Indentation = "  ";

        readonly ISetValueDictionary _set;
        string _name = string.Empty;
        bool ordered = false;

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
            get { return ordered; }
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
            Register.BinaryOperator(OpDefinitionsSet.EqOperator, typeof(SetValue), typeof(SetValue),
                (left, right) => new ScalarValue(((SetValue)left) == ((SetValue)right)));

            Register.BinaryOperator(OpDefinitionsSet.StandardNeqOperator, typeof(SetValue), typeof(SetValue),
                (left, right) => new ScalarValue(((SetValue)left) != ((SetValue)right)));
            Register.BinaryOperator(OpDefinitionsSet.AliasNeqOperator, typeof(SetValue), typeof(SetValue),
                (left, right) => new ScalarValue(((SetValue)left) != ((SetValue)right)));

            Register.BinaryOperator(OpDefinitionsSet.UnionOperator, typeof(SetValue), typeof(SetValue),
                (left, right) => SetValue.TUnion(((SetValue)left), new ArgumentsValue(right)));

            Register.BinaryOperator(OpDefinitionsSet.IntersectOperator, typeof(SetValue), typeof(SetValue),
                (left, right) => SetValue.TIntersect(((SetValue)left), new ArgumentsValue(right)));

            Register.BinaryOperator(OpDefinitionsSet.ExceptOperator, typeof(SetValue), typeof(SetValue),
                (left, right) => SetValue.TExcept(((SetValue)left), new ArgumentsValue(right)));

            Register.BinaryOperator(OpDefinitionsSet.ExceptXorOperator, typeof(SetValue), typeof(SetValue),
                (left, right) => SetValue.TExceptXor(((SetValue)left), new ArgumentsValue(right)));
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
            sb.AppendLine(string.Format("Name = {0}, Sorted = {1}, Count = {2}", Name, Sorted, _set.Count));

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
                ValueWrap item = new ValueWrap();
                item.Deserialize(mem);

                set.Add(item);
            }

            return ret;
        }

        public override bool Equals(object obj)
        {
            return obj is SetValue && Equals((SetValue)obj);
        }

        /// <summary>
        /// Equality.
        /// </summary>
        /// <param name="l">Set l</param>
        /// <param name="r">Set r</param>
        /// <returns>l == r</returns>
        public static Boolean operator ==(SetValue l, SetValue r)
        {
            if (ReferenceEquals(l, r))
            {
                return true;
            }

            if (ReferenceEquals(l, null) || ReferenceEquals(r, null))
            {
                return false;
            }

            return l.Equals(r);
        }

        /// <summary>
        /// Inequality.
        /// </summary>
        /// <param name="l">Set l</param>
        /// <param name="r">Set r</param>
        /// <returns>l != r</returns>
        public static Boolean operator !=(SetValue l, SetValue r)
        {
            return !(l == r);
        }

        /// <summary>
        /// Equals implementation. Only care on Set's content
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(SetValue other)
        {
            if (ReferenceEquals(other, null))
                return false;

            bool eq = this.Set.SetEquals(other.Set);

            return eq;
        }
        public override int GetHashCode()
        {
            int hash = this.Set.GetHashCode();
            return hash;
        }

        /// <summary>
        /// Adds elements to a set
        /// </summary>
        /// <param name="scalarValues">The array of elements to add</param>
        /// <returns>Number of elements that didn't existed and were added</returns>
        public int AddElements(ScalarValue[] scalarValues)
        {
            int added = 0;
            for (int i = 0; i < scalarValues.Length; i++)
            {
                if (_set.Add(new ValueWrap(scalarValues[i])))
                    added++;
            }
            return added;
        }

        /// <summary>
        /// Removes elements from a set
        /// </summary>
        /// <param name="scalarValues">The array of elements to remove</param>
        /// <returns>Number of elements that didn't existed and were removed</returns>
        public int RemoveElements(ScalarValue[] scalarValues)
        {
            int removed = 0;
            for (int i = 0; i < scalarValues.Length; i++)
            {
                if (_set.Remove(new ValueWrap(scalarValues[i])))
                    removed++;
            }
            return removed;
        }

        #endregion


        #region Nested types

        /// <summary>
        /// Represents one Entry in the Set.
        /// </summary>
        public struct ValueWrap : IEquatable<ValueWrap>, IComparable<ValueWrap>
        {
            private BaseType _id;

            public ValueWrap(BaseType id)
            {
                _id = id;
            }

            public static implicit operator ValueWrap(NumericValue id)  // explicit NumericValue to ValueWrap conversion operator
            {
                return new ValueWrap(id);
            }

            public static implicit operator ValueWrap(Int64 id)  // explicit Int64 to ValueWrap conversion operator
            {
                return new ValueWrap(new ScalarValue(id));
            }

            public static implicit operator ValueWrap(String id)  // explicit String to ValueWrap conversion operator
            {
                return new ValueWrap(new StringValue(id));
            }

            public BaseType ID
            {
                get { return _id; }
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
            }

            public override string ToString()
            {
                return ToString(new ParseContext());
            }


            internal string ToString(ParseContext context)
            {
                var sb = new StringBuilder();

                sb.Append(Indentation);
                sb.AppendLine(string.Format("ID = {0}", _id));

                return sb.ToString();
            }

            public override int GetHashCode()
            {
                int hash = _id.GetHashCode();
                return hash;
            }

            public static bool operator ==(ValueWrap x, ValueWrap y)
            {
                return x._id == y._id;
            }

            public static bool operator !=(ValueWrap x, ValueWrap y)
            {
                return !(x == y);
            }

            public override bool Equals(object obj)
            {
                return obj is ValueWrap && Equals((ValueWrap)obj);
            }

            public bool Equals(ValueWrap other)
            {
                bool eq = _id.Equals(other._id);

                return eq;
            }

            public int CompareTo(ValueWrap other)
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


        #region MemberFunctionsHelpers

        /// <summary>
        /// Create a new Set, with the Union of Sets
        /// </summary>
        /// <param name="set">The set</param>
        /// <param name="args">Sets to Union</param>
        /// <returns>The new set</returns>
        internal static SetValue TUnion(SetValue set, ArgumentsValue args)
        {
            string newName = string.Format("({0}", set.Name);

            var newSet = set.Copy() as SetValue;

            int iArgs = 1; //First is "name"
            foreach (var arg in args)
            {
                iArgs++;
                SetValue otherSet = arg as SetValue;
                newName += string.Format("+{0}", otherSet.Name);
                if (!ReferenceEquals(otherSet, null))
                    newSet.Set.UnionWith(otherSet.Set);
                else
                    throw new YAMPArgumentInvalidException("Element is not SetValue", iArgs);
            }
            newName += ")";
            newSet.Name = newName;

            return newSet;
        }

        /// <summary>
        /// Create a new Set, with the Intersection of Sets
        /// </summary>
        /// <param name="set">The set</param>
        /// <param name="args">Sets to Intersect</param>
        /// <returns>The new set</returns>
        internal static SetValue TIntersect(SetValue set, ArgumentsValue args)
        {
            string newName = string.Format("({0}", set.Name);

            var newSet = set.Copy() as SetValue;

            int iArgs = 1; //First is "name"
            foreach (var arg in args)
            {
                iArgs++;
                SetValue otherSet = arg as SetValue;
                newName += string.Format("&{0}", otherSet.Name);
                if (!ReferenceEquals(otherSet, null))
                    newSet.Set.IntersectWith(otherSet.Set);
                else
                    throw new YAMPArgumentInvalidException("Element is not SetValue", iArgs);
            }
            newName += ")";
            newSet.Name = newName;

            return newSet;
        }

        /// <summary>
        /// Create a new Set, with the first Set Except(ed) of the other Sets
        /// </summary>
        /// <param name="set">The set</param>
        /// <param name="args">Sets to Except</param>
        /// <returns>The new set</returns>
        internal static SetValue TExcept(SetValue set, ArgumentsValue args)
        {
            string newName = string.Format("({0}", set.Name);

            var newSet = set.Copy() as SetValue;

            int iArgs = 1; //First is "name"
            foreach (var arg in args)
            {
                iArgs++;
                SetValue otherSet = arg as SetValue;
                newName += string.Format("-{0}", otherSet.Name);
                if (!ReferenceEquals(otherSet, null))
                    newSet.Set.ExceptWith(otherSet.Set);
                else
                    throw new YAMPArgumentInvalidException("Element is not SetValue", iArgs);
            }
            newName += ")";
            newSet.Name = newName;

            return newSet;
        }

        /// <summary>
        /// Create a new Set, with the Symmetric Except(XOR) of all Sets
        /// </summary>
        /// <param name="set">The set</param>
        /// <param name="args">Sets to Except(XOR)</param>
        /// <returns>The new set</returns>
        internal static SetValue TExceptXor(SetValue set, ArgumentsValue args)
        {
            string newName = string.Format("({0}", set.Name);

            var newSet = set.Copy() as SetValue;

            int iArgs = 1; //First is "name"
            foreach (var arg in args)
            {
                iArgs++;
                SetValue otherSet = arg as SetValue;
                newName += string.Format("^{0}", otherSet.Name);
                if (!ReferenceEquals(otherSet, null))
                    newSet.Set.SymmetricExceptWith(otherSet.Set);
                else
                    throw new YAMPArgumentInvalidException("Element is not SetValue", iArgs);
            }
            newName += ")";
            newSet.Name = newName;

            return newSet;
        }

        /// <summary>
        /// Get the number of elements in the Set
        /// </summary>
        /// <param name="set"></param>
        /// <returns>Number of elements in the set</returns>
        internal static ScalarValue TCount(SetValue set)
        {
            if (ReferenceEquals(set, null))
                throw new YAMPArgumentValueException("null", new string[] { "Set" });

            return new ScalarValue(set.Set.Count);
        }

        /// <summary>
        /// Compares two sets
        /// </summary>
        /// <param name="set"></param>
        /// <returns></returns>
        internal static ScalarValue TEquals(SetValue set1, SetValue set2)
        {
            //They should never be null
            if (ReferenceEquals(set1, null) || ReferenceEquals(set2, null))
                return new ScalarValue(false);

            return new ScalarValue(set1.Set.SetEquals(set2.Set));
        }

        public MatrixValue ToMatrix()
        {
            return TToMatrix(this);
        }

        internal static MatrixValue TToMatrix(SetValue set)
        {
            List<ScalarValue> values = new List<ScalarValue>();
            foreach (var el in set.Set)
            {
                if (el.ID is ScalarValue)
                    values.Add(el.ID as ScalarValue);
            }

            MatrixValue matrix = new MatrixValue(values.ToArray(), 1, values.Count);
            return matrix;
        }

        #endregion


        #region MemberFunctions

        [Description("The SetAdd Object function.")]
        [Kind(PopularKinds.Function)]
        sealed class AddFunction : MemberFunction
        {
            [Description("Adds the element to the Set, and returns the set. If Matrix, all its elements will be added")]
            public SetValue Function(Value id)
            {
                SetValue set = @this as SetValue;
                if (ReferenceEquals(set, null))
                    throw new YAMPSetsFunctionNotMemberException("SetAdd");

                if (id is MatrixValue)
                {
                    set.AddElements((id as MatrixValue).ToArray());
                }
                else
                {
                    set.Set.Add(new SetValue.ValueWrap(id));
                }
                return set;
            }
        }

        [Description("The SetAsSort Object function.")]
        [Kind(PopularKinds.Function)]
        sealed class AsSortFunction : MemberFunction
        {
            [Description("Creates a copied sorted Set")]
            public SetValue Function()
            {
                SetValue set = @this as SetValue;
                if (ReferenceEquals(set, null))
                    throw new YAMPSetsFunctionNotMemberException("SetAsSort");

                var newSet = new SetValue(set.Name, set.Set, true);

                return newSet;
            }
        }

        [Description("The SetAsUnsort Object function.")]
        [Kind(PopularKinds.Function)]
        sealed class AsUnsortFunction : MemberFunction
        {
            [Description("Creates a copied unsorted Set")]
            public SetValue Function()
            {
                SetValue set = @this as SetValue;
                if (ReferenceEquals(set, null))
                    throw new YAMPSetsFunctionNotMemberException("SetAsUnsort");

                var newSet = new SetValue(set.Name, set.Set, false);

                return newSet;
            }
        }

        [Description("The SetContains Object function.")]
        [Kind(PopularKinds.Function)]
        sealed class ContainsFunction : MemberFunction
        {
            [Description("Determines whether the set contains the given element's id")]
            public ScalarValue Function(Value id)
            {
                SetValue set = @this as SetValue;
                if (ReferenceEquals(set, null))
                    throw new YAMPSetsFunctionNotMemberException("SetContains");

                bool eq = set.Set.Contains(new SetValue.ValueWrap(id));
                return new ScalarValue(eq);
            }
        }

        [Description("The SetCount Object function.")]
        [Kind(PopularKinds.Function)]
        sealed class CountFunction : MemberFunction //, IObjectFunctions
        {
            [Description("Get the number of elements in the Set")]
            public ScalarValue Function()
            {
                SetValue set = @this as SetValue;
                if (ReferenceEquals(set, null))
                    throw new YAMPSetsFunctionNotMemberException("SetCount");

                return new ScalarValue(set.Set.Count);
            }
        }

        [Description("The SetEquals Object function.")]
        [Kind(PopularKinds.Function)]
        sealed class EqualsFunction : MemberFunction
        {
            [Description("Compares two sets")]
            public ScalarValue Function(SetValue set2)
            {
                SetValue set = @this as SetValue;
                if (ReferenceEquals(set, null))
                    throw new YAMPSetsFunctionNotMemberException("SetEquals");

                bool eq = set.Set.SetEquals(set2.Set);
                return new ScalarValue(eq);
            }
        }

        [Description("The SetExcept Object function.")]
        [Kind(PopularKinds.Function)]
        sealed class ExceptFunction : MemberFunction
        {
            [Description("Create a new Set, with the first Set Except(ed) of the other Sets")]
            [Arguments(0, 0)]
            public SetValue Function(ArgumentsValue args)
            {
                SetValue set = @this as SetValue;
                if (ReferenceEquals(set, null))
                    throw new YAMPSetsFunctionNotMemberException("SetExcept");

                return SetValue.TExcept(set, args);
            }
        }

        [Description("The SetExceptXor Object function.")]
        [Kind(PopularKinds.Function)]
        sealed class ExceptXorFunction : MemberFunction
        {
            [Description("Create a new Set, with the Symmetric Except(XOR) of all Sets")]
            [Arguments(0, 0)]
            public SetValue Function(ArgumentsValue args)
            {
                SetValue set = @this as SetValue;
                if (ReferenceEquals(set, null))
                    throw new YAMPSetsFunctionNotMemberException("SetExceptXor");

                return SetValue.TExceptXor(set, args);
            }
        }

        [Description("The SetIntersect Object function.")]
        [Kind(PopularKinds.Function)]
        sealed class IntersectFunction : MemberFunction
        {
            [Description("Create a new Set, with the Intersection of Sets")]
            [Arguments(0, 0)]
            public SetValue Function(ArgumentsValue args)
            {
                SetValue set = @this as SetValue;
                if (ReferenceEquals(set, null))
                    throw new YAMPSetsFunctionNotMemberException("SetIntersect");

                return SetValue.TIntersect(set, args);
            }
        }

        [Description("The SetIsProperSubsetOf Object function.")]
        [Kind(PopularKinds.Function)]
        sealed class IsProperSubsetOfFunction : MemberFunction
        {
            [Description("Determines whether set1 is a proper subset of set2")]
            public ScalarValue Function(SetValue set2)
            {
                SetValue set = @this as SetValue;
                if (ReferenceEquals(set, null))
                    throw new YAMPSetsFunctionNotMemberException("SetIsProperSubsetOf");

                bool eq = set.Set.IsProperSubsetOf(set2.Set);
                return new ScalarValue(eq);
            }
        }

        [Description("The SetIsProperSupersetOf Object function.")]
        [Kind(PopularKinds.Function)]
        sealed class IsProperSupersetOfFunction : MemberFunction
        {
            [Description("Determines whether set1 is a proper superset of set2")]
            public ScalarValue Function(SetValue set2)
            {
                SetValue set = @this as SetValue;
                if (ReferenceEquals(set, null))
                    throw new YAMPSetsFunctionNotMemberException("SetIsProperSupersetOf");

                bool eq = set.Set.IsProperSupersetOf(set2.Set);
                return new ScalarValue(eq);
            }
        }

        [Description("The SetIsSorted Object function.")]
        [Kind(PopularKinds.Function)]
        sealed class IsSortedFunction : MemberFunction
        {
            [Description("Determines whether set is of Sorted type")]
            public ScalarValue Function()
            {
                SetValue set = @this as SetValue;
                if (ReferenceEquals(set, null))
                    throw new YAMPSetsFunctionNotMemberException("SetIsSorted");

                bool eq = set.Sorted;
                return new ScalarValue(eq);
            }
        }

        [Description("The SetIsSubsetOf Object function.")]
        [Kind(PopularKinds.Function)]
        sealed class IsSubsetOfFunction : MemberFunction
        {
            [Description("Determines whether set1 is a subset of set2")]
            public ScalarValue Function(SetValue set2)
            {
                SetValue set = @this as SetValue;
                if (ReferenceEquals(set, null))
                    throw new YAMPSetsFunctionNotMemberException("SetIsSubsetOf");

                bool eq = set.Set.IsSubsetOf(set2.Set);
                return new ScalarValue(eq);
            }
        }

        [Description("The SetIsSupersetOf Object function.")]
        [Kind(PopularKinds.Function)]
        sealed class IsSupersetOfFunction : MemberFunction
        {
            [Description("Determines whether set1 is a superset of set2")]
            public ScalarValue Function(SetValue set2)
            {
                SetValue set = @this as SetValue;
                if (ReferenceEquals(set, null))
                    throw new YAMPSetsFunctionNotMemberException("SetIsSupersetOf");

                bool eq = set.Set.IsSupersetOf(set2.Set);
                return new ScalarValue(eq);
            }
        }

        [Description("The SetOverlaps Object function.")]
        [Kind(PopularKinds.Function)]
        sealed class OverlapsFunction : MemberFunction
        {
            [Description("Determines whether the sets overlap over at least one common element")]
            public ScalarValue Function(SetValue set2)
            {
                SetValue set = @this as SetValue;
                if (ReferenceEquals(set, null))
                    throw new YAMPSetsFunctionNotMemberException("SetOverlaps");

                bool eq = set.Set.Overlaps(set2.Set);
                return new ScalarValue(eq);
            }
        }

        [Description("The SetRemove Object function.")]
        [Kind(PopularKinds.Function)]
        sealed class RemoveFunction : MemberFunction
        {
            [Description("Removes the specified element from the Set, and returns the set. If Matrix, all its elements will be removed")]
            public SetValue Function(Value id)
            {
                SetValue set = @this as SetValue;
                if (ReferenceEquals(set, null))
                    throw new YAMPSetsFunctionNotMemberException("SetRemove");

                if (id is MatrixValue)
                {
                    set.RemoveElements((id as MatrixValue).ToArray());
                }
                else
                {
                    set.Set.Remove(new SetValue.ValueWrap(id));
                }
                return set;
            }
        }

        [Description("The SetToMatrix Object function.")]
        [Kind(PopularKinds.Function)]
        sealed class ToMatrixFunction : MemberFunction
        {
            [Description("Create a single row Matrix with all Numeric keys")]
            public MatrixValue Function()
            {
                SetValue set = @this as SetValue;
                if (ReferenceEquals(set, null))
                    throw new YAMPSetsFunctionNotMemberException("SetToMatrix");

                return SetValue.TToMatrix(set);
            }
        }

        [Description("The SetUnion Object function.")]
        [Kind(PopularKinds.Function)]
        sealed class UnionFunction : MemberFunction
        {
            [Description("Create a new Set, with the Union of Sets")]
            [Arguments(0, 0)]
            public SetValue Function(ArgumentsValue args)
            {
                SetValue set = @this as SetValue;
                if (ReferenceEquals(set, null))
                    throw new YAMPSetsFunctionNotMemberException("SetUnion");

                return SetValue.TUnion(set, args);
            }
        }

        #endregion

    }
}
