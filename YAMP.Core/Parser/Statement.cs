namespace YAMP
{
    using System;
    using System.Collections.Generic;
    using YAMP.Errors;

    /// <summary>
    /// The class represents a (usually) line of statement or another
    /// self-contained block of expressions and operators.
    /// </summary>
    public class Statement
    {
        #region Fields

        List<YAMPParseError> errors;
        Stack<Expression> _expressions;
        Stack<Operator> _operators;
        Int32 _maxLevel;
        Boolean _takeOperator;
        Boolean _finalized;
        ContainerExpression _container;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new statement.
        /// </summary>
        public Statement()
        {
            _maxLevel = -100;
            errors = new List<YAMPParseError>();
            _expressions = new Stack<Expression>();
            _operators = new Stack<Operator>();
            _takeOperator = false;
            _finalized = false;
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
        public Boolean IsMuted 
        { 
            get; 
            internal set; 
        }

        /// <summary>
        /// Gets a value indicating if the statement consists of one keyword.
        /// </summary>
        public Boolean IsFinished
        {
            get;
            private set; 
        }

        /// <summary>
        /// Gets a value indicating if the statement is empty.
        /// </summary>
        public Boolean IsEmpty
        {
            get { return _expressions.Count == 0 && _operators.Count == 0 && (!_finalized || !_container.HasContent); }
        }

        /// <summary>
        /// Gets a value marking if its the turn of an operator (or not).
        /// </summary>
        internal Boolean IsOperator
        {
            get { return _takeOperator; }
        }

        /// <summary>
        /// Gets a value indicating if the statement will be assigned to a value.
        /// </summary>
        public Boolean IsAssignment
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
        internal Boolean IsKeyword<T>() where T : Keyword
        {
            if (_container.Expressions != null)
            {
                if (_container.Expressions.Length == 1 && _container.Expressions[0] is T)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Gets the specified keyword statement.
        /// </summary>
        /// <typeparam name="T">The type of keyword to retrieve.</typeparam>
        /// <returns>The keyword instance.</returns>
        internal T GetKeyword<T>() where T: Keyword
        {
            if (_container.Expressions != null)
            {
                if (_container.Expressions.Length == 1 && _container.Expressions[0] is T)
                {
                    return (T)_container.Expressions[0];
                }
            }

            return null;
        }

        /// <summary>
        /// Pushes an operator to the stack.
        /// </summary>
        /// <param name="engine">The current parse engine.</param>
        /// <param name="operator">The operator to add.</param>
        /// <returns>The current instance.</returns>
        internal Statement Push(ParseEngine engine, Operator @operator)
        {
            if (!_finalized)
            {
                @operator = @operator ?? Operator.Void;

                if (@operator.Expressions == 1 && @operator.IsRightToLeft)
                {
                    if (_maxLevel > @operator.Level)
                    {
                        PopExpressions(@operator);
                    }

                    _expressions.Push(new ContainerExpression(_expressions.Pop(), @operator));
                }
                else
                {
                    if (@operator.Expressions == 2)
                    {
                        if (@operator.Level >= (@operator.IsRightToLeft ? _maxLevel : _maxLevel + 1))
                        {
                            _maxLevel = @operator.Level;
                        }
                        else
                        {
                            PopExpressions(@operator);
                        }
                    }

                    _operators.Push(@operator);
                    _takeOperator = false;
                }
            }

            return this;
        }

        void PopExpressions(Operator @operator)
        {
            while (true)
            {
                var count = _operators.Peek().Expressions;

                if (count > _expressions.Count)
                {
                    errors.Add(new YAMPExpressionMissingError(@operator.StartLine, @operator.StartColumn));
                    break;
                }

                var exp = new Expression[count];

                for (var i = count - 1; i >= 0; i--)
                {
                    exp[i] = _expressions.Pop();
                }

                var container = new ContainerExpression(exp, _operators.Pop());
                _expressions.Push(container);
                ReduceUnary(container);

                if (_operators.Count <= 0 || _operators.Peek().Level < @operator.Level)
                {
                    _maxLevel = @operator.Level;
                    break;
                }
            }
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
            if (_finalized)
            {
                return this;
            }

            if (_expressions.Count == 0)
            {
                IsFinished = _expression == null ? false : _expression.IsSingleStatement;
            }

            _expressions.Push(_expression ?? Expression.Empty);
            _takeOperator = true;
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
            if (_finalized)
            {
                return this;
            }

            if (errors.Count != 0)
            {
                foreach (var error in errors)
                {
                    engine.AddError(error);
                }

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
                {
                    exp[i] = _expressions.Pop();
                }

                var container = new ContainerExpression(exp, op);
                _expressions.Push(container);
                ReduceUnary(container);
            }

            if (_expressions.Count == 1)
            {
                _container = new ContainerExpression(_expressions.Pop());
            }
            else
            {
                _container = new ContainerExpression();
            }

            _finalized = true;
            return this;
        }

        /// <summary>
        /// Interprets the statement.
        /// </summary>
        /// <param name="symbols">Additional symbols to consider.</param>
        /// <returns>The result of the evaluation.</returns>
        public Value Interpret(IDictionary<String, Value> symbols)
        {
            if (_finalized)
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
