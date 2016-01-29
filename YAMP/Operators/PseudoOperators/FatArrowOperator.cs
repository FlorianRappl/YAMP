using System;
using System.Collections.Generic;

namespace YAMP
{
    /// <summary>
    /// The fat arrow used for lambda expressions.
    /// </summary>
    class FatArrowOperator : BinaryOperator
    {
        #region ctor

        public FatArrowOperator() : base("=>", 2)
        {
        }

        #endregion

        #region Methods

        public override Operator Create()
        {
            return new FatArrowOperator();
        }

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

                if (bracket.HasContent)
                {
                    if (bracket.IsSymbolList)
                    {
                        var expressions = bracket.GetSymbols();
                        args = new string[expressions.Length];

                        for (var i = 0; i != args.Length; i++)
                            args[i] = expressions[i].SymbolName;
                    }
                    else
                        throw new YAMPWrongTypeSuppliedException("a list of symbols");
                }
                else
                    args = new string[0];
            }
            else
                throw new YAMPWrongTypeSuppliedException("a symbol or a list of symbols contained in round brackets");

            return new FunctionValue(args, right);
        }

        public override Value Perform(Value left, Value right)
        {
            return null;
        }

        #endregion
    }
}
