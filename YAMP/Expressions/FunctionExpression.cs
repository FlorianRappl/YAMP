using System;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;

namespace YAMP
{
	class FunctionExpression : Expression
	{
		string _name;
		BracketExpression _child;
        IFunction _func;
		
		public FunctionExpression() : base(@"[A-Za-z]+[A-Za-z0-9_]*\(.*\)")
		{
		}

		public FunctionExpression(ParseContext context, Match match) : this()
		{
            Context = context;
			mx = match;
		}

		public override Expression Create(ParseContext context, Match match)
        {
            return new FunctionExpression(context, match);
        }
		
		public override Value Interpret(Hashtable symbols)
		{
			var val = _child.Interpret(symbols);
			return _func.Perform(Context, val);
		}
		
		public override string Set(string input)
		{
			var sb = new StringBuilder();
            var pos = input.IndexOf('(');
			_name = input.Substring(0, pos);
			sb.Append(_name).Append("(");
            _func = Context.FindFunction(_name);
            _child = new ArgumentsBracketExpression(Context);
			_child.Offset = Offset + pos;
			input = _child.Set(input.Substring(pos));
			sb.Append(_child.Input).Append(")");
			_input = sb.ToString();
			return input;
		}
		
		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.Append(_name).AppendLine(" [ ExpressionType = Function ]");
			sb.Append(_child.ToString());			
			return sb.ToString();
		}
		
		public override void RegisterToken()
		{
			SymbolExpression.SetFunctionPattern(Pattern);
		}
	}
}

