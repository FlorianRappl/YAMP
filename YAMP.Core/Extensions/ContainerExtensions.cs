namespace YAMP
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    static class ContainerExtensions
    {
        public static Boolean IsAssigned(this ContainerExpression container, SymbolExpression symbol)
        {
            var expressions = container.Expressions;

            if (expressions.Length > 0)
            {
                var leftExpression = expressions[0];
                container = leftExpression as ContainerExpression;

                if (container != null)
                {
                    return container.IsAssigned(symbol);
                }

                return Object.ReferenceEquals(leftExpression, symbol);
            }

            return false;
        }

        public static IEnumerable<SymbolExpression> GetGlobalSymbols(this ContainerExpression container)
        {
            var locals = container.GetLocalSymbols();
            var symbols = container.GetSymbols();
            return symbols.Where(symbol => !locals.Contains(symbol));
        }

        public static IEnumerable<SymbolExpression> GetLocalSymbols(this ContainerExpression container)
        {
            //TODO
            return Enumerable.Empty<SymbolExpression>();
        }

        public static IEnumerable<SymbolExpression> GetSymbols(this ContainerExpression container)
        {
            var list = new List<SymbolExpression>();
            var op = container.Operator as ArgsOperator;
            var expressions = container.Expressions;

            if (expressions != null)
            {
                foreach (var expression in expressions)
                {
                    expression.CollectSymbols(list);
                }
            }

            if (op != null)
            {
                op.Content.CollectSymbols(list);
            }

            return list;
        }
    }
}
