using System;
using System.Collections.Generic;
using System.Text;

namespace MathParserDataStructures
{
    public class MathParserBinaryTree 
    {
        private MathParserTreeNode _root;

        #region Constructors
        public MathParserBinaryTree()
        {
            _root = new MathParserTreeNode(Nonterminal.NoValue);
        }
        /// <summary>
        /// Create binary tree object with specified root.
        /// </summary>
        /// <param name="root"></param>
        public MathParserBinaryTree(MathParserTreeNode root)
        {
            _root = root;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Root of the binary tree.
        /// </summary>
        public MathParserTreeNode Root
        {
            get
            {
                return _root;
            }
        }
        /// <summary>
        /// Gets the number of nodes in current Binary tree
        /// </summary>
        public int NumberOfNodes
        {
            get 
            {
                CountVisitor countVisitor = new CountVisitor();
                this.Visit(countVisitor);
                return countVisitor.Count;
            }
        }
        #endregion
        /// <summary>
        /// Visit all nodes in the binary tree. Left-Right-Root movement is performed.
        /// </summary>
        /// <param name="visitor">Visitor</param>
        public virtual void Visit(IVisitor visitor)
        {
            Visit(visitor, _root);
        }
        /// <summary>
        /// Visit all nodes in the binary tree.
        /// </summary>
        /// <param name="visitor">Visitor</param>
        /// <param name="curNode">Current node</param>
        protected virtual void Visit(IVisitor visitor, MathParserTreeNode curNode)
        {
            if (curNode != null)
            {
                if (curNode.Left != null)
                    Visit(visitor, curNode.Left);
                if (curNode.Right != null)
                    Visit(visitor, curNode.Right);
                visitor.Action(curNode);
            }

        }
    }
}
