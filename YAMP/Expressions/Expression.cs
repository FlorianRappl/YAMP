using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace YAMP
{
	public abstract class Expression : IRegisterToken
    {
        #region Members

        string _pattern;
        protected string _input;
		protected Match mx;
        int _offset;

        #endregion

        #region ctor

        public Expression(string pattern)
        {
            DefaultOperator = string.Empty;
            _pattern = pattern;
        }

        #endregion

        #region Properties

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

        public ParseContext Context { get { return Query.Context; } }

        public QueryContext Query { get; protected set; }

        #endregion

        #region Methods

        public Value Interpret()
		{
            return Interpret(new Dictionary<string, object>());
		}
		
		public abstract Value Interpret(Dictionary<string, object> symbols);

        public abstract Expression Create(QueryContext query, Match match);

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

        #endregion
    }
}

