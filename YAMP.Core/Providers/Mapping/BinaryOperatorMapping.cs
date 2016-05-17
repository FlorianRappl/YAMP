namespace YAMP
{
    using System;

    /// <summary>
    /// Represents the elementary object for binary operator mappings.
    /// </summary>
    sealed class BinaryOperatorMapping : OperatorMapping, IEquatable<BinaryOperatorMapping>
    {
        Func<Value, Value, Value> _f;
        Type _arg1;
        Type _arg2;

        public BinaryOperatorMapping(Type arg1, Type arg2, Func<Value, Value, Value> f)
        {
            _arg1 = arg1;
            _arg2 = arg2;
            _f = f;
        }

        public Func<Value, Value, Value> Map
        {
            get { return _f; }
        }

        public MapHit IsMapping(object a, object b)
        {
            var dist1 = TypeDistance(_arg1, a);
            var dist2 = TypeDistance(_arg2, b);

            if (dist1 == MapHit.Miss || dist2 == MapHit.Miss)
            {
                return MapHit.Miss;
            }
            else if (dist1 == MapHit.Direct && dist2 == MapHit.Direct)
            {
                return MapHit.Direct;
            }

            return MapHit.Indirect;
        }

        public override Boolean Equals(Object obj)
        {
            var other = obj as BinaryOperatorMapping;
            return other != null ? this.Equals(other) : base.Equals(obj);
        }

        public Boolean Equals(BinaryOperatorMapping other)
        {
            return other != null && (_arg1 == other._arg1) && (_arg2 == other._arg2) && (_f == other._f);
        }

        public override Int32 GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    enum MapHit
    {
        Miss,
        Direct,
        Indirect
    }
}
