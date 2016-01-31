namespace YAMP
{
    using System;

    /// <summary>
    /// Interface for adding operators.
    /// </summary>
    public interface IElementMapping
    {
        /// <summary>
        /// Adds an expression to the list of expressions.
        /// </summary>
        /// <param name="exp">The instance of the expression.</param>
        void AddExpression(Expression exp);
        
        /// <summary>
        /// Adds a keyword to the dictionary.
        /// </summary>
        /// <param name="pattern">The exact keyword pattern, i.e. for for the for-loop.</param>
        /// <param name="keyword">The instance of the keyword.</param>
        void AddKeyword(String pattern, Keyword keyword);
        
        /// <summary>
        /// Adds an operator to the dictionary.
        /// </summary>
        /// <param name="pattern">The operator pattern, i.e. += for add and assign.</param>
        /// <param name="op">The instance of the operator.</param>
        void AddOperator(String pattern, Operator op);
    }
}
