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
using System.Text.RegularExpressions;
using System.Reflection;

namespace YAMP
{
    /// <summary>
    /// Provides internal access to the tokens and handles the token registration and variable assignment.
    /// </summary>
	class Tokens
	{
		#region Members

        IDictionary<string, Operator> operators;
        IDictionary<Regex, Expression> expressions;
		IDictionary<string, string> sanatizers;
		
		#endregion

        #region ctor

        Tokens ()
		{
            operators = new Dictionary<string, Operator>();
            expressions = new Dictionary<Regex, Expression>();
			sanatizers = new Dictionary<string, string>();
		}
		
		#endregion
		
		#region Properties

        /// <summary>
        /// Gets the available sanatizers.
        /// </summary>
		public IDictionary<string, string> Sanatizers
		{
			get { return sanatizers; }
		}
		
		#endregion
		
		#region Register tokens
		
		void RegisterTokens()
		{
			var assembly = Assembly.GetExecutingAssembly();
            RegisterAssembly(ParseContext.Default, assembly);

			sanatizers.Add("++", "+");
			sanatizers.Add("--", "+");
			sanatizers.Add("+-", "-");
			sanatizers.Add("-+", "-");
			sanatizers.Add("//", "*");
			sanatizers.Add("**", "*");
			sanatizers.Add("^*", "^");
			sanatizers.Add("*^", "^");
			sanatizers.Add("*/", "/");
			sanatizers.Add("/*", "/");
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
            var ir = typeof(IRegisterToken).Name;
            var fu = typeof(IFunction).Name;
            var ct = typeof(IConstants).Name;

            foreach (var type in types)
            {
                if (type.IsAbstract)
                    continue;

                if (type.GetInterface(ir) != null)
                    (type.GetConstructor(Type.EmptyTypes).Invoke(null) as IRegisterToken).RegisterToken();

                if (type.GetInterface(fu) != null)
                {
                    var constructor = type.GetConstructor(Type.EmptyTypes);

                    if(constructor != null)
                    {
                        var name = type.Name.RemoveFunctionConvention().ToLower();
                        var method = constructor.Invoke(null) as IFunction;
                        context.AddFunction(name, method);
                    }
                }

                if (type.GetInterface(ct) != null)
                {
                    var props = type.GetProperties();
                    var mycst = type.GetConstructor(Type.EmptyTypes).Invoke(null);

                    foreach (var prop in props)
                    {
                        if (prop.PropertyType.IsSubclassOf(typeof(Value)))
                            context.AddConstant(prop.Name, prop.GetValue(mycst, null) as Value);
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
            expressions.Add(new Regex("^" + pattern), exp);
        }
		
		#endregion

        #region Find elements

		public Operator FindOperator(ParseContext context, string input)
		{
			var maxop = string.Empty;
			var notfound = true;

			foreach(var op in operators.Keys)
			{
				if(op.Length <= maxop.Length)
					continue;

				notfound = false;

				for(var i = 0; i < op.Length; i++)
					if(notfound = (input[i] != op[i]))
						break;

				if(notfound == false)
					maxop = op;
			}

			if(maxop.Length == 0)
				throw new ParseException(input);

			return operators[maxop].Create(context);
		}
		
		public Expression FindExpression(ParseContext context, string input)
		{
			Match m = null;

			foreach(var rx in expressions.Keys)
			{
				m = rx.Match(input);

				if(m.Success)
					return expressions[rx].Create(context, m);
			}
			
			throw new ExpressionNotFoundException(input);
		}
		
		#endregion
		
		#region Singleton
		
		static Tokens _instance;
		
		public static Tokens Instance
		{
			get
			{
				if(_instance == null)
				{
					_instance = new Tokens();
                    _instance.RegisterTokens();
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