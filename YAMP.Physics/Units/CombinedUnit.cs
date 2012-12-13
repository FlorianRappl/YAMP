using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using YAMP;

namespace YAMP.Physics
{
    class CombinedUnit : PhysicalUnit
    {
        #region Members

        string lastAddedUnit;
        int lastAddedFactor;
        double factor;
        string unit;
        Dictionary<string, int> units;
        static CultureInfo numberFormat = CultureInfo.CreateSpecificCulture("en-us");

        #endregion

        #region ctor

        public CombinedUnit(string unit)
        {
            Unit = unit;
        }

        protected CombinedUnit(string combination, double factor = 1.0)
        {
            this.factor = factor;
            units = Parse(combination);
            unit = GetType().Name.Replace("Unit", string.Empty);
        }

        #endregion

        #region Properties

        public double Factor
        {
            get { return factor; }
        }

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

        #endregion

        #region Methods

        public void Unpack()
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
                    s.Append(" * ");
                else
                    inner = true;

                s.Append(pos);

                if (units[pos] > 1)
                    s.Append("^").Append(units[pos]);
            }

            if (negative.Count != 0)
            {
                inner = false;

                if (s.Length == 0)
                    s.Append("1");

                s.Append(" / (");

                foreach (var neg in negative)
                {
                    if(inner)
                        s.Append(" * ");
                    else
                        inner = true;

                    s.Append(neg);

                    if (-units[neg] > 1)
                        s.Append("^").Append(-units[neg]);
                }

                s.Append(")");
            }

            unit = s.ToString();
        }

        public void ConvertTo(string unit)
        {
            var sourceFactor = factor;
            factor = 1.0;
            var source = units;
            var target = Parse(unit);
            var targetFactor = factor;
            var bag = new Dictionary<string, int>();

            //Calculation of new factor given by unit -- more to come below
            factor = sourceFactor / targetFactor;

            foreach (var srcUnit in source)
            {
                var sign = srcUnit.Value > 0 ? 1 : - 1;
                var abs = srcUnit.Value > 0 ? srcUnit.Value : -srcUnit.Value;

                for (var i = 1; i <= abs; i++)
                {
                    var tu = FindMapping(srcUnit.Key, target, sign);

                    if (bag.ContainsKey(tu))
                        bag[tu] += sign;
                    else
                        bag.Add(tu, sign);
                }
            }

            units = bag;
        }

        string FindMapping(string source, Dictionary<string, int> target, int sign)
        {
            var srcUnit = FindUnit(source);
            //TODO not ready yet!
            return source;//just as dummy!
        }

        protected override PhysicalUnit Create()
        {
            return new CombinedUnit(Unit);
        }

        #endregion

        #region Parsing

        void AddUnit(string unit, int exp)
        {
            if (exp == 0)
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

        Dictionary<string, int> Parse(string unit)
        {
            var reverseStack = new Stack<bool>();
            var reverse = false;
            var units = new Dictionary<string, int>();
            var k = 0;
            var op = Token.Multiply;
            var tk = new Token[unit.Length + 1];

            foreach (var ch in unit)
                tk[k++] = GetToken(ch);

            var sum = Count(Token.OpenBracket, tk) - Count(Token.CloseBracket, tk);

            if (sum != 0)
                throw new BracketException("(", unit);

            tk[k] = Token.End;

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
                        reverse = !reverse;
                }

                var r = reverse ? -1 : 1;
                var current = tk[i];
                var result = EatWhile(current, tk, unit.Substring(i));

                switch (op)
                {
                    case Token.Multiply:
                        if (current == Token.Number)
                            factor *= Math.Pow(ConvertToDouble(result), r);
                        else if (current == Token.Letter)
                            AddUnit(result, r);
                        else
                            throw new ParseException(i, unit.Substring(i));
                        break;

                    case Token.Divide:
                        if (current == Token.Number)
                            factor /= Math.Pow(ConvertToDouble(result), r);
                        else if (current == Token.Letter)
                            AddUnit(result, -r);
                        else
                            throw new ParseException(i, unit.Substring(i));
                        break;

                    case Token.Power:
                        if (current != Token.Number)
                            throw new ParseException(i, unit.Substring(i));

                        var exp = ConvertToInteger(result);
                        AddUnit(lastAddedUnit, r * lastAddedFactor * (exp - 1));
                        break;
                }
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

        string EatWhile(Token token, Token[] tokens, string text)
        {
            var sb = new StringBuilder();
            var index = 0;

            do
            {
                sb.Append(text[index]);
            } while (tokens[++index] == token);

            return sb.ToString();
        }

        double ConvertToDouble(string s)
        {
            var sign = 1.0;

            if (s.LastIndexOf('-') > 0)
                throw new ParseException(s);

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
                throw new ParseException(s);

            if (s.LastIndexOf('.') >= 0)
                throw new ParseException(s);

            if (s[0] == '-')
            {
                sign = -1;
                s = s.Substring(1);
            }

            var d = int.Parse(s, numberFormat);
            return sign * d;
        }

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
            return (ch == 32) ||  // space
                (ch == 9) ||      // horizontal tab
                (ch == 0xB) ||	  // vertical tab
                (ch == 0xC) ||	  // form feed / new page
                (ch == 0xA0);	  // non-breaking space
        }

        #endregion
    }
}
