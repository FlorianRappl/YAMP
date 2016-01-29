using System;
using System.Collections.Generic;

namespace YAMP
{
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

        public override Value Interpret(Dictionary<string, Value> symbols)
        {
            return null;
        }

        public override void RegisterElement()
        {
            //Nothing here.
        }

        public override Expression Scan(ParseEngine engine)
        {
            return new EmptyExpression();
        }

        public override string ToCode()
        {
            return string.Empty;
        }

        #endregion
    }
}
