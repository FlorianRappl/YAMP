namespace YAMP
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    static class ExpressionExtensions
    {
        public static void CollectSymbols(this Expression expression, List<SymbolExpression> list)
        {
            if (expression is ContainerExpression)
            {
                list.AddRange(((ContainerExpression)expression).GetSymbols());
            }
            else if (expression is SymbolExpression)
            {
                list.Add((SymbolExpression)expression);
            }
        }

        public static void CollectLocalSymbols(this Expression expression, List<SymbolExpression> list, IEnumerable<String> locals)
        {
            if (expression is ContainerExpression)
            {
                list.AddRange(((ContainerExpression)expression).GetLocalSymbols(locals));
            }
            else if (expression is SymbolExpression)
            {
                var symbol = (SymbolExpression)expression;

                if (locals.Any(m => m.Equals(symbol.SymbolName)))
                {
                    list.Add(symbol);
                }
            }
        }
    }
}
