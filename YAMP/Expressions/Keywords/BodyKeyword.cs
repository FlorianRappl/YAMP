using System;

namespace YAMP
{
    /// <summary>
    /// The abstract base class for all keywords with a body.
    /// </summary>
    abstract class BodyKeyword : Keyword
    {
        #region ctor

        public BodyKeyword(string token)
            : base(token)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the (breakable) body statement of the breakable block.
        /// </summary>
        public Statement Body { get; protected set; }

        #endregion
    }
}
