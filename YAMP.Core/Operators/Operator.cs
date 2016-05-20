namespace YAMP
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The abstract base class for any operator (unary, binary, ...).
    /// </summary>
	public abstract class Operator : Block, IRegisterElement
	{
		#region Fields

		String _op;

		#endregion

		#region ctor

        /// <summary>
        /// Creates a new operator given the string for the operator.
        /// </summary>
        /// <param name="op">The operator string like +.</param>
		public Operator (String op) : 
            this(op, 0)
		{
		}
		
        /// <summary>
        /// Creates a new operator given the string and level for the operator.
        /// </summary>
        /// <param name="op">The operator string like +.</param>
        /// <param name="level">The operator level like 100.</param>
		public Operator (String op, Int32 level)
		{
			_op = op;
			Level = level;
            Length = op.Length;
		}

		#endregion

        #region Properties

        /// <summary>
        /// Gets a dummy operator for doing nothing.
        /// </summary>
        public static Operator Void
        {
            get { return new VoidOperator(); }
        }

        /// <summary>
        /// Gets how many expressions are eaten by thix operator (1 = unary, 2 = binary, ...).
        /// </summary>
        public Int32 Expressions
        {
            get;
            protected set;
        }
		
		/// <summary>
		/// Gets the operator's string.
		/// </summary>
		public String Op
		{
			get { return _op; }
		}
		
		/// <summary>
		/// Gets the level of the operator.
		/// </summary>
        public Int32 Level
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets if the operator has to be executed from right to left for chained scenarios.
		/// </summary>
		public Boolean IsRightToLeft
		{
			get;
			protected set;
        }

		#endregion

		#region Methods

        /// <summary>
        /// Begins the evaluation of given expressions.
        /// </summary>
        /// <param name="expressions">The expressions to evaluate.</param>
        /// <param name="symbols">External symbols to consider.</param>
        /// <returns>The result of the evaluation.</returns>
        public abstract Value Evaluate(Expression[] expressions, IDictionary<String, Value> symbols);

        /// <summary>
        /// Creates a new instance of the current operator.
        /// </summary>
        /// <returns>The new instance.</returns>
        public abstract Operator Create();

        /// <summary>
        /// Creates a new instance of the current operator.
        /// </summary>
        /// <param name="engine">The engine that is used for parsing the query.</param>
        /// <returns>The new instance.</returns>
        public virtual Operator Create(ParseEngine engine)
        {
            var op = Create();
            op.Query = engine.Query;
            op.StartColumn = engine.CurrentColumn;
            op.StartLine = engine.CurrentLine;
            engine.Advance(Op.Length);
            return op;
        }
		
        /// <summary>
        /// Registers the operator at its factory.
        /// </summary>
        public virtual void RegisterElement(IElementMapping elementMapping)
		{
			elementMapping.AddOperator(_op, this);
        }

        #endregion

        #region String Representations

        /// <summary>
        /// Returns the string representation of the operator.
        /// </summary>
        /// <returns>A string.</returns>
        public override String ToString()
        {
            return String.Format("({0}, {1}) {2}", StartLine, StartColumn, GetType().Name.RemoveOperatorConvention());
		}

        /// <summary>
        /// The code representation of the operator, which is usually just the operator itself.
        /// </summary>
        /// <returns>A valid part of a YAMP query.</returns>
        public override String ToCode()
        {
            return _op;
        }

		#endregion
	}
}