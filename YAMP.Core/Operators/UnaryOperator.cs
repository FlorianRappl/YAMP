namespace YAMP
{
    using System;
    using System.Collections.Generic;
    using YAMP.Exceptions;

    /// <summary>
    /// The abstract base class for every unary operator (!, ', ...)
    /// </summary>
	public abstract class UnaryOperator : Operator
    {
        #region ctor

        /// <summary>
        /// Creates a new unary operator.
        /// </summary>
        /// <param name="op">The operator string.</param>
        /// <param name="level">The operator level.</param>
        public UnaryOperator (String op, Int32 level) : 
            base(op, level)
		{
            Expressions = 1;
		}

        #endregion

        #region Methods

        /// <summary>
        /// Performs the operation with the evaluated value.
        /// </summary>
        /// <param name="value">The value to operate with.</param>
        /// <returns>The result of the operation.</returns>
        public virtual Value Perform(Value value)
        {
            return value;
        }

        /// <summary>
        /// Handles the evaluation of one expression.
        /// </summary>
        /// <param name="expression">The expression on the left.</param>
        /// <param name="symbols">The external symbols to consider.</param>
        /// <returns>The result of the operation.</returns>
		public virtual Value Handle(Expression expression, Dictionary<string, Value> symbols)
		{
			var value = expression.Interpret(symbols);
			return Perform(value);
		}

        /// <summary>
        /// The implementation of the more general evaluate method.
        /// </summary>
        /// <param name="expressions">The array of expressions, unary operators require Length == 1.</param>
        /// <param name="symbols">The external symbols to consider.</param>
        /// <returns>The result of the operation.</returns>
		public override Value Evaluate(Expression[] expressions, Dictionary<string, Value> symbols)
		{
            if (expressions.Length != 1)
                throw new YAMPArgumentNumberException(Op, expressions.Length, 1);

			return Handle(expressions[0], symbols);
        }

        #endregion
    }
}

