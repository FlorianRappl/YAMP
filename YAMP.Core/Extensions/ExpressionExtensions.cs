namespace YAMP
{
    using System.Collections.Generic;

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
    }
}
