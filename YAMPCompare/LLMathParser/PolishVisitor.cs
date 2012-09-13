using System;
using System.Collections.Generic;
using System.Text;

namespace MathParserDataStructures
{
    public class PolishVisitor : IVisitor
    {
        private Operation[] _polish;
        private int _index;
        public PolishVisitor(int sizeOfPolishExpression)
        {
            _polish = new Operation[sizeOfPolishExpression];
        }
        

        #region IVisitor Members

        public void Action(MathParserTreeNode treeNode)
        {
            if (treeNode.Operation == Operation.NoOperation)
                return;
            _polish[_index++] = treeNode.Operation;
        }

        public Operation[] GetPolishPostfixExpression()
        {
            return (Operation[])_polish.Clone(); 
        }

        #endregion
    }
}
