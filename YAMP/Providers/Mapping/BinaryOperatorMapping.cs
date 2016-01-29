using System;

namespace YAMP
{
    /// <summary>
    /// Represents the elementary object for binary operator mappings.
    /// </summary>
    class BinaryOperatorMapping : OperatorMapping
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

        public MapHit IsMapping(object a, object b)
        {
            var dist1 = TypeDistance(_arg1, a);
            var dist2 = TypeDistance(_arg2, b);

            if (dist1 == MapHit.Miss || dist2 == MapHit.Miss)
                return MapHit.Miss;
            else if (dist1 == MapHit.Direct && dist2 == MapHit.Direct)
                return MapHit.Direct;

            return MapHit.Indirect;
        }

        public Func<Value, Value, Value> Map
        {
            get
            {
                return _f;
            }
        }
    }

    enum MapHit
    {
        Miss,
        Direct,
        Indirect
    }
}
