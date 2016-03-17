namespace YAMP
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// This is the class that represents some special expressions (like :).
    /// </summary>
	class SpecialExpression : Expression
    {
        #region Fields

        static readonly Dictionary<String, Func<IDictionary<String, Value>, Value>> specialExpressions = new Dictionary<String, Func<IDictionary<String, Value>, Value>>
        {
            { ":", symbols => new RangeValue() }
        };
        readonly String _specialName;
        Func<IDictionary<String, Value>, Value> _specialValue;

        #endregion

        #region ctor

        public SpecialExpression ()
		{
		}

		public SpecialExpression(ParseEngine engine, String name) 
            : base(engine)
		{
            _specialName = name;
            Length = name.Length;
		}

        #endregion

        #region Methods

        public override Value Interpret(IDictionary<String, Value> symbols)
		{
            return _specialValue(symbols);
		}

        public override Expression Scan(ParseEngine engine)
        {
            foreach(var specialExpression in specialExpressions)
            {
                if (Compare(engine.Characters, engine.Pointer, specialExpression.Key))
                {
                    var exp = new SpecialExpression(engine, specialExpression.Key);
                    engine.Advance();
                    exp._specialValue = specialExpression.Value;
                    return exp;
                }
            }

            return null;
        }

        #endregion

        #region String Representations

        public override String ToCode()
        {
            return _specialName;
        }

        #endregion

        #region Helpers

        static Boolean Compare(Char[] characters, Int32 index, String value)
        {
            if (characters.Length - index >= value.Length)
            {
                for (var i = 0; i < value.Length; i++)
                {
                    if (value[i] != characters[i + index])
                    {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }

        #endregion
    }
}

