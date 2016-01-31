namespace YAMP.Physics
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using YAMP.Exceptions;

    /// <summary>
    /// Used to represent combined units -- temporary and defined.
    /// </summary>
    class CombinedUnit : PhysicalUnit
    {
        #region Fields

        static readonly CultureInfo NumberFormat = CultureInfo.InvariantCulture;

        readonly Dictionary<String, Double> _units;

        String _lastAddedUnit;
        Double _lastAddedFactor;
        Double _factor;

        #endregion

        #region ctor

        public CombinedUnit(String unit)
        {
            _units = new Dictionary<String, Double>();
            Unit = unit;
        }

        protected CombinedUnit(String combination, Double factor)
        {
            _factor = factor;
            _units = Parse(combination);
            base.Unit = GetType().Name.Replace("Unit", String.Empty);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the underlying factor.
        /// </summary>
        public Double Factor
        {
            get { return _factor; }
        }

        /// <summary>
        /// Gets or sets the presented unit.
        /// </summary>
        public override String Unit
        {
            get
            {
                return base.Unit;
            }
            protected set
            {
                base.Unit = value;
                _factor = 1.0;
                _units.Clear();
                Parse(value, _units);
            }
        }

        /// <summary>
        /// Gets the underlying elementary units.
        /// </summary>
        public Dictionary<String, Double> ElementaryUnits
        {
            get { return _units; }
        }

        #endregion

        #region Methods

        public override String ToString()
        {
            return Unit;
        }

        /// <summary>
        /// Unpacks the given units, i.e. returns the representation of the unit in elementary units.
        /// </summary>
        /// <returns>The string represention in elementary units.</returns>
        public String Unpack()
        {
            var inner = false;
            var s = new StringBuilder();
            var positive = new List<String>();
            var negative = new List<String>();

            foreach (var m in _units)
            {
                if (m.Value > 0)
                {
                    positive.Add(m.Key);
                }
                else if (m.Value < 0)
                {
                    negative.Add(m.Key);
                }
            }

            foreach (var pos in positive)
            {
                if (inner)
                {
                    s.Append("*");
                }
                else
                {
                    inner = true;
                }

                s.Append(pos);

                if (_units[pos] != 1.0)
                {
                    s.Append("^").Append(_units[pos]);
                }
            }

            if (negative.Count != 0)
            {
                inner = false;

                if (s.Length == 0)
                {
                    s.Append("1");
                }

                if (negative.Count != 1)
                {
                    s.Append("/(");
                }
                else
                {
                    s.Append("/");
                }

                foreach (var neg in negative)
                {
                    if (inner)
                    {
                        s.Append("*");
                    }
                    else
                    {
                        inner = true;
                    }

                    s.Append(neg);

                    if (_units[neg] != -1.0)
                    {
                        s.Append("^").Append(-_units[neg]);
                    }
                }

                if (negative.Count != 1)
                {
                    s.Append(")");
                }
            }

            return s.ToString();
        }

        /// <summary>
        /// Tries to simplify the current expression by considering combined types.
        /// </summary>
        /// <returns>The current instance.</returns>
        public CombinedUnit Simplify()
        {
            var minChanges = Double.MaxValue;
            var minUnit = String.Empty;
            var isReversed = false;
            var total = 0.0;
            var i = 0;

            do
            {
                //Criteria for cancelling: avoid MORE changes than last round
                total = minChanges;

                if (!String.IsNullOrEmpty(minUnit))
                {
                    var U = CombinedUnits[minUnit];
                    var sgn = isReversed ? -1.0 : 1.0;

                    foreach (var unit in U._units)
                    {
                        AddUnit(_units, unit.Key, -sgn * unit.Value);
                    }

                    _factor /= U._factor;
                    AddUnit(_units, U.Unit, sgn);
                }

                foreach (var cu in CombinedUnits)
                {
                    var reversed = false;
                    var changes = GetChanges(cu.Value, out reversed);

                    if (changes < minChanges)
                    {
                        minChanges = changes;
                        minUnit = cu.Key;
                        isReversed = reversed;
                    }
                }
            }
            while(minChanges > 0 &&  minChanges < total && i++ < 8);

            return this;
        }

        Double GetChanges(CombinedUnit unit, out Boolean reversed)
        {
            var misses = 0.0;
            var posCatches = 0.0;
            var negCatches = 0.0;
            reversed = false;

            foreach (var b in unit._units)
            {
                var found = false;

                foreach (var a in _units)
                {
                    if (a.Key == b.Key)
                    {
                        found = true;
                        posCatches += Math.Abs(b.Value - a.Value);
                        negCatches += Math.Abs(b.Value + a.Value);
                        break;
                    }
                }

                if (!found)
                {
                    misses += Math.Abs(b.Value);
                }
            }

            if (negCatches < posCatches)
            {
                reversed = true;
                return misses + negCatches;
            }

            return misses + posCatches;
        }

        /// <summary>
        /// Tries to convert the unit to the given unit.
        /// </summary>
        /// <param name="unit">The unit (combination) to convert to.</param>
        /// <returns>A list of converters to be performed on a certain value having the current unit.</returns>
        public List<Func<Double, Double>> ConvertTo(String unit)
        {
            var list = new List<Func<Double, Double>>();
            var sourceFactor = _factor;
            _factor = 1.0;
            var source = _units;
            var target = Parse(unit);
            var targetFactor = _factor;
            var bag = new Dictionary<String, Double>();

            //Calculation of new factor given by unit -- more to come below
            _factor = sourceFactor / targetFactor;

            foreach (var srcUnit in source)
            {
                var sign = srcUnit.Value > 0 ? 1 : - 1;
                var abs = srcUnit.Value > 0 ? srcUnit.Value : -srcUnit.Value;

                for (var i = 1; i <= abs; i++)
                {
                    var tu = FindMapping(srcUnit.Key, target, sign, list);

                    if (bag.ContainsKey(tu))
                    {
                        bag[tu] += sign;
                    }
                    else
                    {
                        bag.Add(tu, sign);
                    }
                }
            }

            if (target.Count != 0)
            {
                throw new YAMPException("The conversion is not possible. The units are not converting properly.");
            }

            _units.Clear();

            foreach (var element in bag)
            {
                _units.Add(element.Key, element.Value);
            }

            base.Unit = unit;
            return list;
        }

        String FindMapping(String source, Dictionary<String, Double> target, Int32 sign, List<Func<Double, Double>> list)
        {
            var srcUnit = FindUnit(source);

            if (srcUnit == null)
            {
                throw new YAMPException("The unit " + source + " could not be found.");
            }

            _factor *= Math.Pow(srcUnit.Weight, sign);

            foreach (var unit in target.Keys)
            {
                if (srcUnit.HasConversion(unit))
                {
                    var dstUnit = FindUnit(unit);
                    target[unit] -= sign;

                    if (sign > 0)
                    {
                        list.Add(srcUnit.GetConversion(dstUnit.Unit));
                        _factor /= dstUnit.Weight;
                    }
                    else
                    {
                        list.Add(srcUnit.GetInverseConversion(dstUnit.Unit));
                        _factor *= dstUnit.Weight;
                    }

                    if (target[unit] == 0)
                    {
                        target.Remove(unit);
                    }

                    return unit;
                }
            }

            throw new YAMPException("Nothing found to convert for " + srcUnit.Unit + ".");
        }

        protected override PhysicalUnit Create()
        {
            return new CombinedUnit(Unit);
        }

        public CombinedUnit CreateFrom(String unit)
        {
            var name = Unpack();
            var obj = new CombinedUnit(name, _factor);
            obj._factor *= GetWeight(unit);
            return obj;
        }

        #endregion

        #region Parsing

        void AddUnit(Dictionary<String, Double> units, String unit, Double exp)
        {
            if (exp != 0.0)
            {
                _lastAddedUnit = unit;
                _lastAddedFactor = exp;

                if (units.ContainsKey(unit))
                {
                    units[unit] += exp;
                }
                else
                {
                    units.Add(unit, exp);
                }

                if (units[unit] == 0)
                {
                    units.Remove(unit);
                }
            }
        }

        void Parse(String unit, Dictionary<String, Double> units)
        {
            var reverseStack = new Stack<Boolean>();
            var reverse = false;
            var k = 0;
            var op = Token.Multiply;
            var tk = new Token[unit.Length + 1];

            foreach (var ch in unit)
            { 
                tk[k++] = GetToken(ch);
            }

            tk[k] = Token.End;

            var sum = Count(Token.OpenBracket, tk) - Count(Token.CloseBracket, tk);

            if (sum != 0)
            {
                throw new YAMPUnitBracketException(unit);
            }

            for (var i = 0; i != unit.Length; i++)
            {
                if (tk[i] == Token.Whitespace)
                {
                    continue;
                }

                if (tk[i] == Token.CloseBracket)
                {
                    reverse = reverseStack.Pop();
                    continue;
                }

                if (tk[i] == Token.Multiply || tk[i] == Token.Divide || tk[i] == Token.Power)
                {
                    op = tk[i];
                    continue;
                }

                if (tk[i] == Token.OpenBracket)
                {
                    reverseStack.Push(reverse);

                    if (op == Token.Divide)
                    {
                        op = Token.Multiply;
                        reverse = !reverse;
                    }

                    continue;
                }

                var r = reverse ? -1.0 : 1.0;
                var current = tk[i];
                var result = EatWhile(current, tk, unit, i);

                switch (op)
                {
                    case Token.Multiply:
                        if (current == Token.Number)
                            _factor *= Math.Pow(ConvertToDouble(result), r);
                        else if (current == Token.Letter)
                            AddUnit(units, result, r);
                        else
                            throw new YAMPUnitParseException(i, unit.Substring(i));
                        break;

                    case Token.Divide:
                        if (current == Token.Number)
                            _factor /= Math.Pow(ConvertToDouble(result), r);
                        else if (current == Token.Letter)
                            AddUnit(units, result, -r);
                        else
                            throw new YAMPUnitParseException(i, unit.Substring(i));
                        break;

                    case Token.Power:
                        if (current != Token.Number)
                            throw new YAMPUnitParseException(i, unit.Substring(i));

                        var exp = ConvertToDouble(result);
                        AddUnit(units, _lastAddedUnit, _lastAddedFactor * (exp - 1));
                        break;
                }

                i += result.Length - 1;
            }

            var combined = new List<String>();

            foreach (var unitKey in units.Keys)
            {
                if (IsCombinedUnit(unitKey))
                {
                    combined.Add(unitKey);
                }
            }

            foreach (var cu in combined)
            {
                var newUnit = FindCombinedUnit(cu);
                var count = units[cu];
                units.Remove(cu);

                foreach (var pair in newUnit.ElementaryUnits)
                {
                    if (units.ContainsKey(pair.Key))
                    {
                        units[pair.Key] += pair.Value * count;
                    }
                    else
                    {
                        units.Add(pair.Key, pair.Value * count);
                    }

                    if (units[pair.Key] == 0)
                    {
                        units.Remove(pair.Key);
                    }
                }

                _factor *= Math.Pow(newUnit.Factor, count);
            }
        }

        Dictionary<String, Double> Parse(String unit)
        {
            var units = new Dictionary<String, Double>();
            Parse(unit, units);
            return units;
        }

        Int32 Count(Token token, Token[] tokens)
        {
            var sum = 0;

            foreach (var t in tokens)
            {
                if (t == token)
                {
                    sum++;
                }
            }

            return sum;
        }

        String EatWhile(Token token, Token[] tokens, String text, Int32 start)
        {
            var sb = new StringBuilder();
            var index = start;

            do
            {
                sb.Append(text[index]);
            } 
            while (tokens[++index] == token);

            return sb.ToString();
        }

        #endregion

        #region Converters

        Double ConvertToDouble(String s)
        {
            var sign = 1.0;

            if (s.LastIndexOf('-') > 0)
                throw new YAMPUnitConvertException(s);

            if (s[0] == '-')
            {
                sign = -1.0;
                s = s.Substring(1);
            }

            return sign * Double.Parse(s, NumberFormat);
        }

        int ConvertToInteger(string s)
        {
            var sign = 1;

            if (s.LastIndexOf('-') > 0)
                throw new YAMPUnitConvertException(s);

            if (s.LastIndexOf('.') >= 0)
                throw new YAMPUnitConvertException(s);

            if (s[0] == '-')
            {
                sign = -1;
                s = s.Substring(1);
            }

            return sign * Int32.Parse(s, NumberFormat);
        }

        #endregion

        #region Token Management

        Token GetToken(char ch)
        {
            if (IsWhiteSpace(ch))
                return Token.Whitespace;
            else if (IsNumber(ch))
                return Token.Number;
            else if (ch == '/')
                return Token.Divide;
            else if (ch == '*')
                return Token.Multiply;
            else if (ch == '^')
                return Token.Power;
            else if (ch == '(')
                return Token.OpenBracket;
            else if (ch == ')')
                return Token.CloseBracket;

            return Token.Letter;
        }

        enum Token
        {
            OpenBracket,
            CloseBracket,
            Number,
            Whitespace,
            Power,
            Multiply,
            Divide,
            Letter,
            End
        }

        protected bool IsNumber(char ch)
        {
            return "-0123456789.".Contains(ch.ToString());
        }

        protected bool IsWhiteSpace(char ch)
        {
            return ParseEngine.IsWhiteSpace(ch);
        }

        #endregion

        #region Modifying

        /// <summary>
        /// Multiplies the given combined unit with the string representation of another unit.
        /// </summary>
        /// <param name="unit">The unit to multiply the current unit with.</param>
        /// <returns>The (current) modified unit.</returns>
        public CombinedUnit Multiply(String unit)
        {
            var cu = Parse(unit);

            foreach (var element in cu)
            {
                AddUnit(_units, element.Key, element.Value);
            }

            return this;
        }

        /// <summary>
        /// Multiplies the given combined unit with another combined unit.
        /// </summary>
        /// <param name="unit">The combined unit which is multiplied to the current unit.</param>
        /// <returns>The (current) modified unit.</returns>
        public CombinedUnit Multiply(CombinedUnit unit)
        {
            foreach (var element in unit.ElementaryUnits)
            {
                AddUnit(_units, element.Key, element.Value);
            }

            return this;
        }

        /// <summary>
        /// Divides the given combined unit by the string representation of another unit.
        /// </summary>
        /// <param name="unit">The combined unit to divide the current unit.</param>
        /// <returns>The (current) modified unit.</returns>
        public CombinedUnit Divide(String unit)
        {
            var cu = Parse(unit);

            foreach (var element in cu)
            {
                AddUnit(_units, element.Key, -element.Value);
            }

            return this;
        }

        /// <summary>
        /// Divides the given combined unit by another unit.
        /// </summary>
        /// <param name="unit">The combined unit to divide the current unit.</param>
        /// <returns>The (current) modified unit.</returns>
        public CombinedUnit Divide(CombinedUnit unit)
        {
            foreach (var element in unit.ElementaryUnits)
            {
                AddUnit(_units, element.Key, -element.Value);
            }

            return this;
        }

        /// <summary>
        /// Raises the current unit to the specified power, i.e. kg*s --> raised to 2 is kg^2 * s^2.
        /// </summary>
        /// <param name="pwr">The power to raise the unit with.</param>
        /// <returns>The (current) modified unit.</returns>
        public CombinedUnit Raise(Double pwr)
        {
            foreach (var element in _units.Keys.ToArray())
            {
                _units[element] *= pwr;
            }

            return this;
        }

        /// <summary>
        /// Takes the square root of the current unit, i.e. raises it to the power 1/2.
        /// </summary>
        /// <returns>The (current) modified unit.</returns>
        public CombinedUnit Sqrt()
        {
            return Raise(0.5);
        }

        #endregion
    }
}
