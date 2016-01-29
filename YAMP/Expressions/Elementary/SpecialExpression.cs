using System;
using System.Collections.Generic;

namespace YAMP
{
    /// <summary>
    /// This is the class that represents some special expressions (like :).
    /// </summary>
	class SpecialExpression : Expression
    {
        #region Members

        string specialName;
        Func<Dictionary<string, Value>, Value> specialValue;
        static readonly Dictionary<string, Func<Dictionary<string, Value>, Value>> specialExpressions;

        #endregion

        #region ctor

        public SpecialExpression ()
		{
		}

		public SpecialExpression(ParseEngine engine, string name) : base(engine)
		{
            specialName = name;
            Length = name.Length;
		}

        static SpecialExpression()
        {
            specialExpressions = new Dictionary<string, Func<Dictionary<string, Value>, Value>>();
            specialExpressions.Add(":", symbols => new RangeValue());
        }

        #endregion

        #region Methods

        public override Value Interpret(Dictionary<string, Value> symbols)
		{
            return specialValue(symbols);
		}

        public override Expression Scan(ParseEngine engine)
        {
            foreach(var specialExpression in specialExpressions)
            {
                if (Compare(engine.Characters, engine.Pointer, specialExpression.Key))
                {
                    var exp = new SpecialExpression(engine, specialExpression.Key);
                    engine.Advance();
                    exp.specialValue = specialExpression.Value;
                    return exp;
                }
            }

            return null;
        }

        #endregion

        #region String Representations

        public override string ToCode()
        {
            return specialName;
        }

        #endregion

        #region Helpers

        static bool Compare(char[] characters, int index, string value)
        {
            if (characters.Length - index < value.Length)
                return false;

            for (var i = 0; i < value.Length; i++)
            {
                if (value[i] != characters[i + index])
                    return false;
            }

            return true;
        }

        #endregion
    }
}

