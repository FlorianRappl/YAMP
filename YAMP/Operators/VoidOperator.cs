using System;

namespace YAMP
{
    /// <summary>
    /// This class is only used as a dummy placeholder for operators
    /// that have not been found.
    /// </summary>
    sealed class VoidOperator : Operator
    {
        public VoidOperator() : base("")
        {
        }

        public override void RegisterElement()
        {
            //Nothing to do here.
        }

        public override Value Evaluate(Expression[] expressions, System.Collections.Generic.Dictionary<string, Value> symbols)
        {
            return null;
        }

        public override Operator Create()
        {
            return new VoidOperator();
        }
    }
}
