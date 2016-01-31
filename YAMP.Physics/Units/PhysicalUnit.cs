namespace YAMP.Physics
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// Represents an (in its essence elementary) physical unit.
    /// </summary>
    abstract class PhysicalUnit
    {
        #region Fields

        readonly Dictionary<PhysicalUnit, Func<Double, Double>> _conversionTable;
        readonly Dictionary<PhysicalUnit, Func<Double, Double>> _invConversionTable;
        readonly Dictionary<String, Double> _prefixes;
        Double _weight;
        String _unit;

        protected static Dictionary<String, PhysicalUnit> KnownUnits = new Dictionary<String, PhysicalUnit>();
        protected static Dictionary<String, CombinedUnit> CombinedUnits = new Dictionary<String, CombinedUnit>();

        #endregion

        #region ctor

        static PhysicalUnit()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var types = assembly.GetTypes();
            var combinedTypes = new List<Type>();

            foreach (var type in types)
            {
                if (type.IsSubclassOf(typeof(PhysicalUnit)) && !type.IsAbstract)
                {
                    if (type.IsSubclassOf(typeof(CombinedUnit)))
                    {
                        combinedTypes.Add(type);
                    }
                    else
                    {
                        var ctor = type.GetConstructor(Value.EmptyTypes);

                        if (ctor != null)
                        {
                            var instance = ctor.Invoke(null) as PhysicalUnit;
                            KnownUnits.Add(instance.Unit, instance);
                        }
                    }
                }
            }

            foreach (var type in combinedTypes)
            {
                var ctor = type.GetConstructor(Value.EmptyTypes);

                if (ctor != null)
                {
                    var instance = ctor.Invoke(null) as CombinedUnit;
                    CombinedUnits.Add(instance.Unit, instance);
                }
            }
        }

        public PhysicalUnit()
        {
            _weight = 1.0;
            _invConversionTable = new Dictionary<PhysicalUnit, Func<double, double>>();
            _conversionTable = new Dictionary<PhysicalUnit, Func<double, double>>();
            _prefixes = new Dictionary<string, double>();
            Unit = this.GetType().Name.Replace("Unit", string.Empty);
            SetPrefixes();
        }

        #endregion

        #region Properties

        public Double Weight
        {
            get { return _weight; }
        }

        public virtual String Unit 
        {
            get { return _unit; }
            protected set { _unit = value; }
        }

        #endregion

        #region Methods

        public static PhysicalUnit FindUnit(String unit)
        {
            foreach (var value in KnownUnits.Values)
            {
                if (value.CanBe(unit))
                {
                    return value.TransformTo(unit);
                }
            }

            return null;
        }

        public static Boolean IsCombinedUnit(String unit)
        {
            foreach (var value in CombinedUnits.Values)
            {
                if (value.CanBe(unit))
                {
                    return true;
                }
            }

            return false;
        }

        public static CombinedUnit FindCombinedUnit(String unit)
        {
            foreach (var value in CombinedUnits.Values)
            {
                if (value.CanBe(unit))
                {
                    return value.CreateFrom(unit);
                }
            }

            return null;
        }

        public virtual PhysicalUnit TransformTo(String unit)
        {
            if (!unit.Equals(Unit))
            {
                foreach (var prefix in _prefixes)
                {
                    if (prefix.Key + Unit == unit)
                    {
                        var pu = Create();
                        pu._weight = prefix.Value;
                        return pu;
                    }
                }
            }

            return Create();
        }

        protected Double GetWeight(String unit)
        {
            if (!unit.Equals(Unit))
            {
                foreach (var prefix in _prefixes)
                {
                    if (prefix.Key + Unit == unit)
                    {
                        return prefix.Value;
                    }
                }
            }

            return 1.0;
        }

        protected abstract PhysicalUnit Create();

        public virtual Boolean CanBe(String unit)
        {
            if (!unit.Equals(Unit))
            {
                foreach (var prefix in _prefixes.Keys)
                {
                    if (prefix + Unit == unit)
                    {
                        return true;
                    }
                }

                return false;
            }

            return true;
        }

        protected virtual void SetPrefixes()
        {
            _prefixes.Add("da", 10);
            _prefixes.Add("h", 100);
            _prefixes.Add("k", 1e3);
            _prefixes.Add("M", 1e6);
            _prefixes.Add("G", 1e9);
            _prefixes.Add("T", 1e12);
            _prefixes.Add("P", 1e15);
            _prefixes.Add("E", 1e18);
            _prefixes.Add("Z", 1e21);
            _prefixes.Add("Y", 1e24);

            _prefixes.Add("d", 1e-1);
            _prefixes.Add("c", 1e-2);
            _prefixes.Add("m", 1e-3);
            _prefixes.Add("µ", 1e-6);
            _prefixes.Add("n", 1e-9);
            _prefixes.Add("p", 1e-12);
            _prefixes.Add("f", 1e-15);
            _prefixes.Add("a", 1e-18);
            _prefixes.Add("z", 1e-21);
            _prefixes.Add("y", 1e-24);
        }

        public virtual Boolean HasConversation(String target)
        {
            if (!CanBe(target))
            {
                foreach (var entry in _conversionTable)
                {
                    if (entry.Key.CanBe(target))
                    {
                        return true;
                    }
                }

                return false;
            }

            return true;
        }

        public virtual Func<Double, Double> GetConversation(String unit)
        {
            if (unit == Unit)
            {
                return Identity;
            }

            return _conversionTable[KnownUnits[unit]];
        }

        public virtual Func<Double, Double> GetInverseConversation(String unit)
        {
            if (unit == Unit)
            {
                return Identity;
            }

            return _invConversionTable[KnownUnits[unit]];
        }

        Double Identity(Double x)
        {
            return x;
        }

        /// <summary>
        /// Adds a conversation determined by y = a * x.
        /// </summary>
        /// <param name="target">The target unit, e.g. in m to yd, yd would be the target unit.</param>
        /// <param name="rate">The rate of the conversion.</param>
        /// <returns>The current unit.</returns>
        public PhysicalUnit Add(String target, Double rate)
        {
            if (!KnownUnits.ContainsKey(target))
            {
                KnownUnits.Add(target, new ConversionUnit(target, this));
            }

            _conversionTable.Add(KnownUnits[target], x => x * rate);
            _invConversionTable.Add(KnownUnits[target], x => x / rate);
            return this;
        }

        /// <summary>
        /// Adds a conversation determined by y = a * x + b.
        /// </summary>
        /// <param name="target">The target unit, e.g. in K to °C, °C would be the target unit.</param>
        /// <param name="rate">The rate (a) of the conversion.</param>
        /// <param name="offset">The offset (b) of the conversion.</param>
        /// <returns>The current unit.</returns>
        public PhysicalUnit Add(String target, Double rate, Double offset)
        {
            if (!KnownUnits.ContainsKey(target))
            {
                KnownUnits.Add(target, new ConversionUnit(target, this));
            }

            _conversionTable.Add(KnownUnits[target], x => x * rate + offset);
            _invConversionTable.Add(KnownUnits[target], x => (x - offset) / rate);
            return this;
        }

        #endregion
    }
}
