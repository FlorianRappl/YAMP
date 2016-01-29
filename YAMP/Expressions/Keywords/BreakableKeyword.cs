using System;

namespace YAMP
{
    /// <summary>
    /// Represents the abstract base class for any keyword that allows the break keyword
    /// to be used in an inner scope.
    /// </summary>
    abstract class BreakableKeyword : BodyKeyword
    {
        #region Members

        bool hasMarker;

        #endregion

        #region ctor

        public BreakableKeyword(string token)
            : base(token)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Breaks the execution of the breakable block.
        /// </summary>
        public abstract void Break();

        protected void SetMarker(ParseEngine engine)
        {
            hasMarker = engine.HasMarker(Marker.Breakable);

            if (!hasMarker)
                engine.InsertMarker(Marker.Breakable);
        }

        protected void UnsetMarker(ParseEngine engine)
        {
            if (!hasMarker)
                engine.RemoveMarker(Marker.Breakable);
        }

        #endregion
    }
}
