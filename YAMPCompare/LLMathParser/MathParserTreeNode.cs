using System;
using System.Collections.Generic;
using System.Text;

namespace MathParserDataStructures
{
    /// <summary>
    /// MathParser tree node class.
    /// </summary>
    public class MathParserTreeNode  
    {

        private double _value;
        private Operation _operation;
        private Nonterminal _nodeName;

        private MathParserTreeNode _right;
        private MathParserTreeNode _left;


        #region Constructors
        public MathParserTreeNode()
            : base()
        {
            this._operation = Operation.NoOperation;
            this._nodeName = Nonterminal.NoValue;
        }
        /// <summary>
        /// Create a node with specified name.
        /// </summary>
        /// <param name="nodeName">Node's name</param>
        public MathParserTreeNode(Nonterminal nodeName)
            : this()
        {
            _nodeName = nodeName;
        }
        /// <summary>
        /// Craete a node with specified name and operation to perform.
        /// </summary>
        /// <param name="name">Node's name</param>
        /// <param name="operation">Node's operation</param>
        public MathParserTreeNode(Nonterminal nodeName, Operation operation)
            : this(nodeName)
        {
            _operation = operation;

        }
        #endregion

        #region Properties
        /// <summary>
        /// Right child
        /// </summary>
        public MathParserTreeNode Right
        {
            get
            {
                return _right;
            }
            set
            {
                _right = value;
            }
        }
        /// <summary>
        /// Left child
        /// </summary>
        public MathParserTreeNode Left
        {
            get
            {
                return _left;
            }
            set
            {
                _left = value;
            }
        }
        /// <summary>
        /// Value
        /// </summary>
        public double Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        /// <summary>
        /// Node's operation
        /// </summary>
        public Operation Operation
        {
            get
            {
                return _operation;
            }
            set
            {
                _operation = value;
            }
        }
        /// <summary>
        /// Node's name
        /// </summary>
        public Nonterminal Name
        {
            get
            {
                return _nodeName;
            }
            set
            {
                _nodeName = value;
            }
        }
        #endregion

        /// <summary>
        /// Add children to the current node
        /// </summary>
        /// <param name="right">Right node</param>
        /// <param name="left">Left node</param>
        public void AddChildren(MathParserTreeNode left, MathParserTreeNode right)
        {
            this._right = right;
            this._left = left;
        }
        /// <summary>
        /// Add right child.
        /// </summary>
        /// <param name="right">Child to be added</param>
        public void AddRightChild(MathParserTreeNode right)
        {
            this._right = right;
        }
        /// <summary>
        /// Add left child.
        /// </summary>
        /// <param name="left">Child to be added</param>
        public void AddLeftChild(MathParserTreeNode left)
        {
            this._left = left;
        }
       
    }
}
