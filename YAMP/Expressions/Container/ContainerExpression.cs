/*
	Copyright (c) 2012-2014, Florian Rappl.
	All rights reserved.

	Redistribution and use in source and binary forms, with or without
	modification, are permitted provided that the following conditions are met:
		* Redistributions of source code must retain the above copyright
		  notice, this list of conditions and the following disclaimer.
		* Redistributions in binary form must reproduce the above copyright
		  notice, this list of conditions and the following disclaimer in the
		  documentation and/or other materials provided with the distribution.
		* Neither the name of the YAMP team nor the names of its contributors
		  may be used to endorse or promote products derived from this
		  software without specific prior written permission.

	THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
	ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
	WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
	DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
	DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
	(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
	LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
	ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
	(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
	SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System;
using System.Collections.Generic;
using System.Text;

namespace YAMP
{
    /// <summary>
    /// Represents a container for expressions and corresponding operators.
    /// </summary>
    public class ContainerExpression : Expression
    {
        #region Members

        Expression[] _expressions;
        Operator _operator;

        #endregion

        #region ctors

        /// <summary>
        /// Creates a new expression container.
        /// </summary>
        public ContainerExpression()
        {
        }

        /// <summary>
        /// Creates a new expression container.
        /// </summary>
        /// <param name="expression">The (1) expression to contain.</param>
        public ContainerExpression(Expression expression)
        {
            _expressions = new Expression[] { expression };
            _operator = null;
        }

        /// <summary>
        /// Creates a new expression container.
        /// </summary>
        /// <param name="expression">The (1) expression to contain.</param>
        /// <param name="operator">The assigned operator for the expression.</param>
        public ContainerExpression(Expression expression, Operator @operator)
        {
            _expressions = new Expression[] { expression };
            _operator = @operator;
        }

        /// <summary>
        /// Creates a new expression container.
        /// </summary>
        /// <param name="leftExpression">The left expression to evaluate.</param>
        /// <param name="rightExpression">The right expression to evaluate.</param>
        /// <param name="operator">The operator that connects the expressions.</param>
        public ContainerExpression(Expression leftExpression, Expression rightExpression, Operator @operator)
        {
            _expressions = new Expression[] { leftExpression, rightExpression };
            _operator = @operator;
        }

        /// <summary>
        /// Creates a new expression container.
        /// </summary>
        /// <param name="expressions">The expressions to evaluate.</param>
        /// <param name="operator">The operator that connects the expressions.</param>
        public ContainerExpression(Expression[] expressions, Operator @operator)
        {
            _expressions = expressions;
            _operator = @operator;
        }

        /// <summary>
        /// Creates a new expression container.
        /// </summary>
        /// <param name="container">The container which contains expressions and an operator.</param>
        public ContainerExpression(ContainerExpression container)
        {
            _expressions = container._expressions;
            _operator = container._operator;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value if the interpreter has any content (can do interpretation).
        /// </summary>
        public bool HasContent
        {
            get
            {
                return _expressions != null && _expressions.Length > 0;
            }
        }

        /// <summary>
        /// Gets the operator used for this parse tree (can be null).
        /// </summary>
        public Operator Operator
        {
            get
            {
                return _operator;
            }
            set
            {
                _operator = value;
            }
        }

        /// <summary>
        /// Gets the array with all found expressions in the parse tree.
        /// </summary>
        public Expression[] Expressions
        {
            get
            {
                return _expressions;
            }
            set
            {
                _expressions = value;
            }
        }

        /// <summary>
        /// Gets a value if the container holds an assignment.
        /// </summary>
        public bool IsAssignment
        {
            get
            {
                if (_operator != null)
                    return _operator is AssignmentOperator;

                if (_expressions == null || Expressions.Length != 1)
                    return false;

                if (Expressions[0] is ContainerExpression)
                    return ((ContainerExpression)Expressions[0]).IsAssignment;

                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating if the parse tree consists only of symbols that 
        /// are seperated by columns (commas).
        /// </summary>
        public bool IsSymbolList
        {
            get
            {
                if (Expressions == null)
                    return false;

                if (Operator != null && (!(Operator is ColumnOperator) && !(Operator is CommaOperator)))
                    return false;

                foreach (var expression in Expressions)
                {
                    if (expression is ContainerExpression)
                    {
                        if (((ContainerExpression)expression).IsSymbolList)
                            continue;
                    }
                    else if (expression is SymbolExpression)
                        continue;

                    return false;
                }

                return true;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Registers the element in the beginning.
        /// </summary>
        public override void RegisterElement()
        {
            //Nothing here
        }

        /// <summary>
        /// Interprets the container expression.
        /// </summary>
        /// <param name="symbols">External symbols to be used.</param>
        /// <returns>The evaluated value.</returns>
        public override Value Interpret(Dictionary<string, Value> symbols)
        {
            if (_operator != null)
                return _operator.Evaluate(_expressions, symbols);
            else if (_expressions != null && _expressions.Length > 0)
                return _expressions[0].Interpret(symbols);

            return null;
        }

        /// <summary>
        /// Scans the input of the current parse engine.
        /// </summary>
        /// <param name="engine">The engine to use.</param>
        /// <returns>Null, since container expressions cannot be scanned.</returns>
        public override Expression Scan(ParseEngine engine)
        {
            return null;
        }

        /// <summary>
        /// Gets all the symbols of the container.
        /// </summary>
        /// <returns>An array of symbolic expressions.</returns>
        internal SymbolExpression[] GetSymbols()
        {
            var list = new List<SymbolExpression>();

            if (_expressions != null)
            {
                foreach (var expression in _expressions)
                {
                    if (expression is ContainerExpression)
                        list.AddRange(((ContainerExpression)expression).GetSymbols());
                    else if (expression is SymbolExpression)
                        list.Add((SymbolExpression)expression);
                }
            }

            return list.ToArray();
        }

        #endregion

        #region String Representations

        /// <summary>
        /// Transforms the content into a string.
        /// </summary>
        /// <returns>The representative.</returns>
        public override string ToString()
        {
            if (_expressions != null)
            {
                if (_expressions.Length == 1 && _operator == null)
                    return " [ " + _expressions[0].ToString() + " ] ";
                else if (_operator != null)
                {
                    if (_expressions.Length == 1)
                        return " [ " + _expressions[0].ToString() + " " + _operator.ToString() + " ] ";
                    else if (_expressions.Length == 2)
                        return " [ " + _expressions[0].ToString() + " " + _operator.ToString() + " " + _expressions[1].ToString() + " ] ";
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Transforms the contained expressions and operators into a valid part of a YAMP query.
        /// </summary>
        /// <returns>The code.</returns>
        public override string ToCode()
        {
            var sb = new StringBuilder();

            if (_expressions != null)
            {
                if (_expressions.Length > 0)
                    sb.Append(_expressions[0].ToCode());

                if (_operator != null)
                    sb.Append(_operator.ToCode());

                if (_expressions.Length > 1)
                    sb.Append(_expressions[1].ToCode());
            }

            return sb.ToString();
        }

        #endregion
    }
}
