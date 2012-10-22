using System;

namespace YAMP
{
    class MatrixColumnOperator : CommaOperator
    {
        public MatrixColumnOperator()
        {
            SetDependency(typeof(MatrixBracketExpression));
        }

        public override Value Perform(Value left, Value right)
        {
            if (!(left is NumericValue))
                throw new OperationNotSupportedException(",", left);

            if (!(right is NumericValue))
                throw new OperationNotSupportedException(",", right);

            return MatrixValue.Create(left).AddColumn(right);
        }

        public override void RegisterToken()
        {
            operators.Add(this);
        }        
    }
}
