using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
namespace YAMP
{
	public abstract class AbstractExpression : IRegisterToken
	{	
		string _pattern;
		protected string _input;
		
		public AbstractExpression (string pattern)
		{
			_pattern = pattern;
		}
		
		public Value Interpret()
		{
			return Interpret(new Hashtable());
		}
		
		public abstract Value Interpret(Hashtable symbols);
		
		public virtual string Set(string input)
		{
			var rx = new Regex("^" + _pattern);
			var match = rx.Match(input);
			_input = match.Value;
			return input.Substring(_input.Length);
		}

		#region IRegisterToken implementation
		
		public virtual void RegisterToken ()
		{
			Tokens.Instance.AddExpression(_pattern, GetType());
		}
		
		#endregion
		
		public override string ToString ()
		{
			return string.Format ("{0} [ ExpressionType = {1} ]", _input, GetType().Name.Replace("Expression", string.Empty));
		}
	}
}

