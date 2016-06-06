namespace YAMP
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using YAMP.Exceptions;

    /// <summary>
    /// The fat arrow used for lambda expressions.
    /// </summary>
    class FatArrowOperator : BinaryOperator
    {
        public static readonly String Symbol = OpDefinitions.FatArrowOperator;
        public static readonly int OpLevel = OpDefinitions.FatArrowOperatorLevel;

        #region ctor

        public FatArrowOperator()
            : base(Symbol, OpLevel)
        {
        }

        #endregion

        #region Methods

        public override Operator Create()
        {
            return new FatArrowOperator();
        }

        public override Value Handle(Expression left, Expression right, IDictionary<String, Value> symbols)
        {
            var args = default(String[]);

            if (left is SymbolExpression)
            {
                args = new[] { ((SymbolExpression)left).SymbolName };
            }
            else if (left is BracketExpression)
            {
                var bracket = (BracketExpression)left;

                if (bracket.HasContent)
                {
                    if (bracket.IsSymbolList)
                    {
                        var expressions = bracket.GetSymbols().ToArray();
                        args = new String[expressions.Length];

                        for (var i = 0; i != args.Length; i++)
                        {
                            args[i] = expressions[i].SymbolName;
                        }
                    }
                    else
                    {
                        throw new YAMPWrongTypeSuppliedException("a list of symbols");
                    }
                }
                else
                {
                    args = new String[0];
                }
            }
            else
            {
                throw new YAMPWrongTypeSuppliedException("a symbol or a list of symbols contained in round brackets");
            }

            return new FunctionValue(args, right);
        }

        public override Value Perform(Value left, Value right)
        {
            return null;
        }

        #endregion
    }
}
