/*
    Copyright (c) 2012-2014, Florian Rappl.
    All rights reserved.

    Redistribution and use in source and binary forms, with or without
    modification, are permitted provided that the following conditions are met:
        * Redistributions of source code must retain the above copyright
          notice, this list of conditions and the following disclaimer.
        * Redistributions in binary form must reproduce the above copyright
          notice, this list of conditions and the following disclaimer in the
          documentation and/or other materials provided with the distribution.
        * Neither the name of the YAMP team nor the names of its contributors
          may be used to endorse or promote products derived from this
          software without specific prior written permission.

    THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
    ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
    WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
    DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
    DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
    (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
    LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
    ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
    (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
    SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System;
using System.Collections.Generic;
using System.Reflection;
using YAMP;

namespace YAMP.Physics
{
    /// <summary>
    /// Represents an (in its essence elementary) physical unit.
    /// </summary>
    abstract class PhysicalUnit
    {
        #region Members

        Dictionary<PhysicalUnit, Func<double, double>> conversionTable;
        Dictionary<PhysicalUnit, Func<double, double>> invConversionTable;
        Dictionary<string, double> prefixes;
        double weight;

        protected static Dictionary<string, PhysicalUnit> knownUnits = new Dictionary<string, PhysicalUnit>();
        protected static Dictionary<string, CombinedUnit> combinedUnits = new Dictionary<string, CombinedUnit>();

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
                    if(type.IsSubclassOf(typeof(CombinedUnit)))
                    {
                        combinedTypes.Add(type);
                        continue;
                    }

                    var ctor  = type.GetConstructor(Value.EmptyTypes);

                    if(ctor == null)
                        continue;

                    var instance = ctor.Invoke(null) as PhysicalUnit;
                    knownUnits.Add(instance.Unit, instance);
                }
            }

            foreach (var type in combinedTypes)
            {
                var ctor  = type.GetConstructor(Value.EmptyTypes);

                if(ctor == null)
                    continue;

                var instance = ctor.Invoke(null) as CombinedUnit;
                combinedUnits.Add(instance.Unit, instance);
            }
        }

        public PhysicalUnit()
        {
            weight = 1.0;
            invConversionTable = new Dictionary<PhysicalUnit, Func<double, double>>();
            conversionTable = new Dictionary<PhysicalUnit, Func<double, double>>();
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

        public static bool IsCombinedUnit(string unit)
        {
            foreach (var value in combinedUnits.Values)
            {
                if (value.CanBe(unit))
                    return true;
            }

            return false;
        }

        public static CombinedUnit FindCombinedUnit(string unit)
        {
            foreach (var value in combinedUnits.Values)
            {
                if (value.CanBe(unit))
                    return value.CreateFrom(unit);
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

        protected double GetWeight(string unit)
        {
            if (!unit.Equals(Unit))
            {
                foreach (var prefix in prefixes)
                {
                    if (prefix.Key + Unit == unit)
                        return prefix.Value;
                }
            }

            return 1.0;
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

        public virtual bool HasConversation(string target)
        {
            if (CanBe(target))
                return true;

            foreach (var entry in conversionTable)
            {
                if (entry.Key.CanBe(target))
                    return true;
            }

            return false;
        }

        public virtual Func<double, double> GetConversation(string unit)
        {
            if (unit == Unit)
                return Identity;

            return conversionTable[knownUnits[unit]];
        }

        public virtual Func<double, double> GetInverseConversation(string unit)
        {
            if (unit == Unit)
                return Identity;

            return invConversionTable[knownUnits[unit]];
        }

        double Identity(double x)
        {
            return x;
        }

        /// <summary>
        /// Adds a conversation determined by y = a * x.
        /// </summary>
        /// <param name="target">The target unit, e.g. in m to yd, yd would be the target unit.</param>
        /// <param name="rate">The rate of the conversion.</param>
        /// <returns>The current unit.</returns>
        public PhysicalUnit Add(string target, double rate)
        {
            if (!knownUnits.ContainsKey(target))
                knownUnits.Add(target, new ConversationUnit(target, this));

            conversionTable.Add(knownUnits[target], x => x * rate);
            invConversionTable.Add(knownUnits[target], x => x / rate);
            return this;
        }

        /// <summary>
        /// Adds a conversation determined by y = a * x + b.
        /// </summary>
        /// <param name="target">The target unit, e.g. in K to °C, °C would be the target unit.</param>
        /// <param name="rate">The rate (a) of the conversion.</param>
        /// <param name="offset">The offset (b) of the conversion.</param>
        /// <returns>The current unit.</returns>
        public PhysicalUnit Add(string target, double rate, double offset)
        {
            if (!knownUnits.ContainsKey(target))
                knownUnits.Add(target, new ConversationUnit(target, this));

            conversionTable.Add(knownUnits[target], x => x * rate + offset);
            invConversionTable.Add(knownUnits[target], x => (x - offset) / rate);
            return this;
        }

        #endregion
    }
}
