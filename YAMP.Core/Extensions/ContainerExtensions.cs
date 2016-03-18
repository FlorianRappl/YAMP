namespace YAMP
{
    using System;

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
    }
}
