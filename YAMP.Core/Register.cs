namespace YAMP
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Helpers to handle operator registration.
    /// </summary>
    public static class Register
    {
        static readonly Dictionary<String, BinaryOperatorMappingList> BinaryMappings = new Dictionary<String, BinaryOperatorMappingList>();

        /// <summary>
        /// Provides registration of operator's Mappings list
        /// </summary>
        /// <param name="symbol">The symbol of the operator, e.g., +.</param>
        /// <param name="list">The BinaryMapping list of the operator</param>
        public static void BinaryOperator(String symbol, BinaryOperatorMappingList list)
        {
            var mapping = default(BinaryOperatorMappingList);
            if (!BinaryMappings.TryGetValue(symbol, out mapping))
            {
                BinaryMappings.Add(symbol, list);
            }
        }

        /// <summary>
        /// Registers a global binary operator.
        /// </summary>
        /// <param name="symbol">The symbol of the operator, e.g., +.</param>
        /// <param name="a">The type on the left side of the expression.</param>
        /// <param name="b">The type on the right side of the expression.</param>
        /// <param name="handler">The handler to invoke.</param>
        public static void BinaryOperator(String symbol, Type a, Type b, Func<Value, Value, Value> handler)
        {
            var mapping = default(BinaryOperatorMappingList);

            if (BinaryMappings.TryGetValue(symbol, out mapping))
            {
                var item = new BinaryOperatorMapping(a, b, handler);
                mapping.With(item);
            }
        }
    }
}
