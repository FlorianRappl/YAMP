using System;

namespace YAMP
{
    /// <summary>
    /// This is an abstract basic parse block.
    /// </summary>
    public abstract class Block
    {
        #region Properties

        /// <summary>
        /// Gets the line where the block starts in the query.
        /// </summary>
        public int StartLine
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the column where the block starts in the query.
        /// </summary>
        public int StartColumn
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the length in characters of the block.
        /// </summary>
        public int Length
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the associated parse context of the block.
        /// </summary>
        public ParseContext Context
        {
            get
            {
                return Query.Context;
            }
        }

        /// <summary>
        /// Get the corresponding query context of the block.
        /// </summary>
        public QueryContext Query
        {
            get;
            protected set;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the values StartColumn, StartLine and Query.
        /// </summary>
        /// <param name="engine">The engine to use for initialization.</param>
        protected void Init(ParseEngine engine)
        {
            StartColumn = engine.CurrentColumn;
            StartLine = engine.CurrentLine;
            Query = engine.Query;
        }

        /// <summary>
        /// Converts the given block to a valid part of a query.
        /// </summary>
        /// <returns>The string that represents the part of the query.</returns>
        public abstract string ToCode();

        #endregion
    }
}
