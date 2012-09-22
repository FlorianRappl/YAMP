using System;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;

namespace YAMP
{
	class FunctionExpression : Expression
	{
		FunctionDelegate _func;
		string _name;
		BracketExpression _child;
		
		public FunctionExpression () : base(@"[A-Za-z]+[A-Za-z0-9]*\(.*\)")
		{
		}

		public FunctionExpression (Match match) : this()
		{
			mx = match;
		}

		public override Expression Create(Match match)
        {
            return new FunctionExpression(match);
        }
		
		public override Value Interpret (Hashtable symbols)
		{
			var val = _child.Interpret(symbols);
			return _func(val);
		}
		
		public override string Set (string input)
		{
			var sb = new StringBuilder();
            var pos = input.IndexOf('(');
            var isList = false;
			_name = input.Substring(0, pos);
			sb.Append(_name).Append("(");
			_func = Tokens.Instance.FindFunction(_name, out isList);
			_child = new BracketExpression();
			_child.Offset = Offset + pos;
			input = _child.Set(input.Substring(pos), isList);
			sb.Append(_child.Input).Append(")");
			_input = sb.ToString();
			return input;
		}
		
		public override string ToString ()
		{
			var sb = new StringBuilder();
			sb.Append(_name).AppendLine(" [ ExpressionType = Function ]");
			sb.Append(_child.ToString());			
			return sb.ToString();
		}
		
		public override void RegisterToken ()
		{
			SymbolExpression.SetFunctionPattern(Pattern);
		}
	}
}

