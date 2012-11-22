using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace YAMP
{
    class UnaryExpression : Expression
    {
        #region Members

        static readonly ScalarValue minusOne = new ScalarValue(-1.0, 0.0);

        #endregion

        #region ctors

        public UnaryExpression() : base(@"[\+\-]+")
        {
        }

        public UnaryExpression(QueryContext query) : this()
        {
            Query = query;
        }

        #endregion

        #region Methods

        public override Expression Create(QueryContext query, Match match)
        {
            return new UnaryExpression(query);
        }

        public override Value Interpret(Dictionary<string, Value> symbols)
        {
            return minusOne;
        }

        public override string Set(string input)
        {
            _input = string.Empty;
            return "*" + input.Substring(1);
        }

        #endregion
    }
}
