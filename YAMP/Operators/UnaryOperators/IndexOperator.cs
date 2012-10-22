using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace YAMP
{
    class IndexOperator : UnaryOperator
    {
        ArgumentsBracketExpression _content;
        ParseContext _context;
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
            _context = ParseContext.Default;
        }

        public IndexOperator(ParseContext context) : base("(", 1000)
        {
            _context = context;
        }

        public override Operator Create(ParseContext context)
        {
            return new IndexOperator(context);
        }

        public override string Set(string input)
        {
            _content = new ArgumentsBracketExpression(_context);
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
            if (expression is SymbolExpression)
            {
                var sym = expression as SymbolExpression;

                if (sym.IsSymbol && !_context.Variables.ContainsKey(sym.SymbolName))
                    _context.AssignVariable(sym.SymbolName, new MatrixValue());
            }

            var left = expression.Interpret(symbols);
            SetIndexer(left, symbols);
            return Perform(left, value);
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