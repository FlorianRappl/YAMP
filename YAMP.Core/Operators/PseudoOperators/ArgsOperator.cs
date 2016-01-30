using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace YAMP
{
    /// <summary>
    /// Operator for arguments () for symbols (usually functions!).
    /// </summary>
    class ArgsOperator : RightUnaryOperator
    {
        #region Fields

        Expression _content;

		#endregion

		#region ctor

		public ArgsOperator() : base("(", 1000)
        {
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
                var ao = new ArgsOperator();
                ao.Query = engine.Query;
                ao.StartLine = engine.CurrentLine;
                ao.StartColumn = engine.CurrentColumn;
                ao._content = engine.Elements.FindExpression<BracketExpression>().Scan(engine);
                ao.Length = engine.Pointer - start;
                return ao;
            }

            return null;
        }

        #endregion

        #region Methods

        public override Value Perform(Value left)
        {
            return left;
        }

        public override Value Handle(Expression expression, Dictionary<string, Value> symbols)
        {
            var args = _content.Interpret(symbols);
            var left = expression.Interpret(symbols);

            if(left is IFunction)
                return ((IFunction)left).Perform(Context, args);

            if(expression is SymbolExpression)
                throw new YAMPFunctionMissingException(((SymbolExpression)expression).SymbolName);

            throw new YAMPExpressionNoFunctionException();
        }

        public Value Handle(Expression expression, Value value, Dictionary<string, Value> symbols)
        {
            var symbolName = string.Empty;
            var isSymbol = expression is SymbolExpression;
            var args = _content.Interpret(symbols);

            if (isSymbol)
            {
                var sym = (SymbolExpression)expression;
                symbolName = sym.SymbolName;

                if (Context.GetVariableContext(sym.SymbolName) == null)
                    Context.AssignVariable(sym.SymbolName, new MatrixValue());
            }

            var left = expression.Interpret(symbols);

            if (left is ISetFunction)
            {
                var sf = (ISetFunction)left;
                sf.Perform(Context, args, value);

                if (isSymbol)
                    Context.AssignVariable(symbolName, left);

                return left;
            }

            if (expression is SymbolExpression)
                throw new YAMPFunctionMissingException(((SymbolExpression)expression).SymbolName);

            throw new YAMPExpressionNoFunctionException();
        }

        #endregion

        #region String Representations

        public override string ToString()
        {
            return _content.ToString();
        }

        public override string ToCode()
        {
            return "(" + _content.ToCode() + ")";
        }

		#endregion
    }
}