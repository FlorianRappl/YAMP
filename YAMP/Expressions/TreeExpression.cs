using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Collections;

namespace YAMP
{
    class TreeExpression : Expression
    {
        #region Members

        ParseTree _tree;

        #endregion

        #region ctor

        public TreeExpression(QueryContext query) : base("")
        {
            Query = query;
        }

        public TreeExpression(Operator op, Expression exp) : base("")
        {
            _tree = new ParseTree(op, exp);
        }

        public TreeExpression(Operator op, Expression left, Expression right) : base("")
        {
            _tree = new ParseTree(op, left, right);
        }

        protected TreeExpression(string pattern) : base(pattern)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the expression tree.
        /// </summary>
        public ParseTree Tree
        {
            get { return _tree; }
            protected set { _tree = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the tree expression.
        /// </summary>
        /// <param name="context">The context of the created tree expression.</param>
        /// <param name="match">The match that corresponds to this tree expression.</param>
        /// <returns>The created three expression.</returns>
        public override Expression Create(QueryContext query, Match match)
        {
            return new TreeExpression(query);
        }

        /// <summary>
        /// Interprets this tree expression.
        /// </summary>
        /// <param name="symbols">The symbols to consider.</param>
        /// <returns>The value of this tree expression.</returns>
        public override Value Interpret(Dictionary<string, Value> symbols)
        {
            return Tree.Interpret(symbols);
        }

        public override string Set(string input)
        {
            _input = input;
            _tree = new ParseTree(Query, _input, Offset);
            return string.Empty;
        }

        public override string ToString()
        {
            if (Tree != null)
                return Tree.ToString();

            return base.ToString();
        }

        #endregion
    }
}
