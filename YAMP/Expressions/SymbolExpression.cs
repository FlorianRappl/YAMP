using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;

namespace YAMP
{
	class SymbolExpression : Expression
	{
        static Regex fx;
        FunctionExpression func;
		
		public SymbolExpression() : base(@"[A-Za-z]+[A-Za-z0-9_]*\b")
		{
		}

        public SymbolExpression(QueryContext query, Match match) : this()
		{
            Query = query;
		    mx = match;
        }

        public bool IsSymbol
        {
            get { return func == null; }
        }

        public string SymbolName
        {
            get { return _input; }
        }

        public static void SetFunctionPattern(string pattern)
        {
            fx = new Regex("^" + pattern);
        }

        public override Expression Create(QueryContext query, Match match)
        {
            return new SymbolExpression(query, match);
        }
		
		public override Value Interpret(Dictionary<string, object> symbols)
		{
			if(func != null)
				return func.Interpret(symbols);

			if(symbols.ContainsKey(_input))
				return new ScalarValue((double)symbols[_input]);

            var variable = Context.GetVariable(_input);

            if (variable != null)
                return variable;
			
			return Context.FindConstants(_input);
		}
		
		public override string ToString()
		{
			if(func != null)
				return func.ToString();
			
			return base.ToString();
		}
		
		public override string Set(string input)
		{
			var m = fx.Match(input);

			if(m.Success)
			{
                var name = input.Substring(0, input.IndexOf('('));

                if (Context.GetVariable(name) == null)
                {
                    func = new FunctionExpression(Query, m);
                    input = func.Set(input);
                    _input = func.Input;
                    return input;
                }
			}
				
			return base.Set(input);
		}
	}
}

