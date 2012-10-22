using System;
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

        public TreeExpression(ParseContext context) : base("")
        {
            Context = context;
        }

        public TreeExpression() : this(ParseContext.Default)
        {
        }

        public TreeExpression(Operator op, Expression exp) : this(ParseContext.Default)
        {
            _tree = new ParseTree(op, exp);
        }

        public TreeExpression(Operator op, Expression left, Expression right) : this(ParseContext.Default)
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
        public override Expression Create(ParseContext context, Match match)
        {
            return new TreeExpression(context);
        }

        /// <summary>
        /// Interprets this tree expression.
        /// </summary>
        /// <param name="symbols">The symbols to consider.</param>
        /// <returns>The value of this tree expression.</returns>
        public override Value Interpret(Hashtable symbols)
        {
            return Tree.Interpret(symbols);
        }

        public override string Set(string input)
        {
            _input = input;
            _tree = new ParseTree(Context, _input, Offset, this);
            return string.Empty;
        }

        public override void RegisterToken()
        {
            //Does not register this expression.
        }

        public override string ToString()
        {
            if (Tree != null)
                return Tree.ToString();

            return string.Empty;
        }

        #endregion
    }
}
