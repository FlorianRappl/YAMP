using System;

namespace YAMP
{
	class SymbolValue : Value
	{
		string _symbol;
		
		public override Value Add (Value right)
		{
			throw new System.NotImplementedException();
		}
		
		public override Value Subtract (Value right)
		{
			throw new System.NotImplementedException();
		}
		
		public override Value Multiply (Value right)
		{
			throw new System.NotImplementedException();
		}
		
		public override Value Divide (Value denominator)
		{
			throw new System.NotImplementedException();
		}
		
		public override Value Power (Value exponent)
		{
			throw new System.NotImplementedException();
		}
		
		public override Value Faculty ()
		{
			throw new System.NotImplementedException();
		}
		
		public SymbolValue (string symbol)
		{
			_symbol = symbol;
		}
		
		public string Name
		{
			get { return _symbol; }
		}
	}
}

