namespace YAMP
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The empty expression - just a dummy!
    /// </summary>
    sealed class EmptyExpression : Expression
    {
        #region ctor

        public EmptyExpression()
        {
        }

        #endregion

        #region Methods

        public override Value Interpret(IDictionary<String, Value> symbols)
        {
            return null;
        }

        public override void RegisterElement(IElementMapping elementMapping)
        {
            //Nothing here.
        }

        public override Expression Scan(ParseEngine engine)
        {
            return new EmptyExpression();
        }

        public override String ToCode()
        {
            return String.Empty;
        }

        #endregion
    }
}
