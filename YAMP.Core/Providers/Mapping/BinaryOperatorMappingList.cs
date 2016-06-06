namespace YAMP
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A list of binary operators.
    /// </summary>
    public sealed class BinaryOperatorMappingList
    {
        readonly List<BinaryOperatorMapping> _mapping;

        /// <summary>
        /// Creates a new binary mapping list.
        /// </summary>
        public BinaryOperatorMappingList(String symbol)
        {
            _mapping = new List<BinaryOperatorMapping>();

            //Registers itself
            Register.BinaryOperator(symbol, this);
        }

        /// <summary>
        /// Gets the operator at the given index.
        /// </summary>
        public BinaryOperatorMapping this[Int32 index]
        {
            get { return _mapping[index]; }
        }

        /// <summary>
        /// Gets the number of contained operators.
        /// </summary>
        public Int32 Count
        {
            get { return _mapping.Count; }
        }

        /// <summary>
        /// Includes the specified operator in the list.
        /// </summary>
        public void With(BinaryOperatorMapping item)
        {
            var i = 0;
            var count = Count;

            while (i < count && !_mapping[i].Equals(item))
            {
                i++;
            }

            if (i == count)
            {
                _mapping.Add(item);
            }
        }
    }
}
