using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace YAMP
{
    class IndexOperator : UnaryOperator
    {
        ArgumentsBracketExpression _content;
        QueryContext _query;
        _Function _indexer;

        public override string Input
        {
            get
            {
                return _content.Input;
            }
        }

        public IndexOperator() : base("(", 1000)
        {
        }

        public IndexOperator(QueryContext query) : base("(", 1000)
        {
            _query = query;
        }

        public override Operator Create(QueryContext query)
        {
            return new IndexOperator(query);
        }

        public override string Set(string input)
        {
            _content = new ArgumentsBracketExpression(_query);
            return _content.Set(input);
        }

        public override Value Perform(Value left)
        {
            return _indexer.Get();
        }

        public Value Perform(Value left, Value value)
        {
            _indexer.Set(value);
            return left;
        }

        public override Value Handle(Expression expression, Hashtable symbols)
        {
            var left = expression.Interpret(symbols);
            SetIndexer(left, symbols);
            return Perform(left);
        }

        public Value Handle(Expression expression, Value value, Hashtable symbols)
        {
            var isSymbol = expression is SymbolExpression;
            var symbolName = string.Empty;
            var context = _query.Context;

            if (isSymbol)
            {
                var sym = expression as SymbolExpression;
                symbolName = sym.SymbolName;
                isSymbol = sym.IsSymbol;

                if (isSymbol && !context.Variables.ContainsKey(sym.SymbolName))
                    context.AssignVariable(sym.SymbolName, new MatrixValue());
            }

            var left = expression.Interpret(symbols);
            SetIndexer(left, symbols);
            var ret = Perform(left, value);

            if (isSymbol)
                context.AssignVariable(symbolName, ret);

            return ret;
        }

        public override string ToString()
        {
            return base.ToString() + Environment.NewLine + _content.ToString();
        }

        void SetIndexer(Value left, Hashtable symbols)
        {
            _indexer = new _Function(left);
            _indexer.Perform(_content.Interpret(symbols));
        }
    }
}