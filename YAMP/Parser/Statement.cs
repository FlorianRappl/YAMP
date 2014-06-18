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
    /// The class represents a (usually) line of statement or another
    /// self-contained block of expressions and operators.
    /// </summary>
    public class Statement
    {
        #region Members

        List<YAMPParseError> errors;
        Stack<Expression> _expressions;
        Stack<Operator> _operators;
        int maxLevel;
        bool takeOperator;
        bool finalized;
        ContainerExpression _container;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new statement.
        /// </summary>
        public Statement()
        {
            maxLevel = -100;
            errors = new List<YAMPParseError>();
            _expressions = new Stack<Expression>();
            _operators = new Stack<Operator>();
            takeOperator = false;
            finalized = false;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the expression (and operator) container.
        /// </summary>
        public ContainerExpression Container
        {
            get { return _container; }
        }

        /// <summary>
        /// Gets a boolean if the statement was actually muted (terminated with a colon ;).
        /// </summary>
        public bool IsMuted 
        { 
            get; 
            internal set; 
        }

        /// <summary>
        /// Gets a value indicating if the statement consists of one keyword.
        /// </summary>
        public bool IsFinished
        {
            get;
            private set; 
        }

        /// <summary>
        /// Gets a value indicating if the statement is empty.
        /// </summary>
        public bool IsEmpty
        {
            get { return _expressions.Count == 0 && _operators.Count == 0 && (!finalized || !_container.HasContent); }
        }

        /// <summary>
        /// Gets a value marking if its the turn of an operator (or not).
        /// </summary>
        internal bool IsOperator
        {
            get { return takeOperator; }
        }

        /// <summary>
        /// Gets a value indicating if the statement will be assigned to a value.
        /// </summary>
        public bool IsAssignment
        {
            get { return _container.IsAssignment; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Checks if the statement is a special kind of keyword.
        /// </summary>
        /// <typeparam name="T">The keyword to search for.</typeparam>
        /// <returns>A boolean to indicate the search result.</returns>
        internal bool IsKeyword<T>() where T : Keyword
        {
            if (_container.Expressions == null)
                return false;

            if (_container.Expressions.Length != 1)
                return false;

            if (_container.Expressions[0] is T)
                return true;

            return false;
        }

        /// <summary>
        /// Gets the specified keyword statement.
        /// </summary>
        /// <typeparam name="T">The type of keyword to retrieve.</typeparam>
        /// <returns>The keyword instance.</returns>
        internal T GetKeyword<T>() where T: Keyword
        {
            if (_container.Expressions == null)
                return null;

            if (_container.Expressions.Length != 1)
                return null;

            if (_container.Expressions[0] is T)
                return (T)_container.Expressions[0];

            return null;
        }

        /// <summary>
        /// Pushes an operator to the stack.
        /// </summary>
        /// <param name="engine">The current parse engine.</param>
        /// <param name="_operator">The operator to add.</param>
        /// <returns>The current instance.</returns>
        internal Statement Push(ParseEngine engine, Operator _operator)
        {
            if (finalized)
                return this;

            _operator = _operator ?? Operator.Void;

            if (_operator.Expressions == 1 && _operator.IsRightToLeft)
            {
                _expressions.Push(new ContainerExpression(_expressions.Pop(), _operator));
            }
            else
            {
                if (_operator.Expressions == 2)
                {
                    if (_operator.Level >= (_operator.IsRightToLeft ? maxLevel : maxLevel + 1))
                        maxLevel = _operator.Level;
                    else
                    {
                        while (true)
                        {
                            var count = _operators.Peek().Expressions;

                            if (count > _expressions.Count)
                            {
                                errors.Add(new YAMPExpressionMissingError(_operator.StartLine, _operator.StartColumn));
                                break;
                            }

                            var exp = new Expression[count];

                            for (var i = count - 1; i >= 0; i--)
                                exp[i] = _expressions.Pop();

                            var container = new ContainerExpression(exp, _operators.Pop());
                            _expressions.Push(container);
                            ReduceUnary(container);

                            if (_operators.Count > 0 && _operators.Peek().Level > _operator.Level)
                                continue;

                            maxLevel = _operator.Level;
                            break;
                        }
                    }
                }

                _operators.Push(_operator);
                takeOperator = false;
            }

            return this;
        }

        void ReduceUnary(ContainerExpression container)
        {
            while (_operators.Count != 0 && _operators.Peek().Expressions == 1 && _operators.Peek().Level >= container.Operator.Level)
            {
                var e = container.Expressions[0];
                container.Expressions[0] = new ContainerExpression(e, _operators.Pop());
            }
        }

        /// <summary>
        /// Pushes an expression to the stack.
        /// </summary>
        /// <param name="engine">The current parse engine.</param>
        /// <param name="_expression">The expression to add.</param>
        /// <returns>The current instance.</returns>
        internal Statement Push(ParseEngine engine, Expression _expression)
        {
            if (finalized)
                return this;

            if (_expressions.Count == 0)
                IsFinished = _expression == null ? false : _expression.IsSingleStatement;

            _expressions.Push(_expression ?? Expression.Empty);
            takeOperator = true;
            return this;
        }

        /// <summary>
        /// Finalizes the statement by analyzing the contained objects and creating
        /// the container.
        /// </summary>
        /// <param name="engine">The current parse engine.</param>
        /// <returns>The current (finalized) instance.</returns>
        internal Statement Finalize(ParseEngine engine)
        {
            if (finalized)
                return this;

            if (errors.Count != 0)
            {
                foreach (var error in errors)
                    engine.AddError(error);

                return this;
            }

            if (_expressions.Count == 0 && _operators.Count > 0)
            {
                engine.AddError(new YAMPExpressionMissingError(engine));
                return this;
            }

            while (_operators.Count > 0)
            {
                var op = _operators.Pop();
                var exp = new Expression[op.Expressions];

                if (_expressions.Count < op.Expressions)
                {
                    engine.AddError(new YAMPExpressionMissingError(engine, op, _expressions.Count));
                    return this;
                }

                for (var i = op.Expressions - 1; i >= 0; i--)
                    exp[i] = _expressions.Pop();

                var container = new ContainerExpression(exp, op);
                _expressions.Push(container);
                ReduceUnary(container);
            }

            if (_expressions.Count == 1)
                _container = new ContainerExpression(_expressions.Pop());
            else
                _container = new ContainerExpression();

            finalized = true;
            return this;
        }

        /// <summary>
        /// Interprets the statement.
        /// </summary>
        /// <param name="symbols">Additional symbols to consider.</param>
        /// <returns>The result of the evaluation.</returns>
        public Value Interpret(Dictionary<string, Value> symbols)
        {
            if (finalized)
            {
                var val = _container.Interpret(symbols);
                return IsMuted ? null : val;
            }

            return null;
        }

        #endregion

        #region General stuff

        /// <summary>
        /// Returns the string representation of the included objects.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString()
        {
            //CHANGED
            return string.Empty;
            //return _container.ToString();
        }

        /// <summary>
        /// Transforms the statement into executable code.
        /// </summary>
        /// <returns>The code of the statement as a string.</returns>
        public string ToCode()
        {
            //CHANGED
            return string.Empty;
            //return Container.ToCode() + ";";
        }

        #endregion
    }
}
