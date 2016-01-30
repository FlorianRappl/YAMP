namespace YAMP
{ 
    /// <summary>
    /// This is the abstract base class for expressions that contain other
    /// expressions (and operators), i.e. for containing a container expressions.
    /// </summary>
    abstract class TreeExpression : ContainerExpression
    {
        #region ctor

        /// <summary>
        /// Creates a new instance
        /// </summary>
        public TreeExpression()
        {
        }

        /// <summary>
        /// Creates a new instance with some parameters.
        /// </summary>
        /// <param name="child">The child to add.</param>
        /// <param name="query">The associated query context.</param>
        /// <param name="line">The line where the tree expression starts.</param>
        /// <param name="column">The column in the line where the tree exp. starts.</param>
        public TreeExpression(ContainerExpression child, QueryContext query, int line, int column)
            : base(child)
        {
            Query = query;
            StartColumn = column;
            StartLine = line;
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="child">The child to add.</param>
        /// <param name="engine">The engine that has been used.</param>
        public TreeExpression(ContainerExpression child, ParseEngine engine)
            : this(child, engine.Query, engine.CurrentLine, engine.CurrentColumn)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Registers the element.
        /// </summary>
        public override void RegisterElement(Elements elements)
        {
            elements.AddExpression(this);
        }

        #endregion
    }
}
