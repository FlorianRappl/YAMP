using System;
using System.Text;
using System.Collections;

namespace YAMP
{
	public abstract class Operator : IRegisterToken
    {
        #region Members

        string _op;
		int _level;
		bool _expect;
		bool _isList;

        #endregion

        #region ctor

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

        #endregion

        #region Properties

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

        #endregion

        #region Methods

        public virtual string Set(string input)
		{
			return input.Substring(_op.Length);
		}

        public abstract Operator Create();
		
		public abstract Value Evaluate(Expression[] expressions, Hashtable symbols);

		#region IRegisterToken implementation
		
		public void RegisterToken ()
		{
			Tokens.Instance.AddOperator(_op, this);
		}

        #endregion

        protected bool IsNumeric(Value value)
        {
            return value is MatrixValue || value is ScalarValue;
        }
		
		public override string ToString ()
		{
			return string.Format ("{0} [ Operator: Level = {1} ]", _op, _level);
        }

        #endregion
    }
}

