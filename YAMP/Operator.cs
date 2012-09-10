using System;
using System.Collections;

namespace YAMP
{
	public abstract class Operator : IRegisterToken
	{
		string _op;
		int _level;
		bool _expect;
		bool _isList;
		
		public Operator (string op) : this(op, 0, true)
		{
		}

		public Operator (string op, int level) : this(op, level, true)
		{
		}
		
		public Operator (string op, int level, bool expect)
		{
			_op = op;
			_level = level;
			_expect = expect;
		}

		public bool IsList
		{
			get { return _isList; }
			set { _isList = value; }
		}

		public virtual string Input
		{
			get { return _op; }
		}
		
		public string Op
		{
			get { return _op; }
		}
		
		public int Level
		{
			get { return _level; }
		}

		public bool ExpectExpression
		{
			get { return _expect; }
		}
		
		public virtual string Set(string input)
		{
			return input.Substring(_op.Length);
		}
		
		public abstract Value Evaluate(AbstractExpression[] expressions, Hashtable symbols);

		#region IRegisterToken implementation
		
		public void RegisterToken ()
		{
			Tokens.Instance.AddOperator(_op, GetType());
		}
		
		#endregion
		
		public override string ToString ()
		{
			return string.Format ("{0} [ Operator: Level = {1} ]", _op, _level);
		}
	}
}

