using System;

namespace YAMP
{
    class ArgumentsSeperatorOperator : CommaOperator
    {
        public ArgumentsSeperatorOperator()
        {
            SetDependency(typeof(ArgumentsBracketExpression));
        }

        public override Value Perform(Value left, Value right)
        {
            return ArgumentsValue.Create(left, right);
        }

        public override void RegisterToken()
        {
            operators.Add(this);
        }
    }
}
