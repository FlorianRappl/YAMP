using System;
using System.Collections.Generic;
using System.Reflection;
using YAMP;

namespace YAMP.Physics
{
    abstract class PhysicalUnit
    {
        #region Members

        Dictionary<string, Func<double, double>> conversationTable;
        Dictionary<string, double> prefixes;
        double weight;

        static Dictionary<string, PhysicalUnit> knownUnits = new Dictionary<string, PhysicalUnit>();

        #endregion

        #region ctor

        static PhysicalUnit()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var types = assembly.GetTypes();

            foreach (var type in types)
            {
                if (type.IsSubclassOf(typeof(PhysicalUnit)) && !type.IsAbstract)
                {
                    var ctor  = type.GetConstructor(Value.EmptyTypes);

                    if(ctor == null)
                        continue;

                    var instance = ctor.Invoke(null) as PhysicalUnit;
                    knownUnits.Add(instance.Unit, instance);
                }
            }
        }

        public PhysicalUnit()
        {
            weight = 1.0;
            conversationTable = new Dictionary<string, Func<double, double>>();
            prefixes = new Dictionary<string, double>();
            Unit = this.GetType().Name.Replace("Unit", string.Empty);
            SetPrefixes();
        }

        #endregion

        #region Properties

        public virtual double Weight
        {
            get
            {
                return weight;
            }
        }

        public virtual string Unit { get; protected set; }

        #endregion

        #region Methods

        public static PhysicalUnit FindUnit(string unit)
        {
            foreach (var value in knownUnits.Values)
            {
                if (value.CanBe(unit))
                    return value.TransformTo(unit);
            }

            return null;
        }

        public virtual PhysicalUnit TransformTo(string unit)
        {
            if (!unit.Equals(Unit))
            {
                foreach (var prefix in prefixes)
                {
                    if (prefix.Key + Unit == unit)
                    {
                        var pu = Create();
                        pu.weight = prefix.Value;
                        return pu;
                    }
                }
            }

            return Create();
        }

        protected abstract PhysicalUnit Create();

        public virtual bool CanBe(string unit)
        {
            if (unit.Equals(Unit))
                return true;

            foreach (var prefix in prefixes.Keys)
            {
                if (prefix + Unit == unit)
                    return true;
            }

            return false;
        }

        protected virtual void SetPrefixes()
        {
            prefixes.Add("da", 10);
            prefixes.Add("h", 100);
            prefixes.Add("k", 1e3);
            prefixes.Add("M", 1e6);
            prefixes.Add("G", 1e9);
            prefixes.Add("T", 1e12);
            prefixes.Add("P", 1e15);
            prefixes.Add("E", 1e18);
            prefixes.Add("Z", 1e21);
            prefixes.Add("Y", 1e24);

            prefixes.Add("d", 1e-1);
            prefixes.Add("c", 1e-2);
            prefixes.Add("m", 1e-3);
            prefixes.Add("µ", 1e-6);
            prefixes.Add("n", 1e-9);
            prefixes.Add("p", 1e-12);
            prefixes.Add("f", 1e-15);
            prefixes.Add("a", 1e-18);
            prefixes.Add("z", 1e-21);
            prefixes.Add("y", 1e-24);
        }

        public PhysicalUnit Add(string target, double rate)
        {
            conversationTable.Add(target, x => x * rate);
            return this;
        }

        public PhysicalUnit Add(string target, Func<double, double> rate)
        {
            conversationTable.Add(target, rate);
            return this;
        }

        #endregion
    }
}
