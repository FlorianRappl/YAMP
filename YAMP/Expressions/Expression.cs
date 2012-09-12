using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
namespace YAMP
{
	public abstract class Expression : IRegisterToken
	{	
		string _pattern;
		protected string _input;
		Regex rx;
		int _offset;
		
		internal Regex SearchExpression
		{
			get { return rx; }
			set { rx = value; }
		}
		
		internal int Offset
		{
			get { return _offset; }
			set { _offset = value; }
		}

		internal string Input
		{
			get { return _input; }
		}
		
		public Expression (string pattern)
		{
			_pattern = pattern;
		}
		
		public Value Interpret()
		{
			return Interpret(new Hashtable());
		}
		
		public abstract Value Interpret(Hashtable symbols);

        public abstract Expression Create();
		
		public virtual string Set(string input)
		{
			var match = rx.Match(input);
			_input = match.Value;
			return input.Substring(_input.Length);
		}

		#region IRegisterToken implementation
		
		public virtual void RegisterToken ()
		{
			Tokens.Instance.AddExpression(_pattern, this);
		}
		
		#endregion
		
		public override string ToString ()
		{
			return string.Format ("{0} [ ExpressionType = {1} ]", _input, GetType().Name.Replace("Expression", string.Empty));
		}
	}
}

