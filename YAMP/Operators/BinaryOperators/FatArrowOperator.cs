using System;
using System.Collections.Generic;

namespace YAMP
{
    class FatArrowOperator : BinaryOperator
    {
        #region ctor

        public FatArrowOperator() : base("=>", 2)
        {
        }

        #endregion

        #region Methods

        public override Value Handle(Expression left, Expression right, Dictionary<string, Value> symbols)
        {
            string[] args;

            if (left is SymbolExpression)
            {
                args = new string[] { ((SymbolExpression)left).SymbolName };
            }
            else if (left is BracketExpression)
            {
                var bracket = (BracketExpression)left;

                if (bracket.Tree.HasContent)
                {
                    var expressions = GetAllExpressions(bracket);
                    args = new string[expressions.Length];

                    for (var i = 0; i != args.Length; i++)
                        args[i] = expressions[i].SymbolName;
                }
                else
                    args = new string[0];
            }
            else
                throw new ParseException(left.Offset, left.Input);

            return new FunctionValue(args, right);
        }

        public override Value Perform(Value left, Value right)
        {
            return null;
        }

        SymbolExpression[] GetAllExpressions(TreeExpression tree)
        {
            var list = new List<SymbolExpression>();

            if (tree.Tree.Operator != null && tree.Tree.Operator.Op != ",")
                throw new OperationNotSupportedException(tree.Tree.Operator.Op, "with lambda expressions. Arguments need to be symbols only separated with commas.");

            foreach (var expression in tree.Tree.Expressions)
            {
                if (expression is SymbolExpression)
                    list.Add((SymbolExpression)expression);
                else if (expression is TreeExpression)
                    list.AddRange(GetAllExpressions((TreeExpression)expression));
                else
                    throw new ParseException(expression.Offset, expression.Input);
            }

            return list.ToArray();
        }

        #endregion
    }
}
