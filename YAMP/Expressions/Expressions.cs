using System;
using System.Collections.Generic;
using System.Reflection;
namespace YAMP
{
	public class Expressions
	{	
		AbstractExpression[] _expressions;
		
		private Expressions ()
		{
			_expressions = new AbstractExpression[]
			{
				new FunctionExpression(),
				new BracketExpression(),
				new MultiplyExpression(),
				new DivideExpression(),
				new AddExpression(),
				new SubtractExpression(),
				new SymbolExpression(),
				new NumberExpression()
			};
		}
		
		static Expressions _single;
		
		public static Expressions Instance
		{
			get
			{
				if(_single == null)
					_single = new Expressions();
				
				return _single;
			}
		}
	}
}

