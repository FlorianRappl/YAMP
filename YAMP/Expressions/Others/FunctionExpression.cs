using System;
using System.Text;
using System.Collections;

namespace YAMP
{
	class FunctionExpression : AbstractExpression
	{
		FunctionDelegate _func;
		string _name;
		BracketExpression _child;
		const string PATTERN = @"[A-Za-z]+[A-Za-z0-9]*\(.*\)";
		
		public FunctionExpression () : base(PATTERN)
		{
		}
		
		public override Value Interpret (Hashtable symbols)
		{
			var val = _child.Interpret(symbols);
			return _func(val);
		}
		
		public override string Set (string input)
		{
			var pos = input.IndexOf('(');
			_name = input.Substring(0, pos);
			_func = Tokens.Instance.FindFunction(_name);
			_child = new BracketExpression();
			_child.Offset = Offset + pos;
			return _child.Set(input.Substring(pos));
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
			SymbolExpression.SetFunctionPattern(PATTERN);
		}
	}
}

