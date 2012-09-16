using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace YAMP
{
	/// <summary>
	/// The YAMP interaction class.
	/// </summary>
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

		/// <summary>
		/// Creates the parse tree for the given expression.
		/// </summary>
		/// <param name='input'>
		/// The expression to evaluate.
		/// </param>
		public static Parser Parse(string input)
		{
			return new Parser(new Context(input));
		}

		/// <summary>
		/// Load the required functions, operators and expressions (CAN only be performed once).
		/// </summary>
		public static void Load()
		{
			Tokens.Instance.Touch();
		}

		/// <summary>
		/// Gets the currently set variables.
		/// </summary>
		/// <value>
		/// The variables.
		/// </value>
		public static IDictionary<string, Value> Variables
		{
			get { return Tokens.Instance.Variables; }
		}
		
		#endregion
		
		#region Properties

		/// <summary>
		/// Gets the context of the current parser instance (expression, value, ...).
		/// </summary>
		/// <value>
		/// The current context of this parser instance.
		/// </value>
		public Context Context
		{
			get { return _expression; }
		}

		/// <summary>
		/// Gets the expression tree.
		/// </summary>
		/// <value>
		/// The generated expression tree.
		/// </value>
		public ParseTree Tree
		{
			get { return _tree; }
		}
		
		#endregion
		
		#region Execution

		/// <summary>
		/// Execute the evaluation of this parser instance without any external symbols.
		/// </summary>
		public Value Execute()
		{
			return Execute(new Hashtable());
		}

		/// <summary>
		/// Execute the evaluation of this parser instance with external symbols.
		/// </summary>
		/// <param name='values'>
		/// The values in an Hashtable containing string (name), Value (value) pairs.
		/// </param>
		public Value Execute (Hashtable values)
		{
			_expression.Output = _interpreter.Interpret(values);
			Tokens.Instance.AssignVariable("$", _expression.Output);
			
			if(_expression.IsMuted)
				return null;
			
			return _expression.Output;
		}

		/// <summary>
		/// Execute the evaluation of this parser instance with external symbols.
		/// </summary>
		/// <param name='values'>
		/// The values in an anonymous object - containing name - value pairs.
		/// </param>
		public Value Execute(object values)
		{
			var symbols = new Hashtable();
			
			if(values != null)
			{
				var props = values.GetType().GetProperties();
				
				foreach(var prop in props)
					symbols.Add(prop.Name, prop.GetValue(values, null));
			}

			return Execute(symbols);
		}
		
		#endregion
		
		#region Customization

		/// <summary>
		/// Adds a custom constant to the parser.
		/// </summary>
		/// <param name='name'>
		/// The name of the symbol corresponding to the constant.
		/// </param>
		/// <param name='constant'>
		/// The value of the constant.
		/// </param>
		public static void AddCustomConstant(string name, double constant)
		{
			Tokens.Instance.AddConstant(name, constant, true);
		}

		/// <summary>
		/// Adds a custom constant to the parser.
		/// </summary>
		/// <param name='name'>
		/// The name of the symbol corresponding to the constant.
		/// </param>
		/// <param name='constant'>
		/// The value of the constant.
		/// </param>
		public static void AddCustomConstant(string name, Value constant)
		{
			Tokens.Instance.AddConstant(name, constant, true);
		}

		/// <summary>
		/// Removes a custom constant.
		/// </summary>
		/// <param name='name'>
		/// The name of the symbol corresponding to the constant that should be removed.
		/// </param>
		public static void RemoveCustomConstant(string name)
		{
			Tokens.Instance.RemoveConstant(name);
		}

		/// <summary>
		/// Adds a custom function to be used by the parser.
		/// </summary>
		/// <param name='name'>
		/// The name of the symbol corresponding to the function that should be added.
		/// </param>
		/// <param name='f'>
		/// The function that fulfills the signature Value f(Value v).
		/// </param>
		public static void AddCustomFunction(string name, FunctionDelegate f)
		{
			Tokens.Instance.AddFunction(name, f, true);
		}

		/// <summary>
		/// Removes a custom function.
		/// </summary>
		/// <param name='name'>
		/// The name of the symbol corresponding to the function that should be removed.
		/// </param>
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