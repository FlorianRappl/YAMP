namespace YAMP
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// This class is only used as a dummy placeholder for operators
    /// that have not been found.
    /// </summary>
    sealed class VoidOperator : Operator
    {
        public VoidOperator() 
            : base("")
        {
        }

        public override void RegisterElement(IElementMapping elementMapping)
        {
            //Nothing to do here.
        }

        public override Value Evaluate(Expression[] expressions, IDictionary<String, Value> symbols)
        {
            return null;
        }

        public override Operator Create()
        {
            return new VoidOperator();
        }
    }
}
