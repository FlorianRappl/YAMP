namespace YAMP
{
    using System;
    using System.Collections.Generic;
    using YAMP.Exceptions;

    /// <summary>
    /// Operator for arguments () for symbols (usually functions!).
    /// </summary>
    class ArgsOperator : RightUnaryOperator
    {
        #region Fields

        readonly Expression _content;

		#endregion

		#region ctor

		public ArgsOperator() : 
            this(null)
        {
        }

        public ArgsOperator(Expression content) :
            base("(", 1000)
        {
            _content = content;
        }

		#endregion

		#region Creation

		public override Operator Create()
        {
            return new ArgsOperator();
        }

        public override Operator Create(ParseEngine engine)
        {
            var start = engine.Pointer;

            //Arguments need to be attached directly.
            if (start != 0 && !ParseEngine.IsWhiteSpace(engine.Characters[start - 1]) && !ParseEngine.IsNewLine(engine.Characters[start - 1]))
            {
                var query = engine.Query;
                var line = engine.CurrentLine;
                var column = engine.CurrentColumn;
                var content = engine.Elements.FindExpression<BracketExpression>().Scan(engine);
                var length = engine.Pointer - start;

                return new ArgsOperator(content)
                {
                    Query = query,
                    StartLine = line,
                    StartColumn = column,
                    Length = length
                };
            }

            return null;
        }

        #endregion

        #region Properties

        public Expression Content
        {
            get { return _content; }
        }

        #endregion

        #region Methods

        public override Value Perform(Value left)
        {
            return left;
        }

        public override Value Handle(Expression expression, IDictionary<String, Value> symbols)
        {
            var args = _content.Interpret(symbols);
            var left = expression.Interpret(symbols);

            if (left is IFunction)
            {
                return ((IFunction)left).Perform(Context, args);
            }

            if (expression is SymbolExpression)
            {
                throw new YAMPFunctionMissingException(((SymbolExpression)expression).SymbolName);
            }

            throw new YAMPExpressionNoFunctionException();
        }

        public Value Handle(Expression expression, Value value, IDictionary<String, Value> symbols)
        {
            var symbolName = String.Empty;
            var isSymbol = expression is SymbolExpression;
            var args = _content.Interpret(symbols);

            if (isSymbol)
            {
                var sym = (SymbolExpression)expression;
                symbolName = sym.SymbolName;

                if (Context.GetVariableContext(sym.SymbolName) == null)
                {
                    Context.AssignVariable(sym.SymbolName, new MatrixValue());
                }
            }

            var left = expression.Interpret(symbols);

            if (left is ISetFunction)
            {
                var sf = (ISetFunction)left;
                sf.Perform(Context, args, value);

                if (isSymbol)
                {
                    Context.AssignVariable(symbolName, left);
                }

                return left;
            }

            if (expression is SymbolExpression)
            {
                throw new YAMPFunctionMissingException(((SymbolExpression)expression).SymbolName);
            }

            throw new YAMPExpressionNoFunctionException();
        }

        #endregion

        #region String Representations

        public override String ToString()
        {
            return _content.ToString();
        }

        public override String ToCode()
        {
            return "(" + _content.ToCode() + ")";
        }

        /// <summary>
        /// Returns a string to allow visualization of a Expression tree
        /// </summary>
        /// <returns>The string that represents the part of the expression tree element.</returns>
        public override String ToDebug(int padLeft, int tabsize)
        {
            string baseDebug = base.ToDebug(padLeft, tabsize);

            string pad = new string(' ', padLeft);
            string tab = new string(' ', tabsize);

            return string.Format(
                "{2}" + Environment.NewLine +
                "{0}{1}<" + Environment.NewLine +
                "{3}" + Environment.NewLine +
                "{0}{1}>", pad, tab, baseDebug, _content.ToDebug(padLeft + 3 * tabsize, tabsize));
        }

        #endregion
    }
}