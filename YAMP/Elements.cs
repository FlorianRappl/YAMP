/*
	Copyright (c) 2012-2014, Florian Rappl.
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
        IDictionary<string, Operator> unaryOperators;
		List<Expression> expressions;
		IDictionary<string, Keyword> keywords;
        IDictionary<int, Plugin> plugins;
		
		#endregion

		#region ctor

		Elements ()
		{
            operators = new Dictionary<string, Operator>();
            unaryOperators = new Dictionary<string, Operator>();
			expressions = new List<Expression>();
			keywords = new Dictionary<string, Keyword>();
            plugins = new Dictionary<int, Plugin>();
            var list = new List<int>();
		}
		
		#endregion

        #region Properties

        /// <summary>
        /// Gets the list of possible keywords.
        /// </summary>
        public string[] Keywords
        {
            get
            {
                return keywords.Keys.ToArray();
            }
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
        /// <returns>The ID for the assembly.</returns>
		public int RegisterAssembly(ParseContext context, Assembly assembly)
		{
            var plugin = new Plugin(context, assembly.FullName);
			var types = assembly.GetTypes();
			var ir = typeof(IRegisterElement).Name;
			var fu = typeof(IFunction).Name;
			var ct = typeof(IConstants).Name;

			foreach (var type in types)
			{
				if (type.IsAbstract)
					continue;

                if (type.Name.EndsWith("Value"))
                {
                    var ctor = type.GetConstructor(Value.EmptyTypes);

                    if (ctor != null)
                    {
                        (ctor.Invoke(null) as IRegisterElement).RegisterElement();
                        plugin.ValueTypes.Add(type.Name.RemoveValueConvention());
                    }

                    continue;
                }

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
                        plugin.Functions.Add(name);
						context.AddFunction(name, method);
					}
				}

                if (interfaces.Any(iface => iface.Name.Equals(ct)))
				{
					var ctor = type.GetConstructor(Value.EmptyTypes);

					if (ctor != null)
					{
                        var instc = ctor.Invoke(null) as IConstants;
                        plugin.Functions.Add(instc.Name);
						context.AddConstant(instc.Name, instc);
					}
				}
			}

            plugins.Add(plugin.Id, plugin);
            return plugin.Id;
		}

        /// <summary>
        /// Removes a previously added assembly.
        /// </summary>
        /// <param name="pluginID">The id of the plugin to remove.</param>
        public void RemoveAssembly(int pluginID)
        {
            if (plugins.ContainsKey(pluginID))
            {
                plugins[pluginID].Uninstall();
            }
        }
		
		#endregion

		#region Add elements
		
        /// <summary>
        /// Adds an operator to the dictionary.
        /// </summary>
        /// <param name="pattern">The operator pattern, i.e. += for add and assign.</param>
        /// <param name="op">The instance of the operator.</param>
		public void AddOperator(string pattern, Operator op)
		{
            if (!op.IsRightToLeft && op.Expressions == 1)
                unaryOperators.Add(pattern, op);
            else
                operators.Add(pattern, op);
		}

        /// <summary>
        /// Adds an expression to the list of expressions.
        /// </summary>
        /// <param name="exp">The instance of the expression.</param>
		public void AddExpression(Expression exp)
		{
			expressions.Add(exp);
		}

        /// <summary>
        /// Adds a keyword to the dictionary.
        /// </summary>
        /// <param name="pattern">The exact keyword pattern, i.e. for for the for-loop.</param>
        /// <param name="keyword">The instance of the keyword.</param>
        public void AddKeyword(string pattern, Keyword keyword)
        {
            keywords.Add(pattern, keyword);
        }
		
		#endregion

		#region Find elements

		/// <summary>
		/// Searches for the given keyword in the list of available keywords. Creates a class if the keyword is found.
		/// </summary>
        /// <param name="keyword">The keyword to look for.</param>
        /// <param name="engine">The engine to use.</param>
		/// <returns>Keyword that matches the given keyword.</returns>
        public Expression FindKeywordExpression(string keyword, ParseEngine engine)
        {
            if (keywords.ContainsKey(keyword))
                return keywords[keyword].Scan(engine);

            return null;
        }

        /// <summary>
        /// Finds the exact keyword by its type.
        /// </summary>
        /// <typeparam name="T">The type of the keyword.</typeparam>
        /// <returns>The keyword or null.</returns>
        public T FindKeywordExpression<T>() where T : Keyword
        {
            foreach (var keyword in keywords.Values)
                if (keyword is T)
                    return (T)keyword;

            return null;
        }

        /// <summary>
        /// Finds the closest matching expression.
        /// </summary>
        /// <param name="engine">The engine to parse the query.</param>
        /// <returns>Expression that matches the current characters.</returns>
        public Expression FindExpression(ParseEngine engine)
        {
            foreach (var origin in expressions)
            {
                var exp = origin.Scan(engine);

                if (exp != null)
                    return exp;
            }

            return null;
        }

        /// <summary>
        /// Finds the exact expression by its type.
        /// </summary>
        /// <typeparam name="T">The type of the expression.</typeparam>
        /// <returns>The expression or null.</returns>
        public T FindExpression<T>() where T : Expression
        {
            foreach (var exp in expressions)
                if (exp is T)
                    return (T)exp;

            return null;
        }

        /// <summary>
        /// Finds the closest matching operator (all except left unary).
        /// </summary>
        /// <param name="engine">The engine to parse the query.</param>
        /// <returns>Operator that matches the current characters.</returns>
        public Operator FindOperator(ParseEngine engine)
        {
            var maxop = FindArbitraryOperator(operators.Keys, engine);

            if (maxop.Length == 0)
                return null;

            return operators[maxop].Create(engine);
        }

        /// <summary>
        /// Finds the closest matching left unary operator.
        /// </summary>
        /// <param name="engine">The engine to parse the query.</param>
        /// <returns>Operator that matches the current characters.</returns>
        public Operator FindLeftUnaryOperator(ParseEngine engine)
        {
            var maxop = FindArbitraryOperator(unaryOperators.Keys, engine);

            if (maxop.Length == 0)
                return null;

            return unaryOperators[maxop].Create(engine);
        }

        string FindArbitraryOperator(IEnumerable<string> operators, ParseEngine engine)
        {
            var maxop = string.Empty;
            var notfound = true;
            var chars = engine.Characters;
            var ptr = engine.Pointer;
            var rest = chars.Length - ptr;

            foreach (var op in operators)
            {
                if (op.Length > rest)
                    continue;

                if (op.Length <= maxop.Length)
                    continue;

                notfound = false;

                for (var i = 0; i < op.Length; i++)
                    if (notfound = (chars[ptr + i] != op[i]))
                        break;

                if (notfound == false)
                    maxop = op;
            }

            return maxop;
        }

        /// <summary>
        /// Finds the exact operator by its type.
        /// </summary>
        /// <typeparam name="T">The type of the operator.</typeparam>
        /// <returns>The operator or null.</returns>
        public T FindOperator<T>() where T : Operator
        {
            foreach (var op in operators.Values)
                if (op is T)
                    return (T)op;

            foreach (var op in unaryOperators.Values)
                if (op is T)
                    return (T)op;

            return null;
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