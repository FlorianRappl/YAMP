using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace YAMP
{
	public abstract class Expression : IRegisterElement
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

		public ParseContext Context { get { return Query.Context; } }

		public QueryContext Query { get; protected set; }

		#endregion

		#region Methods
		
		public abstract Value Interpret(Dictionary<string, Value> symbols);

		public abstract Expression Create(QueryContext query, Match match);

		public virtual string Set(string input)
		{
			_input = mx.Value;
			return input.Substring(_input.Length);
		}
		
		public virtual void RegisterElement ()
		{
			Elements.Instance.AddExpression(_pattern, this);
		}

		public override string ToString ()
		{
            return GetType().Name.Replace("Expression", string.Empty);
		}

		#endregion
	}
}

