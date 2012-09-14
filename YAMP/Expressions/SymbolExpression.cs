using System;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;

namespace YAMP
{
	class SymbolExpression : Expression
	{
        static Regex fx;
        FunctionExpression func;

		public static void SetFunctionPattern(string pattern)
		{
			fx = new Regex("^" + pattern);
		}

		public override Expression Create( Match match)
        {
            return new SymbolExpression(match);
        }

        public bool IsSymbol
        {
            get { return func == null; }
        }

        public string SymbolName
        {
            get { return _input; }
        }
		
		public SymbolExpression () : base(@"[A-Za-z]+[A-Za-z0-9]*\b")
		{
		}

		public SymbolExpression (Match match) : this()
		{
		    mx = match;
		}
		
		public override Value Interpret (Hashtable symbols)
		{
			if(func != null)
				return func.Interpret(symbols);

			if(symbols.ContainsKey(_input))
				return new ScalarValue((double)symbols[_input]);

            var variable = Tokens.Instance.GetVariable(_input);

            if (variable != null)
                return variable;
			
			return Tokens.Instance.FindConstants(_input);
		}
		
		public override string ToString ()
		{
			if(func != null)
				return func.ToString();
			
			return base.ToString();
		}
		
		public override string Set (string input)
		{
			var m = fx.Match(input);

			if(m.Success)
			{
				func = new FunctionExpression(m);
				input = func.Set(input);
				_input = func.Input;
				return input;
			}
				
			return base.Set (input);
		}
	}
}

