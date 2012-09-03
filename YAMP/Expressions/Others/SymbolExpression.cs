using System;
using System.Text.RegularExpressions;
using System.Collections;

namespace YAMP
{
	class SymbolExpression : AbstractExpression
	{	
		static Regex fx;
		
		public static void SetFunctionPattern(string pattern)
		{
			fx = new Regex(pattern);
		}
		
		FunctionExpression func;

        public bool IsSymbol
        {
            get { return func == null; }
        }

        public string SymbolName
        {
            get { return this._input; }
        }
		
		public SymbolExpression () : base(@"[A-Za-z]+[A-Za-z0-9]*\b")
		{
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
			if(fx.IsMatch(input))
			{
				func = new FunctionExpression();
				return func.Set(input);
			}
				
			return base.Set (input);
		}
	}
}

