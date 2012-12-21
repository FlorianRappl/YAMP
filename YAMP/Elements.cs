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
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Reflection;

namespace YAMP
{
	/// <summary>
	/// Provides internal access to the elements and handles the element registration and variable assignment.
	/// </summary>
	sealed class Elements
	{
		#region Members

		IDictionary<string, Operator> operators;
		IDictionary<Regex, Expression> expressions;
		IDictionary<string, Keyword> keywords;
		
		#endregion

		#region ctor

		Elements ()
		{
			operators = new Dictionary<string, Operator>();
			expressions = new Dictionary<Regex, Expression>();
			keywords = new Dictionary<string, Keyword>();
		}
		
		#endregion
		
		#region Register elements
		
		void RegisterElements()
		{
			var assembly = Assembly.GetExecutingAssembly();
			RegisterAssembly(ParseContext.Default, assembly);
		}

		/// <summary>
		/// Registers the IFunction, IConstant and IRegisterToken token classes at the specified context.
		/// </summary>
		/// <param name="context">
		/// The context where the IFunction and IConstant instances will be placed.
		/// </param>
		/// <param name="assembly">
		/// The assembly to load.
		/// </param>
		public void RegisterAssembly(ParseContext context, Assembly assembly)
		{
			var types = assembly.GetTypes();
			var ir = typeof(IRegisterElement).Name;
			var fu = typeof(IFunction).Name;
			var ct = typeof(IConstants).Name;

			foreach (var type in types)
			{
				if (type.IsAbstract)
					continue;

                if (type.Name.EndsWith("Value"))
                    continue;

			    var interfaces = type.GetInterfaces();

				if (interfaces.Any(iface => iface.Name.Equals(ir)))
				{
					var ctor = type.GetConstructor(Value.EmptyTypes);

					if(ctor != null)
						(ctor.Invoke(null) as IRegisterElement).RegisterElement();
				}

                if (interfaces.Any(iface => iface.Name.Equals(fu)))
				{
					var ctor = type.GetConstructor(Value.EmptyTypes);

					if(ctor != null)
					{
						var name = type.Name.RemoveFunctionConvention().ToLower();
						var method = ctor.Invoke(null) as IFunction;
						context.AddFunction(name, method);
					}
				}

                if (interfaces.Any(iface => iface.Name.Equals(ct)))
				{
					var ctor = type.GetConstructor(Value.EmptyTypes);

					if (ctor != null)
					{
						var instc = ctor.Invoke(null) as IConstants;
						context.AddConstant(instc.Name, instc);
					}
				}
			}
		}
		
		#endregion

		#region Add elements
		
		public void AddOperator(string pattern, Operator op)
		{
			operators.Add(pattern, op);
		}

		public void AddExpression(string pattern, Expression exp)
		{
			expressions.Add(new Regex("^" + pattern, RegexOptions.Singleline), exp);
		}

		public void AddKeyword(string pattern, Keyword keyword)
		{
			keywords.Add(pattern, keyword);
		}
		
		#endregion

		#region Find elements

		/// <summary>
		/// Finds the closest matching operator in a given dictionary of expressions and returns null when nothing is found.
		/// </summary>
		/// <param name="expressions">The pool of operators passed in a dictionary.</param>
		/// <param name="context">The query's context.</param>
		/// <param name="input">The current input.</param>
		/// <returns>Operator that matches the beginning of the input string or null.</returns>
		public static Operator FindOperator(IDictionary<string, Operator> operators, QueryContext context, string input)
		{
			var maxop = string.Empty;
			var notfound = true;

			foreach (var op in operators.Keys)
			{
				if (op.Length > input.Length)
					continue;

				if (op.Length <= maxop.Length)
					continue;

				notfound = false;

				for (var i = 0; i < op.Length; i++)
					if (notfound = (input[i] != op[i]))
						break;

				if (notfound == false)
					maxop = op;
			}

			if (maxop.Length == 0)
				return null;

			return operators[maxop].Create(context);
		}

		/// <summary>
		/// Finds the closest matching operator in the available pool and returns null when nothing is found.
		/// </summary>
		/// <param name="context">The query's context.</param>
		/// <param name="input">The current input.</param>
		/// <returns>Operator that matches the beginning of the input string or null.</returns>
		public static Operator FindAvailableOperator(QueryContext context, string input)
		{
			return FindOperator(Instance.operators, context, input);
		}

		/// <summary>
		/// Searches for the given keyword in the list of available keywords. Creates a class if the keyword is found.
		/// </summary>
		/// <param name="context">The current context.</param>
		/// <param name="keyword">The keyword to look for.</param>
		/// <returns>A freshly created instance of the found keyword object.</returns>
		public Keyword FindKeyword(QueryContext context, string keyword)
		{
			if(keywords.ContainsKey(keyword))
				return keywords[keyword].Create(context);

			return null;
		}

		/// <summary>
		/// Finds the closest matching operator and throws and exception when nothing is found.
		/// </summary>
		/// <param name="context">The query's context.</param>
		/// <param name="input">The current input.</param>
		/// <returns>Operator that matches the beginning of the input string.</returns>
		public Operator FindOperator(QueryContext context, string input)
		{
			var op = FindOperator(operators, context, input);

			if (op == null)
				throw new ParseException(input);

			return op;
		}

		/// <summary>
		/// Finds the closest matching expression in a given dictionary of expressions and returns null when nothing is found.
		/// </summary>
		/// <param name="expressions">The pool of expressions passed in a dictionary.</param>
		/// <param name="context">The query's context.</param>
		/// <param name="input">The current input.</param>
		/// <returns>Expression that matches the beginning of the input string or null.</returns>
		public static Expression FindExpression(IDictionary<Regex, Expression> expressions, QueryContext context, string input)
		{
			Match m = null;

			foreach (var rx in expressions.Keys)
			{
				m = rx.Match(input);

				if (m.Success)
					return expressions[rx].Create(context, m);
			}

			return null;
		}

		/// <summary>
		/// Finds the closest matching expression in the available pool and returns null when nothing is found.
		/// </summary>
		/// <param name="context">The query's context.</param>
		/// <param name="input">The current input.</param>
		/// <returns>Expression that matches the beginning of the input string or null.</returns>
		public static Expression FindAvailableExpression(QueryContext context, string input)
		{
			return FindExpression(Instance.expressions, context, input);
		}
		
		/// <summary>
		/// Finds the closest matching expression and throws and exception when nothing is found.
		/// </summary>
		/// <param name="context">The query's context.</param>
		/// <param name="input">The current input.</param>
		/// <returns>Expression that matches the beginning of the input string.</returns>
		public Expression FindExpression(QueryContext context, string input)
		{
			var exp = FindExpression(expressions, context, input);

			if(exp == null)
				throw new ExpressionNotFoundException(input);

			return exp;
		}
		
		#endregion

		#region Singleton

		static Elements _instance;
		
		public static Elements Instance
		{
			get
			{
				if(_instance == null)
				{
					_instance = new Elements();
					_instance.RegisterElements();
				}
				
				return _instance;
			}
		}
		
		#endregion
		
		#region Misc
		
		public void Touch()
		{
			//Empty on intention
		}
		
		#endregion
	}
}