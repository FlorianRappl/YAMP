namespace YAMP
{
    using System;
    using System.Collections.Generic;
    using YAMP.Exceptions;

    /// <summary>
    /// The abstract base class for any binary operator (+, -, *, ...).
    /// </summary>
	public abstract class BinaryOperator : Operator
    {
        #region ctor

        /// <summary>
        /// Creates a new binary operator.
        /// </summary>
        /// <param name="op">The operator string.</param>
        /// <param name="level">The operator level.</param>
        public BinaryOperator(String op, Int32 level)
            : base(op, level)
		{
            Expressions = 2;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Performs the operation with the 2 evaluated values.
        /// </summary>
        /// <param name="left">The left value.</param>
        /// <param name="right">The right value.</param>
        /// <returns>The result of the operation.</returns>
        public abstract Value Perform(Value left, Value right);

        /// <summary>
        /// Handles the evaluation of two expressions.
        /// </summary>
        /// <param name="left">The expression on the left.</param>
        /// <param name="right">The expression on the right.</param>
        /// <param name="symbols">The external symbols to consider.</param>
        /// <returns>The result of the operation.</returns>
		public virtual Value Handle(Expression left, Expression right, IDictionary<String, Value> symbols)
		{
			var l = left.Interpret(symbols);
			var r = right.Interpret(symbols);
			return Perform(l, r);
		}

        public Value PerformOverFind(Value left, Value right, BinaryOperatorMappingList mapping)
        {
            Value ret;
            if (!TryPerformOverFind(left, right, mapping, out ret))
                throw new YAMPOperationInvalidException(Op, left, right);

            return ret;
        }

        public static Boolean TryPerformOverFind(Value left, Value right, BinaryOperatorMappingList mapping, out Value value)
        {
            var least = default(Func<Value, Value, Value>);

            for (var i = 0; i != mapping.Count; i++)
            {
                var hit = mapping[i].IsMapping(left, right);

                if (hit == MapHit.Direct)
                {
                    value = mapping[i].Map(left, right);
                    return true;
                }
                else if (hit == MapHit.Indirect)
                {
                    least = mapping[i].Map;
                }
            }

            if (least != null)
            {
                value = least(left, right);
                return true;
            }

            value = null;
            return false;
        }

        /// <summary>
        /// The implementation of the more general evaluate method.
        /// </summary>
        /// <param name="expressions">The array of expressions, binary operators require Length == 2.</param>
        /// <param name="symbols">The external symbols to consider.</param>
        /// <returns>The result of the operation.</returns>
		public override Value Evaluate(Expression[] expressions, IDictionary<String, Value> symbols)
		{
            if (expressions.Length != 2)
            {
                throw new YAMPArgumentNumberException(Op, expressions.Length, 2);
            }

			return Handle(expressions[0], expressions[1], symbols);
        }

        #endregion
    }
}