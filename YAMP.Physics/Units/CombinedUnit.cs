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
using System.Globalization;
using System.Text;
using YAMP;
using System.Linq;

namespace YAMP.Physics
{
    /// <summary>
    /// Used to represent combined units -- temporary and defined.
    /// </summary>
    class CombinedUnit : PhysicalUnit
    {
        #region Members

        string lastAddedUnit;
        double lastAddedFactor;
        double factor;
        string unit;
        Dictionary<string, double> units = new Dictionary<string, double>();
        static CultureInfo numberFormat = new CultureInfo("en-us");

        #endregion

        #region ctor

        public CombinedUnit(string unit)
        {
            Unit = unit;
        }

        protected CombinedUnit(string combination, double factor)
        {
            this.factor = factor;
            units = Parse(combination);
            unit = GetType().Name.Replace("Unit", string.Empty);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the underlying factor.
        /// </summary>
        public double Factor
        {
            get { return factor; }
        }

        /// <summary>
        /// Gets or sets the presented unit.
        /// </summary>
        public override string Unit
        {
            get
            {
                return unit;
            }
            protected set
            {
                unit = value;
                factor = 1.0;
                units = Parse(value);
            }
        }

        /// <summary>
        /// Gets the underlying elementary units.
        /// </summary>
        public Dictionary<string, double> ElementaryUnits
        {
            get
            {
                return units;
            }
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return Unit;
        }

        /// <summary>
        /// Unpacks the given units, i.e. returns the representation of the unit in elementary units.
        /// </summary>
        /// <returns>The string represention in elementary units.</returns>
        public string Unpack()
        {
            var inner = false;
            var s = new StringBuilder();
            var positive = new List<string>();
            var negative = new List<string>();

            foreach (var m in units)
            {
                if (m.Value > 0)
                    positive.Add(m.Key);
                else if (m.Value < 0)
                    negative.Add(m.Key);
            }

            foreach (var pos in positive)
            {
                if (inner)
                    s.Append("*");
                else
                    inner = true;

                s.Append(pos);

                if (units[pos] != 1.0)
                    s.Append("^").Append(units[pos]);
            }

            if (negative.Count != 0)
            {
                inner = false;

                if (s.Length == 0)
                    s.Append("1");

                if (negative.Count != 1)
                    s.Append("/(");
                else
                    s.Append("/");

                foreach (var neg in negative)
                {
                    if(inner)
                        s.Append("*");
                    else
                        inner = true;

                    s.Append(neg);

                    if (units[neg] != -1.0)
                        s.Append("^").Append(-units[neg]);
                }

                if (negative.Count != 1)
                    s.Append(")");
            }

            return s.ToString();
        }

        /// <summary>
        /// Tries to simplify the current expression by considering combined types.
        /// </summary>
        /// <returns>The current instance.</returns>
        public CombinedUnit Simplify()
        {
            var minChanges = double.MaxValue;
            var minUnit = string.Empty;
            var isReversed = false;
            var total = 0.0;
            var i = 0;

            do
            {
                //Criteria for cancelling: avoid MORE changes than last round
                total = minChanges;

                if (!string.IsNullOrEmpty(minUnit))
                {
                    var U = combinedUnits[minUnit];
                    var sgn = isReversed ? -1.0 : 1.0;

                    foreach (var unit in U.units)
                        AddUnit(units, unit.Key, -sgn * unit.Value);

                    factor /= U.factor;
                    AddUnit(units, U.unit, sgn);
                }

                foreach (var cu in combinedUnits)
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

        double GetChanges(CombinedUnit unit, out bool reversed)
        {
            var misses = 0.0;
            var posCatches = 0.0;
            var negCatches = 0.0;
            reversed = false;

            foreach (var b in unit.units)
            {
                var found = false;

                foreach (var a in units)
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
                    misses += Math.Abs(b.Value);
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
        public List<Func<double, double>> ConvertTo(string unit)
        {
            var list = new List<Func<double, double>>();
            var sourceFactor = factor;
            factor = 1.0;
            var source = units;
            var target = Parse(unit);
            var targetFactor = factor;
            var bag = new Dictionary<string, double>();

            //Calculation of new factor given by unit -- more to come below
            factor = sourceFactor / targetFactor;

            foreach (var srcUnit in source)
            {
                var sign = srcUnit.Value > 0 ? 1 : - 1;
                var abs = srcUnit.Value > 0 ? srcUnit.Value : -srcUnit.Value;

                for (var i = 1; i <= abs; i++)
                {
                    var tu = FindMapping(srcUnit.Key, target, sign, list);

                    if (bag.ContainsKey(tu))
                        bag[tu] += sign;
                    else
                        bag.Add(tu, sign);
                }
            }

            if (target.Count != 0)
                throw new YAMPException("The conversation is not possible. The units are not converting properly.");

            units = bag;
            this.unit = unit;
            return list;
        }

        string FindMapping(string source, Dictionary<string, double> target, int sign, List<Func<double, double>> list)
        {
            var srcUnit = FindUnit(source);

            if (srcUnit == null)
                throw new YAMPException("The unit " + source + " could not be found.");

            factor *= Math.Pow(srcUnit.Weight, sign);

            foreach(var unit in target.Keys)
            {
                if (srcUnit.HasConversation(unit))
                {
                    var dstUnit = FindUnit(unit);
                    target[unit] -= sign;

                    if (sign > 0)
                    {
                        list.Add(srcUnit.GetConversation(dstUnit.Unit));
                        factor /= dstUnit.Weight;
                    }
                    else
                    {
                        list.Add(srcUnit.GetInverseConversation(dstUnit.Unit));
                        factor *= dstUnit.Weight;
                    }

                    if (target[unit] == 0)
                        target.Remove(unit);

                    return unit;
                }
            }

            throw new YAMPException("Nothing found to convert for " + srcUnit.Unit + ".");
        }

        protected override PhysicalUnit Create()
        {
            return new CombinedUnit(Unit);
        }

        public CombinedUnit CreateFrom(string unit)
        {
            var name = Unpack();
            var obj = new CombinedUnit(name, factor);
            obj.factor *= GetWeight(unit);
            return obj;
        }

        #endregion

        #region Parsing

        void AddUnit(Dictionary<string, double> units, string unit, double exp)
        {
            if (exp == 0.0)
                return;

            lastAddedUnit = unit;
            lastAddedFactor = exp;

            if (units.ContainsKey(unit))
                units[unit] += exp;
            else
                units.Add(unit, exp);

            if (units[unit] == 0)
                units.Remove(unit);
        }

        Dictionary<string, double> Parse(string unit)
        {
            var reverseStack = new Stack<bool>();
            var reverse = false;
            var units = new Dictionary<string, double>();
            var k = 0;
            var op = Token.Multiply;
            var tk = new Token[unit.Length + 1];

            foreach (var ch in unit)
                tk[k++] = GetToken(ch);

            tk[k] = Token.End;

            var sum = Count(Token.OpenBracket, tk) - Count(Token.CloseBracket, tk);

            if (sum != 0)
                throw new YAMPUnitBracketException(unit);

            for (var i = 0; i != unit.Length; i++)
            {
                if (tk[i] == Token.Whitespace)
                    continue;

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
                            factor *= Math.Pow(ConvertToDouble(result), r);
                        else if (current == Token.Letter)
                            AddUnit(units, result, r);
                        else
                            throw new YAMPUnitParseException(i, unit.Substring(i));
                        break;

                    case Token.Divide:
                        if (current == Token.Number)
                            factor /= Math.Pow(ConvertToDouble(result), r);
                        else if (current == Token.Letter)
                            AddUnit(units, result, -r);
                        else
                            throw new YAMPUnitParseException(i, unit.Substring(i));
                        break;

                    case Token.Power:
                        if (current != Token.Number)
                            throw new YAMPUnitParseException(i, unit.Substring(i));

                        var exp = ConvertToDouble(result);
                        AddUnit(units, lastAddedUnit, lastAddedFactor * (exp - 1));
                        break;
                }

                i += result.Length - 1;
            }

            var combined = new List<string>();

            foreach (var unitKey in units.Keys)
            {
                if (IsCombinedUnit(unitKey))
                    combined.Add(unitKey);
            }

            foreach (var cu in combined)
            {
                var newUnit = FindCombinedUnit(cu);
                var count = units[cu];
                units.Remove(cu);

                foreach (var pair in newUnit.ElementaryUnits)
                {
                    if (units.ContainsKey(pair.Key))
                        units[pair.Key] += pair.Value * count;
                    else
                        units.Add(pair.Key, pair.Value * count);

                    if (units[pair.Key] == 0)
                        units.Remove(pair.Key);
                }

                factor *= Math.Pow(newUnit.Factor, count);
            }

            return units;
        }

        int Count(Token token, Token[] tokens)
        {
            var sum = 0;

            foreach (var t in tokens)
                if (t == token)
                    sum++;

            return sum;
        }

        string EatWhile(Token token, Token[] tokens, string text, int start)
        {
            var sb = new StringBuilder();
            var index = start;

            do
            {
                sb.Append(text[index]);
            } while (tokens[++index] == token);

            return sb.ToString();
        }

        #endregion

        #region Converters

        double ConvertToDouble(string s)
        {
            var sign = 1.0;

            if (s.LastIndexOf('-') > 0)
                throw new YAMPUnitConvertException(s);

            if (s[0] == '-')
            {
                sign = -1.0;
                s = s.Substring(1);
            }

            var d = double.Parse(s, numberFormat);
            return sign * d;
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

            var d = int.Parse(s, numberFormat);
            return sign * d;
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
        public CombinedUnit Multiply(string unit)
        {
            var cu = Parse(unit);

            foreach (var element in cu)
                AddUnit(units, element.Key, element.Value);

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
                AddUnit(units, element.Key, element.Value);

            return this;
        }

        /// <summary>
        /// Divides the given combined unit by the string representation of another unit.
        /// </summary>
        /// <param name="unit">The combined unit to divide the current unit.</param>
        /// <returns>The (current) modified unit.</returns>
        public CombinedUnit Divide(string unit)
        {
            var cu = Parse(unit);

            foreach (var element in cu)
                AddUnit(units, element.Key, -element.Value);

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
                AddUnit(units, element.Key, -element.Value);

            return this;
        }

        /// <summary>
        /// Raises the current unit to the specified power, i.e. kg*s --> raised to 2 is kg^2 * s^2.
        /// </summary>
        /// <param name="pwr">The power to raise the unit with.</param>
        /// <returns>The (current) modified unit.</returns>
        public CombinedUnit Raise(double pwr)
        {
            foreach (var element in units.Keys.ToArray())
                units[element] *= pwr;

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
