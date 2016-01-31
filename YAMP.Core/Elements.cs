namespace YAMP
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Provides internal access to the elements and handles the element registration and variable assignment.
    /// </summary>
    public sealed class Elements
    {
        #region Fields

        readonly IDictionary<String, Operator> _binaryOperators;
        readonly IDictionary<String, Operator> _unaryOperators;
        readonly List<Expression> _expressions;
        readonly IDictionary<String, Keyword> _keywords;
        readonly IDictionary<Guid, Plugin> _plugins;

        #endregion

        #region ctor

        public Elements(ParseContext context)
        {
            _binaryOperators = new Dictionary<String, Operator>();
            _unaryOperators = new Dictionary<String, Operator>();
            _expressions = new List<Expression>();
            _keywords = new Dictionary<String, Keyword>();
            _plugins = new Dictionary<Guid, Plugin>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the function loader, if any.
        /// </summary>
        public IEnumerable<IFunctionLoader> Loaders
        {
            get { return _plugins.SelectMany(m => m.Value.Loaders); }
        }

        /// <summary>
        /// Gets the list of possible keywords.
        /// </summary>
        public String[] Keywords
        {
            get { return _keywords.Keys.ToArray(); }
        }

        #endregion

        #region Register elements

        /// <summary>
        /// Registers the IFunction, IConstant and IRegisterToken token classes at the specified context.
        /// </summary>
        /// <param name="context">
        /// The context where the IFunction and IConstant instances will be placed.
        /// </param>
        /// <param name="assembly">
        /// The assembly to load.
        /// </param>
        /// <returns>The ID for the assembly.</returns>
        public Guid RegisterAssembly(ParseContext context, Assembly assembly)
        {
            var plugin = new Plugin(context, assembly);
            plugin.Install();
            _plugins.Add(plugin.Id, plugin);
            return plugin.Id;
        }

        /// <summary>
        /// Removes a previously added assembly.
        /// </summary>
        /// <param name="pluginId">The id of the plugin to remove.</param>
        public void RemoveAssembly(Guid pluginId)
        {
            if (_plugins.ContainsKey(pluginId))
            {
                var plugin = _plugins[pluginId];
                plugin.Uninstall();
                _plugins.Remove(pluginId);
            }
        }

        #endregion

        #region Add elements

        /// <summary>
        /// Adds an operator to the dictionary.
        /// </summary>
        /// <param name="pattern">The operator pattern, i.e. += for add and assign.</param>
        /// <param name="op">The instance of the operator.</param>
        public void AddOperator(String pattern, Operator op)
        {
            if (!op.IsRightToLeft && op.Expressions == 1)
            {
                _unaryOperators.Add(pattern, op);
            }
            else
            {
                _binaryOperators.Add(pattern, op);
            }
        }

        /// <summary>
        /// Adds an expression to the list of expressions.
        /// </summary>
        /// <param name="exp">The instance of the expression.</param>
        public void AddExpression(Expression exp)
        {
            _expressions.Add(exp);
        }

        /// <summary>
        /// Adds a keyword to the dictionary.
        /// </summary>
        /// <param name="pattern">The exact keyword pattern, i.e. for for the for-loop.</param>
        /// <param name="keyword">The instance of the keyword.</param>
        public void AddKeyword(String pattern, Keyword keyword)
        {
            _keywords.Add(pattern, keyword);
        }

        #endregion

        #region Find elements

        /// <summary>
        /// Searches for the given keyword in the list of available keywords. Creates a class if the keyword is found.
        /// </summary>
        /// <param name="keyword">The keyword to look for.</param>
        /// <param name="engine">The engine to use.</param>
        /// <returns>Keyword that matches the given keyword.</returns>
        public Expression FindKeywordExpression(String keyword, ParseEngine engine)
        {
            if (_keywords.ContainsKey(keyword))
            {
                return _keywords[keyword].Scan(engine);
            }

            return null;
        }

        /// <summary>
        /// Finds the exact keyword by its type.
        /// </summary>
        /// <typeparam name="T">The type of the keyword.</typeparam>
        /// <returns>The keyword or null.</returns>
        public T FindKeywordExpression<T>()
            where T : Keyword
        {
            foreach (var keyword in _keywords.Values)
            {
                if (keyword is T)
                {
                    return (T)keyword;
                }
            }

            return null;
        }

        /// <summary>
        /// Finds the closest matching expression.
        /// </summary>
        /// <param name="engine">The engine to parse the query.</param>
        /// <returns>Expression that matches the current characters.</returns>
        public Expression FindExpression(ParseEngine engine)
        {
            foreach (var origin in _expressions)
            {
                var exp = origin.Scan(engine);

                if (exp != null)
                {
                    return exp;
                }
            }

            return null;
        }

        /// <summary>
        /// Finds the exact expression by its type.
        /// </summary>
        /// <typeparam name="T">The type of the expression.</typeparam>
        /// <returns>The expression or null.</returns>
        public T FindExpression<T>()
            where T : Expression
        {
            foreach (var exp in _expressions)
            {
                if (exp is T)
                {
                    return (T)exp;
                }
            }

            return null;
        }

        /// <summary>
        /// Finds the closest matching operator (all except left unary).
        /// </summary>
        /// <param name="engine">The engine to parse the query.</param>
        /// <returns>Operator that matches the current characters.</returns>
        public Operator FindOperator(ParseEngine engine)
        {
            var maxop = FindArbitraryOperator(_binaryOperators.Keys, engine);

            if (maxop.Length == 0)
            {
                return null;
            }

            return _binaryOperators[maxop].Create(engine);
        }

        /// <summary>
        /// Finds the closest matching left unary operator.
        /// </summary>
        /// <param name="engine">The engine to parse the query.</param>
        /// <returns>Operator that matches the current characters.</returns>
        public Operator FindLeftUnaryOperator(ParseEngine engine)
        {
            var maxop = FindArbitraryOperator(_unaryOperators.Keys, engine);

            if (maxop.Length == 0)
            {
                return null;
            }

            return _unaryOperators[maxop].Create(engine);
        }

        String FindArbitraryOperator(IEnumerable<String> operators, ParseEngine engine)
        {
            var maxop = String.Empty;
            var notfound = true;
            var chars = engine.Characters;
            var ptr = engine.Pointer;
            var rest = chars.Length - ptr;

            foreach (var op in operators)
            {
                if (op.Length <= rest && op.Length > maxop.Length)
                {
                    notfound = false;

                    for (var i = 0; !notfound && i < op.Length; i++)
                    {
                        notfound = (chars[ptr + i] != op[i]);
                    }

                    if (!notfound)
                    {
                        maxop = op;
                    }
                }
            }

            return maxop;
        }

        /// <summary>
        /// Finds the exact operator by its type.
        /// </summary>
        /// <typeparam name="T">The type of the operator.</typeparam>
        /// <returns>The operator or null.</returns>
        public T FindOperator<T>()
            where T : Operator
        {
            foreach (var op in _binaryOperators.Values)
            {
                if (op is T)
                {
                    return (T)op;
                }
            }

            foreach (var op in _unaryOperators.Values)
            {
                if (op is T)
                {
                    return (T)op;
                }
            }

            return null;
        }

        #endregion
    }
}