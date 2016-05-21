namespace YAMP
{
    using System;
    using System.Collections.Generic;

    sealed class BinaryOperatorMappingList
    {
        readonly List<BinaryOperatorMapping> _mapping;

        public BinaryOperatorMappingList()
        {
            _mapping = new List<BinaryOperatorMapping>();
        }

        public BinaryOperatorMapping this[Int32 index]
        {
            get { return _mapping[index]; }
        }

        public Int32 Count
        {
            get { return _mapping.Count; }
        }

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
