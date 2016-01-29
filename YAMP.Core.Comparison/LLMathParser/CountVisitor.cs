using System;
using System.Collections.Generic;
using System.Text;

namespace MathParserDataStructures
{
    public class CountVisitor : IVisitor
    {
        private int _count;
        public int Count
        {
            get
            {
                return _count;
            }
        }
        public void ResetCounter()
        {
            _count = 0;
        }
        #region visitor Members

        public void Action(MathParserTreeNode treeNode)
        {
            _count++;
        }

        #endregion
    }
}
