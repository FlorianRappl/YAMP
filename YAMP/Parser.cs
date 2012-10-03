/*
    Copyright (c) 2012, Florian Rappl.
    All rights reserved.

    Redistribution and use in source and binary forms, with or without
    modification, are permitted provided that the following conditions are met:
        * Redistributions of source code must retain the above copyright
          notice, this list of conditions and the following disclaimer.
        * Redistributions in binary form must reproduce the above copyright
          notice, this list of conditions and the following disclaimer in the
          documentation and/or other materials provided with the distribution.
        * Neither the name of the YAMP team nor the names of its contributors
          may be used to endorse or promote products derived from this
          software without specific prior written permission.

    THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
    ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
    WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
    DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
    DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
    (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
    LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
    ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
    (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
    SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.ComponentModel;
using System.Reflection;

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

        #region static Events

        public static event Action<Value, Exception> OnExecuted;

        #endregion

        #region ctor

        private Parser (Context expression)
		{
			_expression = expression;
			_tree = new ParseTree(expression.Input);
			_interpreter = new BracketExpression(_tree);
		}

        #endregion

        #region Static constructions

        /// <summary>
		/// Creates the parse tree for the given expression.
		/// </summary>
		/// <param name="input">
		/// The expression to evaluate.
		/// </param>
		public static Parser Parse(string input)
		{
			return new Parser(new Context(input));
		}

        /// <summary>
        /// Creates the parse tree and evaluates the expression asynchronously (followed by a continuation with the OnExecuted event).
        /// </summary>
        /// <param name="input">
        /// The expression to evaluate.
        /// </param>
        public static void ExecuteAsync(string input)
        {
            var continuation = OnExecuted;
            
            if(continuation == null)
                continuation = (v, e) => {};

            ExecuteAsync(input, null, continuation);
        }

        /// <summary>
        /// Creates the parse tree and evaluates the expression asynchronously.
        /// </summary>
        /// <param name="input">
        /// The expression to evaluate.
        /// </param>
        /// <param name="continuation">
        /// The continuation action to invoke after the evaluation finished.
        /// </param>
        public static void ExecuteAsync(string input, Action<Value, Exception> continuation)
        {
            ExecuteAsync(input, null, continuation);
        }

        /// <summary>
        /// Creates the parse tree and evaluates the expression asynchronously.
        /// </summary>
        /// <param name="input">
        /// The expression to evaluate.
        /// </param>
        /// <param name="variables">
        /// The variables to consider from external.
        /// </param>
        /// <param name="continuation">
        /// The continuation action to invoke after the evaluation finished.
        /// </param>
        public static void ExecuteAsync(string input, Hashtable variables, Action<Value, Exception> continuation)
        {
            var worker = new AsyncTask();
            worker.Continuation = continuation;
            worker.RunWorkerAsync(new object[] { input, variables });
            worker.DoWork += new DoWorkEventHandler(taskInitialized);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(taskCompleted);
        }

        static void taskInitialized(object sender, DoWorkEventArgs e)
        {
            var parameters = e.Argument as object[];
            var input = parameters[0] as string;
            var variables = parameters[1] as Hashtable;
            var parser = new Parser(new Context(input));
            var result = parser.Execute(variables);
            e.Result = result;
        }

        static void taskCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var worker = sender as AsyncTask;
            worker.Continuation(e.Result as Value, e.Error);
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
		/// <param name="name">
		/// The name of the symbol corresponding to the constant.
		/// </param>
		/// <param name="constant">
		/// The value of the constant.
		/// </param>
		public static void AddCustomConstant(string name, double constant)
		{
			Tokens.Instance.AddConstant(name, constant, true);
		}

		/// <summary>
		/// Adds a custom constant to the parser.
		/// </summary>
		/// <param name="name">
		/// The name of the symbol corresponding to the constant.
		/// </param>
		/// <param name="constant">
		/// The value of the constant.
		/// </param>
		public static void AddCustomConstant(string name, Value constant)
		{
			Tokens.Instance.AddConstant(name, constant, true);
		}

		/// <summary>
		/// Removes a custom constant.
		/// </summary>
		/// <param name="name">
		/// The name of the symbol corresponding to the constant that should be removed.
		/// </param>
		public static void RemoveCustomConstant(string name)
		{
			Tokens.Instance.RemoveConstant(name);
		}

		/// <summary>
		/// Adds a custom function to be used by the parser.
		/// </summary>
		/// <param name="name">
		/// The name of the symbol corresponding to the function that should be added.
		/// </param>
		/// <param name="f">
		/// The function that fulfills the signature Value f(Value v).
		/// </param>
		public static void AddCustomFunction(string name, FunctionDelegate f)
		{
			Tokens.Instance.AddFunction(name, f, true);
		}

		/// <summary>
		/// Removes a custom function.
		/// </summary>
		/// <param name="name">
		/// The name of the symbol corresponding to the function that should be removed.
		/// </param>
		public static void RemoveCustomFunction(string name)
		{
			Tokens.Instance.RemoveFunction(name);
		}

		/// <summary>
		/// Adds a variable to be used by the parser.
		/// </summary>
		/// <param name="name">
		/// The name of the symbol corresponding to the variable that should be added.
		/// </param>
		/// <param name="value">
		/// The value of the variable.
		/// </param>
		public static void AddVariable(string name, Value value)
		{
			Tokens.Instance.Variables.Add(name, value);
		}

		/// <summary>
		/// Removes a variable from the workspace.
		/// </summary>
		/// <param name="name">
		/// The name of the symbol corresponding to the variable that should be removed.
		/// </param>
		public static void RemoveVariable(string name)
		{
            if(Tokens.Instance.Variables.ContainsKey(name))
			    Tokens.Instance.Variables.Remove(name);
		}

        /// <summary>
        /// Loads an external library (assembly) that uses IFunction, Operator, ..., 
        /// </summary>
        /// <param name="assembly">
        /// The assembly to load as a plugin.
        /// </param>
        public static void LoadPlugin(Assembly assembly)
        {
            Tokens.Instance.RegisterAssembly(assembly);
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