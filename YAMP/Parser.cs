using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace YAMP
{
	public class Parser
	{
		#region Members
		
		Context _expression;
		BracketExpression _interpreter;
		ParseTree _tree;
		
		#endregion
		
		#region ctor
		
		private Parser (Context expression)
		{
			_expression = expression;
			_tree = new ParseTree(expression.Input);
			_interpreter = new BracketExpression(_tree);
		}
		
		public static Parser Parse(string input)
		{
			return new Parser(new Context(input));
		}
		
		public static void Load()
		{
			Tokens.Instance.Touch();
		}
		
		#endregion
		
		#region Properties
		
		public Context Context
		{
			get { return _expression; }
		}
		
		public ParseTree Tree
		{
			get { return _tree; }
		}
		
		#endregion
		
		#region Execution
		
		public Value Execute()
		{
			return Execute(null);
		}
		
		public Value Execute(object values)
		{
			var symbols = new Hashtable();
			
			if(values != null)
			{
				var props = values.GetType().GetProperties();
				
				foreach(var prop in props)
				{
					symbols.Add(prop.Name, prop.GetValue(values, null));
				}
			}
			
			_expression.Output = _interpreter.Interpret(symbols);
			return _expression.Output;
		}
		
		#endregion
		
		#region Customization
		
		public static void AddCustomConstant(string name, double constant)
		{
			Tokens.Instance.AddConstant(name, constant, true);
		}
		
		public static void RemoveCustomConstant(string name)
		{
			Tokens.Instance.RemoveConstant(name);
		}
		
		public static void AddCustomFunction(string name, FunctionDelegate f)
		{
			Tokens.Instance.AddFunction(name, f, true);
		}
		
		public static void RemoveCustomFunction(string name)
		{
			Tokens.Instance.RemoveFunction(name);
		}
		
		#endregion
		
		#region General 
		
		public override string ToString ()
		{
			var sb = new StringBuilder();
			sb.Append("YAMP [ input = ").Append(_expression.Original).AppendLine(" ]");
			sb.AppendLine("--------------");
			sb.Append(_interpreter.Tree.ToString());
			return sb.ToString();
		}
		
		#endregion
	}
}