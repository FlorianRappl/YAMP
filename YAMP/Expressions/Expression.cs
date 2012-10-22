using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace YAMP
{
	public abstract class Expression : IRegisterToken
	{	
		string _pattern;
        protected string _input;
		protected Match mx;
        int _offset;

        public Expression(string pattern)
        {
            DefaultOperator = string.Empty;
            _pattern = pattern;
        }

		internal string Pattern
		{
			get { return _pattern; }
		}

		internal Match Match
		{
			get { return mx; }
		}
		
		internal int Offset
		{
			get { return _offset; }
			set { _offset = value; }
		}

		internal virtual string Input
		{
			get { return _input; }
		}

        public string DefaultOperator { get; set; }

        public ParseContext Context { get; protected set; }
		
		public Value Interpret()
		{
			return Interpret(new Hashtable());
		}
		
		public abstract Value Interpret(Hashtable symbols);

        public virtual Expression Create(Match match)
        {
            return Create(ParseContext.Default, match);
        }

        public abstract Expression Create(ParseContext context, Match match);

        public virtual string Set(string input)
		{
			_input = mx.Value;
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
			return string.Format ("[ Expression : {0} ]", _input);
		}
    }
}

