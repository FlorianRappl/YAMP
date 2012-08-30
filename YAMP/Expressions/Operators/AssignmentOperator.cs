using System;

namespace YAMP
{
	class AssignmentOperator : Operator
	{
		public AssignmentOperator () : base("=", 0)
		{
		}
		
		public override Value Perform (Value left, Value right)
		{
			if(left is SymbolValue)
				Assign(left as SymbolValue, right);
			
			return right;
		}
		
		public void Assign(SymbolValue left, Value right)
		{
			Console.WriteLine("symbol could be assigned... " + left.Name);
		}
	}
}

