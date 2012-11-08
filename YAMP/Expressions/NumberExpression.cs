using System;
using System.Globalization;
using System.Collections;
using System.Text.RegularExpressions;

namespace YAMP
{
	class NumberExpression : Expression
	{		
		const NumberStyles style = NumberStyles.Float;
		
		public NumberExpression () : base(@"[-+]?[0-9]*\.?[0-9]+([eE][-+]?[0-9]+)?i?")
		{
		}

		public NumberExpression (QueryContext query, Match match) : this()
		{
            Query = query;
			mx = match;
		}

		public override Expression Create(QueryContext query, Match match)
        {
            return new NumberExpression(query, match);
        }
		
		public override Value Interpret (Hashtable symbols)
		{
			var real = 0.0;
			var imag = 0.0;
			
			if(_input[_input.Length - 1] == 'i')
				imag = double.Parse(_input.Remove(_input.Length - 1), style, Context.NumberFormat);
			else
                real = double.Parse(_input, style, Context.NumberFormat);
			
			return new ScalarValue(real, imag);
		}
	}
}

