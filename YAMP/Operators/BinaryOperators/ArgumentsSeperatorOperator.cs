using System;

namespace YAMP
{
	class ArgumentsSeperatorOperator : BinaryOperator
	{
		public ArgumentsSeperatorOperator() : base(",", 2)
		{
		}

		public override Value Perform(Value left, Value right)
		{
			return ArgumentsValue.Create(left, right);
		}

		public override void RegisterToken()
		{
			ArgumentsParseTree.Register(this);
		}
	}
}
