using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;

namespace YAMP
{
	class SymbolExpression : Expression
    {
        #region Members

        static readonly string symbolRegex = @"[A-Za-z]+[A-Za-z0-9_]*\b";

        #endregion

        #region ctor

        public SymbolExpression() : base(symbolRegex)
		{
		}

		public SymbolExpression(QueryContext query, Match match) : this()
		{
			Query = query;
			mx = match;
		}

        #endregion

        #region Properties

        public static string SymbolRegularExpression
        {
            get { return symbolRegex; }
        }

		public string SymbolName
		{
			get { return _input; }
		}

        #endregion

        #region Methods

        public override Expression Create(QueryContext query, Match match)
		{
			return new SymbolExpression(query, match);
		}

		public override Value Interpret(Dictionary<string, Value> symbols)
		{
            if (symbols.ContainsKey(_input))
                return symbols[_input];

            var variable = Context.GetVariable(_input);

            if (variable != null)
                return variable;

            var constant = Context.FindConstants(_input);

            if (constant != null)
                return constant.Value;

            var function = Context.FindFunction(_input);

            if (function != null)
                return new FunctionValue(function);
            
            throw new SymbolException(_input);
        }

        #endregion
	}
}

