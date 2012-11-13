using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace YAMP
{
	public abstract class Operator : IRegisterToken
    {
        #region Members

        string _op;
        Type _dependency;

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
            _dependency = typeof(TreeExpression);
			_op = op;
			Level = level;
            ExpectExpression = expect;
		}

        #endregion

        #region Properties

        /// <summary>
        /// Gets the input passed to this operator.
        /// </summary>
		public virtual string Input
		{
			get { return _op; }
		}
		
        /// <summary>
        /// Gets the operator's string.
        /// </summary>
		public string Op
		{
			get { return _op; }
		}
		
        /// <summary>
        /// Gets the level of the operator.
        /// </summary>
		public int Level
        {
            get;
            private set;
		}

        /// <summary>
        /// Gets a value if the operator expects an expression.
        /// </summary>
		public bool ExpectExpression
        {
            get;
            private set;
		}

        /// <summary>
        /// Gets the dependency for this operator.
        /// </summary>
        public Type Dependency
        {
            get { return _dependency; }
        }

        /// <summary>
        /// Gets if the operator has to be executed from right to left for chained scenarios.
        /// </summary>
        public bool IsRightToLeft
        {
            get;
            protected set;
        }

        #endregion

        #region Methods

        public virtual string Set(string input)
		{
            if (input.Length < _op.Length)
                return input;

            for(var i = 0; i < _op.Length; i++)
            {
                if (input[i] != _op[i])
                    return input;
            }

            return input.Substring(_op.Length);
		}

        public virtual Operator Create(QueryContext query)
        {
            return this;
        }

        public virtual Operator Create(QueryContext query, Expression premise)
        {
            return Create(query);
        }

        protected void SetDependency(Type dependency)
        {
            _dependency = dependency;
        }
		
		public abstract Value Evaluate(Expression[] expressions, Dictionary<string, object> symbols);

		#region IRegisterToken implementation
		
		public virtual void RegisterToken()
		{
			Tokens.Instance.AddOperator(_op, this);
		}

        #endregion

        protected bool IsNumeric(Value value)
        {
            return value is NumericValue;
        }
		
		public override string ToString()
		{
			return string.Format ("[ Operator   : {0} ]", _op);
        }

        #endregion
    }
}