namespace YAMP
{
    using System;

    /// <summary>
    /// Information about a specific matrix index.
    /// </summary>
    public struct MatrixIndex
    {
        #region Fields

        int _row;
        int _column;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new instance and sets the given properties.
        /// </summary>
        /// <param name="row">The 1-based row index.</param>
        /// <param name="column">The 1-based column index.</param>
        public MatrixIndex(int row, int column)
        {
            _row = row;
            _column = column;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the 0-based row index.
        /// </summary>
        public int Row
        {
            get { return _row; }
            set { _row = value; }
        }

        /// <summary>
        /// Gets or sets the 0-based column index.
        /// </summary>
        public int Column
        {
            get { return _column; }
            set { _column = value; }
        }

        #endregion
    }
}
